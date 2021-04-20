using Microsoft.Extensions.Configuration;
using NationalDrivingLicense.Data;
using NationalDrivingLicense.MattrOpenApiClient;
using NationalDrivingLicense.Services;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;

namespace NationalDrivingLicense
{
    public class MattrCredentialsService
    {
        private readonly IConfiguration _configuration;
        private readonly DriverLicenseCredentialsService _driverLicenseService;
        private readonly IHttpClientFactory _clientFactory;
        private readonly MattrTokenApiService _mattrTokenApiService;

        public static string MATTR_SANDBOX = "damianbod-sandbox.vii.mattr.global";

        public MattrCredentialsService(IConfiguration configuration,
            DriverLicenseCredentialsService driverLicenseService,
            IHttpClientFactory clientFactory,
            MattrTokenApiService mattrTokenApiService)
        {
            _configuration = configuration;
            _driverLicenseService = driverLicenseService;
            _clientFactory = clientFactory;
            _mattrTokenApiService = mattrTokenApiService;
        }

        public async Task<string> CreateCredentialsAndCallback(string name)
        {
            // create a new one
            var driverLicenseCredentials = await CreateMattrDidAndCredentialIssuer();
            driverLicenseCredentials.Name = name;
            await _driverLicenseService.CreateDriverLicense(driverLicenseCredentials);

            var callback = $"https://{MATTR_SANDBOX}/ext/oidc/v1/issuers/{driverLicenseCredentials.OidcIssuerId}/federated/callback";
            return callback;
        }

        private async Task<DriverLicenseCredentials> CreateMattrDidAndCredentialIssuer()
        {
            HttpClient client = _clientFactory.CreateClient();
            var accessToken = await _mattrTokenApiService.GetApiToken(client, "mattrAccessToken");
            client.DefaultRequestHeaders.Authorization =
                new AuthenticationHeaderValue("Bearer", accessToken);
            client.DefaultRequestHeaders.TryAddWithoutValidation("Content-Type", "application/json");

            var did = await CreateMattrDid(client);
            var oidcIssuer = await CreateMattrCredentialIssuer(client, did);

            return new DriverLicenseCredentials
            {
                Name = "not_named",
                Did = JsonConvert.SerializeObject(did),
                OidcIssuer = JsonConvert.SerializeObject(oidcIssuer),
                OidcIssuerId = oidcIssuer.Id
            };
        }

        private async Task<V1_CreateOidcIssuerResponse> CreateMattrCredentialIssuer(HttpClient client, V1_CreateDidResponse did)
        {
            // create vc, post to credentials api
            // https://learn.mattr.global/api-ref/#operation/createCredential
            // https://learn.mattr.global/tutorials/issue/issue-zkp-credential

            // https://tenant.vii.mattr.global/ext/oidc/v1/issuers
            var createCredentialsUrl = $"https://{MATTR_SANDBOX}/ext/oidc/v1/issuers";

            var payload = new MattrOpenApiClient.V1_CreateOidcIssuerRequest
            {
                Credential = new Credential
                {
                    IssuerDid = did.Did,
                    Name = "National Driving License",
                    Context = new List<Uri> { new Uri("https://ndl.org") },
                    Type = new List<string> { "driving_license" }
                },
                ClaimMappings = new List<ClaimMappings>
                {
                    new ClaimMappings{ JsonLdTerm="Name", OidcClaim="https://ndl/name"},
                    new ClaimMappings{ JsonLdTerm="FirstName", OidcClaim="https://ndl/first_name"},
                    new ClaimMappings{ JsonLdTerm="LicenseType", OidcClaim="https://ndl/license_type"},
                    new ClaimMappings{ JsonLdTerm="DateOfBirth", OidcClaim="https://ndl/date_of_birth"},
                    new ClaimMappings{ JsonLdTerm="IssuedAt", OidcClaim="https://ndl/license_issued_at"}
                },
                FederatedProvider = new FederatedProvider
                {
                    ClientId = _configuration["Auth0Wallet:ClientId"],
                    ClientSecret = _configuration["Auth0Wallet:ClientSecret"],
                    Url = new Uri($"https://{_configuration["Auth0Wallet:Domain"]}"),
                    Scope = new List<string> { "openid", "profile", "email"}
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
                    var v1CreateOidcIssuerResponse = JsonConvert.DeserializeObject<V1_CreateOidcIssuerResponse>(
                            await tokenResponse.Content.ReadAsStringAsync());

                    return v1CreateOidcIssuerResponse;
                }

                var error = await tokenResponse.Content.ReadAsStringAsync();

            }

            throw new Exception("whoops something went wrong");
        }

        private async Task<V1_CreateDidResponse> CreateMattrDid(HttpClient client)
        {
            // create did , post to dids 
            // https://learn.mattr.global/api-ref/#operation/createDid
            // https://learn.mattr.global/tutorials/dids/use-did/

            var createDidUrl = $"https://{MATTR_SANDBOX}/core/v1/dids";

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
                    var v1CreateDidResponse = JsonConvert.DeserializeObject<V1_CreateDidResponse>(
                            await tokenResponse.Content.ReadAsStringAsync());

                    return v1CreateDidResponse;
                }

                var error = await tokenResponse.Content.ReadAsStringAsync();
            }

            return null;
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
