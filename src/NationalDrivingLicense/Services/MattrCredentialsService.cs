using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using NationalDrivingLicense.Services;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Threading.Tasks;

namespace NationalDrivingLicense
{
    public class MattrCredentialsService
    {
        private readonly IConfiguration _configuration;
        private readonly DriverLicenseService _driverLicenseService;
        private readonly IHttpClientFactory _clientFactory;
        private readonly MattrConfiguration _mattrConfiguration;
        private readonly MattrTokenApiService _mattrTokenApiService;

        public MattrCredentialsService(IConfiguration configuration,
            DriverLicenseService driverLicenseService,
            IHttpClientFactory clientFactory,
            IOptions<MattrConfiguration> optionsMattrConfiguration,
            MattrTokenApiService mattrTokenApiService)
        {
            _configuration = configuration;
            _driverLicenseService = driverLicenseService;
            _clientFactory = clientFactory;
            _mattrConfiguration = optionsMattrConfiguration.Value;
            _mattrTokenApiService = mattrTokenApiService;
        }

        public async Task<string> GetDriverLicenseCredential(string username)
        {
            if (!await _driverLicenseService.HasIdentityDriverLicense(username))
            {
                throw new ArgumentException("user has no valid driver license");
            }

            var driverLicense = await _driverLicenseService.GetDriverLicense(username);

            if (!string.IsNullOrEmpty(driverLicense.DriverLicenseCredentials))
            {
                return driverLicense.DriverLicenseCredentials;
            }
            IDictionary<string, string> credentialValues = new Dictionary<String, String>() {
                {"Issued At", driverLicense.IssuedAt.ToString()},
                {"Name", driverLicense.Name},
                {"First Name", driverLicense.FirstName},
                {"Date of Birth", driverLicense.DateOfBirth.Date.ToString()},
                {"License Type", driverLicense.LicenseType}
            };

            await CreateMattrVc(credentialValues);

            driverLicense.DriverLicenseCredentials = string.Empty;
            await _driverLicenseService.UpdateDriverLicense(driverLicense);

            return "https://damienbod.com";
        }

        private async Task CreateMattrVc(IDictionary<string, string> credentialValues)
        {
            HttpClient client = _clientFactory.CreateClient();
            var accessToken = await  _mattrTokenApiService.GetApiToken(client, "mattrAccessToken");
            client.DefaultRequestHeaders.Authorization =
                new AuthenticationHeaderValue("Bearer", accessToken);
            client.DefaultRequestHeaders.TryAddWithoutValidation("Content-Type", "application/json");

            // var did = await CreateMattrDid(client);
            var vc = await CreateMattrCredentials(client, credentialValues);

        }

        private async Task<string> CreateMattrCredentials(HttpClient client, IDictionary<string, string> credentialValues)
        {
            // create vc, post to credentials api
            // https://learn.mattr.global/api-ref/#operation/createCredential
            // https://learn.mattr.global/tutorials/issue/issue-zkp-credential

            var createCredentialsUrl = "https://damianbod-sandbox.vii.mattr.global/core/v1/credentials";

            var payload = new MattrOpenApiClient.V1_CreateDidDocument
            {
                Method = MattrOpenApiClient.V1_CreateDidDocumentMethod.Key,
                Options = new MattrOptions()
            };
            var payloadJson = JsonConvert.SerializeObject(payload);
            var uri = new Uri(createCredentialsUrl);

            var result = string.Empty;
            using (var content = new StringContentWithoutCharset(payloadJson, "application/json"))
            {
                var tokenResponse = await client.PostAsync(uri, content);
                
                if (tokenResponse.StatusCode == System.Net.HttpStatusCode.Created)
                {
                    result = await tokenResponse.Content.ReadAsStringAsync();
                    return result;
                }

                var error = await tokenResponse.Content.ReadAsStringAsync();

            }

            return result;
        }

        private async Task<string> CreateMattrDid(HttpClient client)
        {
            // create did , post to dids 
            // https://learn.mattr.global/api-ref/#operation/createDid
            // https://learn.mattr.global/tutorials/dids/use-did/

            var createDidUrl = "https://damianbod-sandbox.vii.mattr.global/core/v1/dids";

            var payload = new MattrOpenApiClient.V1_CreateDidDocument
            {
                Method = MattrOpenApiClient.V1_CreateDidDocumentMethod.Key,
                Options = new MattrOptions()
            };
            var payloadJson = JsonConvert.SerializeObject(payload);
            var uri = new Uri(createDidUrl);

            var result = string.Empty;
            using (var content = new StringContentWithoutCharset(payloadJson, "application/json"))
            {
                var tokenResponse = await client.PostAsync(uri, content);

                if (tokenResponse.StatusCode == System.Net.HttpStatusCode.Created)
                {
                    result = await tokenResponse.Content.ReadAsStringAsync();
                    return result;
                }

                var error = await tokenResponse.Content.ReadAsStringAsync();

            }

            return result;
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
