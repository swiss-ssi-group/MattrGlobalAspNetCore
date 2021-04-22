using Microsoft.Extensions.Configuration;
using BoInsurance.MattrOpenApiClient;
using BoInsurance.Services;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;

namespace BoInsurance
{
    public class MattrVerifyService
    {
        private readonly IConfiguration _configuration;
        private readonly IHttpClientFactory _clientFactory;
        private readonly MattrTokenApiService _mattrTokenApiService;

        public static string MATTR_SANDBOX = "damianbod-sandbox.vii.mattr.global";
        public static string MATTR_DOMAIN = "https://damianbod-sandbox.vii.mattr.global";

        public MattrVerifyService(IConfiguration configuration,
            IHttpClientFactory clientFactory,
            MattrTokenApiService mattrTokenApiService)
        {
            _configuration = configuration;
            _clientFactory = clientFactory;
            _mattrTokenApiService = mattrTokenApiService;
        }

        public async Task<string> CreatePresentationTemplateUrl(string didId)
        {
            //// create a new one
            await CreateMattrVerifier(didId);
            //var driverLicenseCredentials = await CreateMattrDidAndCredentialIssuer();
            //driverLicenseCredentials.Name = name;
            //await _driverLicenseService.CreateDriverLicense(driverLicenseCredentials);

            //var callback = $"https://{MATTR_SANDBOX}/ext/oidc/v1/issuers/{driverLicenseCredentials.OidcIssuerId}/federated/callback";
            return string.Empty;
        }

        private async Task CreateMattrVerifier(string didId)
        {
            HttpClient client = _clientFactory.CreateClient();
            var accessToken = await _mattrTokenApiService.GetApiToken(client, "mattrAccessToken");

            client.DefaultRequestHeaders.Authorization =
                new AuthenticationHeaderValue("Bearer", accessToken);
            client.DefaultRequestHeaders.TryAddWithoutValidation("Content-Type", "application/json");

            var v1PresentationTemplateResponse = await CreateMattrPresentationTemplate(client, didId);
            // Create 
          
        }

        /// <summary>
        /// @context is the JSON-LD schema, which should match the schema as defined in the credential that is being 
        /// requested.
        /// 
        /// type is the credential type that the Mobile Wallet will use to find matching credentials that it holds.
        /// 
        /// The issuer in trustedIssuer filters which Credentials will be acceptable.
        /// An employer, for instance, might only accept the public DIDs of certain universities.
        /// </summary>
        /// <param name="client"></param>
        /// <param name="didId">Did Id of the OIDC used to create the credential verifier</param>
        /// <returns></returns>
        private async Task<V1_PresentationTemplateResponse> CreateMattrPresentationTemplate(
            HttpClient client, string didId)
        {
            // create presentation, post to presentations templates api
            // https://learn.mattr.global/tutorials/verify/presentation-request-template

            var createPresentationsTemplatesUrl = $"https://{MATTR_SANDBOX}/v1/presentations/templates";

            var domain = new Dictionary<string, object>();
            domain.Add("domain", MATTR_SANDBOX);
            var payload = new MattrOpenApiClient.V1_CreatePresentationQueryByExample
            {
                AdditionalProperties = domain,
                Type = "QueryByExample", 
                CredentialQuery = new List<CredentialQuery>
                {
                    new CredentialQuery
                    {
                        Reason = "Please provide your driving license",
                        Required = true,
                        Example = new List<Example>
                        {
                            new Example
                            {
                                Context = new List<object>{ "https://schema.org" },
                                Type = "nationaldrivinglicense",
                                TrustedIssuer = new List<TrustedIssuer2>
                                { 
                                    new TrustedIssuer2
                                    {
                                        Required = true,
                                        Issuer = didId // DID use to create the oidc
                                    }
                                }
                            }
                        }
                    }
                }
            };

            var payloadJson = JsonConvert.SerializeObject(payload);

            var uri = new Uri(createPresentationsTemplatesUrl);

            var result = string.Empty;
            using (var content = new StringContentWithoutCharset(payloadJson, "application/json"))
            {
                var tokenResponse = await client.PostAsync(uri, content);

                if (tokenResponse.StatusCode == System.Net.HttpStatusCode.Created)
                {
        
                    var v1PresentationTemplateResponse = JsonConvert
                            .DeserializeObject<MattrOpenApiClient.V1_PresentationTemplateResponse>(
                            await tokenResponse.Content.ReadAsStringAsync());

                    return v1PresentationTemplateResponse;
                }

                var error = await tokenResponse.Content.ReadAsStringAsync();

            }

            throw new Exception("whoops something went wrong");
        }

    }
    public class MattrOptions
    {
        /// <summary>
        /// The supported key types for the DIDs are ed25519 and bls12381g2. 
        /// If the keyType is omitted, the default key type that will be used is ed25519.
        /// 
        /// If the keyType in options is set to bls12381g2 a DID will be created with 
        /// a BLS key type which supports BBS+ signatures for issuing ZKP-enabled credentials.
        /// </summary>
        public string keyType { get; set; } = "ed25519";
    }
}
