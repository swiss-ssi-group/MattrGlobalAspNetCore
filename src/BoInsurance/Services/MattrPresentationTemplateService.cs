using Microsoft.Extensions.Configuration;
using BoInsurance.MattrOpenApiClient;
using BoInsurance.Services;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using BoInsurance.Data;

namespace BoInsurance
{
    public class MattrPresentationTemplateService
    {
        private readonly IConfiguration _configuration;
        private readonly IHttpClientFactory _clientFactory;
        private readonly MattrTokenApiService _mattrTokenApiService;
        private readonly BoInsuranceDbService _boInsuranceDbService;
        public static string MATTR_SANDBOX = "damianbod-sandbox.vii.mattr.global";
        public static string MATTR_DOMAIN = "https://damianbod-sandbox.vii.mattr.global";

        public MattrPresentationTemplateService(IConfiguration configuration,
            IHttpClientFactory clientFactory,
            MattrTokenApiService mattrTokenApiService,
            BoInsuranceDbService boInsuranceDbService)
        {
            _configuration = configuration;
            _clientFactory = clientFactory;
            _mattrTokenApiService = mattrTokenApiService;
            _boInsuranceDbService = boInsuranceDbService;
        }

        public async Task<string> CreatePresentationTemplateId(string didId)
        {
            // create a new one
            var v1PresentationTemplateResponse = await CreateMattrPresentationTemplate(didId);

            // save to db
            var drivingLicensePresentationTemplate = new DrivingLicensePresentationTemplate
            {
                DidId = didId,
                TemplateId = v1PresentationTemplateResponse.Id,
                MattrPresentationTemplateReponse = JsonConvert.SerializeObject(v1PresentationTemplateResponse)
            };
            await _boInsuranceDbService.CreateDriverLicensePresentationTemplate(drivingLicensePresentationTemplate);

            return v1PresentationTemplateResponse.Id;
        }

        private async Task<V1_PresentationTemplateResponse> CreateMattrPresentationTemplate(string didId)
        {
            HttpClient client = _clientFactory.CreateClient();
            var accessToken = await _mattrTokenApiService.GetApiToken(client, "mattrAccessToken");

            client.DefaultRequestHeaders.Authorization =
                new AuthenticationHeaderValue("Bearer", accessToken);
            client.DefaultRequestHeaders.TryAddWithoutValidation("Content-Type", "application/json");

            var v1PresentationTemplateResponse = await CreateMattrPresentationTemplate(client, didId);
            return v1PresentationTemplateResponse;
        }

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
                                Type = "VerifiableCredential",
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
}
