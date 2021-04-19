using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using NationalDrivingLicense.Data;
using NationalDrivingLicense.MattrOpenApiClient;
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
        private readonly DriverLicenseCredentialsService _driverLicenseService;
        private readonly IHttpClientFactory _clientFactory;
        private readonly MattrConfiguration _mattrConfiguration;
        private readonly MattrTokenApiService _mattrTokenApiService;

        public MattrCredentialsService(IConfiguration configuration,
            DriverLicenseCredentialsService driverLicenseService,
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

            var driverLicenseExists = await _driverLicenseService.GetDriverLicense(username);

            if (driverLicenseExists != null && !string.IsNullOrEmpty(driverLicenseExists.OfferUrl))
            {
                return driverLicenseExists.OfferUrl;
            }

            // create a new one
            var driverLicenseCredentials = await CreateMattrVc();
            await _driverLicenseService.UpdateDriverLicense(driverLicenseCredentials);
            return driverLicenseCredentials.OfferUrl;
        }

        private async Task<DriverLicenseCredentials> CreateMattrVc()
        {
            HttpClient client = _clientFactory.CreateClient();
            var accessToken = await _mattrTokenApiService.GetApiToken(client, "mattrAccessToken");
            client.DefaultRequestHeaders.Authorization =
                new AuthenticationHeaderValue("Bearer", accessToken);
            client.DefaultRequestHeaders.TryAddWithoutValidation("Content-Type", "application/json");

            // var did = await CreateMattrDid(client);
            var vc = await CreateMattrCredentials(client);
            return vc;
        }

        
        private async Task<DriverLicenseCredentials> CreateMattrCredentials(HttpClient client)
        {
            // create vc, post to credentials api
            // https://learn.mattr.global/api-ref/#operation/createCredential
            // https://learn.mattr.global/tutorials/issue/issue-zkp-credential

            // https://tenant.vii.mattr.global/ext/oidc/v1/issuers
            var createCredentialsUrl = "https://damianbod-sandbox.vii.mattr.global/ext/oidc/v1/issuers";

            var payload = new MattrOpenApiClient.V1_CreateOidcIssuerRequest 
            {
                Credential = new Credential
                {
                    IssuerDid = "",
                    Name = "National Driving License",
                    Type = new List<string> { "driving_license" }
                },
                ClaimMappings = new List<ClaimMappings>
                {
                    new ClaimMappings{ JsonLdTerm="Name", OidcClaim="https://ndl/name"},
                    new ClaimMappings{ JsonLdTerm="First Name", OidcClaim="https://ndl/first_name"},
                    new ClaimMappings{ JsonLdTerm="License Type", OidcClaim="https://ndl/license_type"},
                    new ClaimMappings{ JsonLdTerm="Date of Birth", OidcClaim="https://ndl/date_of_birth"},
                    new ClaimMappings{ JsonLdTerm="Issued At", OidcClaim="https://ndl/license_issued_at"}
                },
                FederatedProvider = new FederatedProvider
                {
                    ClientId = _configuration["Auth0:ClientId"],
                    ClientSecret = _configuration["Auth0:ClientSecret"],
                    Scope = new List<string> { "openid", "profile", "email"}, 
                    Url = new Uri("https://dev-damienbod.eu.auth0.com")
                }
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
                    return new DriverLicenseCredentials
                    {
                        // TODO set data
                    };
                }

                var error = await tokenResponse.Content.ReadAsStringAsync();

            }

            return null;
        }

        //private async Task<DriverLicenseCredentials> CreateMattrCredentials(HttpClient client)
        //{
        //    // create vc, post to credentials api
        //    // https://learn.mattr.global/api-ref/#operation/createCredential
        //    // https://learn.mattr.global/tutorials/issue/issue-zkp-credential

        //    var createCredentialsUrl = "https://damianbod-sandbox.vii.mattr.global/core/v1/credentials";

        //    var payload = new MattrOpenApiClient.V1_CreateDidDocument
        //    {
        //        Method = MattrOpenApiClient.V1_CreateDidDocumentMethod.Key,
        //        Options = new MattrOptions()
        //    };
        //    var payloadJson = JsonConvert.SerializeObject(payload);
        //    var uri = new Uri(createCredentialsUrl);

        //    var result = string.Empty;
        //    using (var content = new StringContentWithoutCharset(payloadJson, "application/json"))
        //    {
        //        var tokenResponse = await client.PostAsync(uri, content);

        //        if (tokenResponse.StatusCode == System.Net.HttpStatusCode.Created)
        //        {
        //            result = await tokenResponse.Content.ReadAsStringAsync();
        //            return new DriverLicenseCredentials
        //            {
        //                // TODO set data
        //            };
        //        }

        //        var error = await tokenResponse.Content.ReadAsStringAsync();

        //    }

        //    return null;
        //}

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
