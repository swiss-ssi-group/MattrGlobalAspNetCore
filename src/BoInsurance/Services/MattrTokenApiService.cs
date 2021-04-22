using System;
using System.Threading.Tasks;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using System.Net.Http;
using System.Net.Http.Json;

namespace BoInsurance.Services
{
    public class MattrTokenApiService
    {
        private readonly ILogger<MattrTokenApiService> _logger;
        private readonly MattrConfiguration _mattrConfiguration;

        private static readonly Object _lock = new Object();
        private IDistributedCache _cache;

        private const int cacheExpirationInDays = 1;

        private class AccessTokenResult
        {
            public string AcessToken { get; set; } = string.Empty;
            public DateTime ExpiresIn { get; set; }
        }

        private class AccessTokenItem
        {
            public string access_token { get; set; } = string.Empty;
            public int expires_in { get; set; }
            public string token_type { get; set; }
            public string scope { get; set; }
        }

        private class MattrCrendentials
        {
            public string audience { get; set; }
            public string client_id { get; set; }
            public string client_secret { get; set; }
            public string grant_type { get; set; } = "client_credentials";
        }

        public MattrTokenApiService(
                IOptions<MattrConfiguration> mattrConfiguration,
                IHttpClientFactory httpClientFactory,
                ILoggerFactory loggerFactory,
                IDistributedCache cache)
        {
            _mattrConfiguration = mattrConfiguration.Value;
            _logger = loggerFactory.CreateLogger<MattrTokenApiService>();
            _cache = cache;
        }

        public async Task<string> GetApiToken(HttpClient client, string api_name)
        {
            var accessToken = GetFromCache(api_name);

            if (accessToken != null)
            {
                if (accessToken.ExpiresIn > DateTime.UtcNow)
                {
                    return accessToken.AcessToken;
                }
                else
                {
                    // remove  => NOT Needed for this cache type
                }
            }

            _logger.LogDebug($"GetApiToken new from oauth server for {api_name}");

            // add
            var newAccessToken = await GetApiTokenClient(client);
            AddToCache(api_name, newAccessToken);

            return newAccessToken.AcessToken;
        }

        private async Task<AccessTokenResult> GetApiTokenClient(HttpClient client)
        {
            try
            {
                var payload = new MattrCrendentials
                {
                    client_id = _mattrConfiguration.ClientId,
                    client_secret = _mattrConfiguration.ClientSecret,
                    audience = _mattrConfiguration.Audience
                };

                var authUrl = "https://auth.mattr.global/oauth/token";
                var tokenResponse = await client.PostAsJsonAsync(authUrl, payload);

                if (tokenResponse.StatusCode == System.Net.HttpStatusCode.OK)
                {
                    var result = await tokenResponse.Content.ReadFromJsonAsync<AccessTokenItem>();
                    DateTime expirationTime = DateTimeOffset.FromUnixTimeSeconds(result.expires_in).DateTime;
                    return new AccessTokenResult
                    {
                        AcessToken = result.access_token,
                        ExpiresIn = expirationTime
                    };
                }

                _logger.LogError($"tokenResponse.IsError Status code: {tokenResponse.StatusCode}, Error: {tokenResponse.ReasonPhrase}");
                throw new ApplicationException($"Status code: {tokenResponse.StatusCode}, Error: {tokenResponse.ReasonPhrase}");
            }
            catch (Exception e)
            {
                _logger.LogError($"Exception {e}");
                throw new ApplicationException($"Exception {e}");
            }
        }

        private void AddToCache(string key, AccessTokenResult accessTokenItem)
        {
            var options = new DistributedCacheEntryOptions().SetSlidingExpiration(TimeSpan.FromDays(cacheExpirationInDays));

            lock (_lock)
            {
                _cache.SetString(key, JsonConvert.SerializeObject(accessTokenItem), options);
            }
        }

        private AccessTokenResult GetFromCache(string key)
        {
            var item = _cache.GetString(key);
            if (item != null)
            {
                return JsonConvert.DeserializeObject<AccessTokenResult>(item);
            }

            return null;
        }
    }

}
