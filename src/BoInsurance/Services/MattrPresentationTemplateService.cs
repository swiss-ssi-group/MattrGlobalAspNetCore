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
        private readonly IHttpClientFactory _clientFactory;
        private readonly MattrTokenApiService _mattrTokenApiService;
        private readonly BoInsuranceDbService _boInsuranceDbService;
        public static string MATTR_SANDBOX = "damianbod-sandbox.vii.mattr.global";
        public static string MATTR_DOMAIN = "https://damianbod-sandbox.vii.mattr.global";

        public MattrPresentationTemplateService(IHttpClientFactory clientFactory,
            MattrTokenApiService mattrTokenApiService,
            BoInsuranceDbService boInsuranceDbService)
        {
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

            var additionalProperties = new Dictionary<string, object>();
            additionalProperties.Add("type", "QueryByExample");
            additionalProperties.Add("credentialQuery", new List<CredentialQuery> {
                new CredentialQuery
                {
                    Reason = "Please provide your driving license",
                    Required = true,
                    Example = new Example
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
            });

            var payload = new MattrOpenApiClient.V1_CreatePresentationTemplate
            {
                Domain = MATTR_SANDBOX,
                Name = "certificate-presentation",
                Query = new List<Query>
                {
                    new Query
                    {
                        AdditionalProperties = additionalProperties
                    }
                }
            };

            var payloadJson = JsonConvert.SerializeObject(payload);

            var uri = new Uri(createPresentationsTemplatesUrl);

            using (var content = new StringContentWithoutCharset(payloadJson, "application/json"))
            {
                var presentationTemplateResponse = await client.PostAsync(uri, content);

                if (presentationTemplateResponse.StatusCode == System.Net.HttpStatusCode.Created)
                {

                    var v1PresentationTemplateResponse = JsonConvert
                            .DeserializeObject<MattrOpenApiClient.V1_PresentationTemplateResponse>(
                            await presentationTemplateResponse.Content.ReadAsStringAsync());

                    return v1PresentationTemplateResponse;
                }

                var error = await presentationTemplateResponse.Content.ReadAsStringAsync();

            }

            throw new Exception("whoops something went wrong");
        }
    }
}
