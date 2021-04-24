using Microsoft.Extensions.Configuration;
using BoInsurance.MattrOpenApiClient;
using BoInsurance.Services;
using Newtonsoft.Json;
using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using BoInsurance.Data;

namespace BoInsurance
{
    /// <summary>
    /// https://learn.mattr.global/tutorials/verify/using-callback/callback-e-to-e
    /// </summary>
    public class MattrCredentialVerifyCallbackService
    {
        private readonly IConfiguration _configuration;
        private readonly IHttpClientFactory _clientFactory;
        private readonly MattrTokenApiService _mattrTokenApiService;
        private readonly BoInsuranceDbService _boInsuranceDbService;
        public static string MATTR_SANDBOX = "damianbod-sandbox.vii.mattr.global";
        public static string MATTR_DOMAIN = "https://damianbod-sandbox.vii.mattr.global";
        public static string MATTR_CALLBACK_VERIFY_PATH = "api/Verification/DrivingLicenseCallback";

        public MattrCredentialVerifyCallbackService(IConfiguration configuration,
            IHttpClientFactory clientFactory,
            MattrTokenApiService mattrTokenApiService,
            BoInsuranceDbService boInsuranceDbService)
        {
            _configuration = configuration;
            _clientFactory = clientFactory;
            _mattrTokenApiService = mattrTokenApiService;
            _boInsuranceDbService = boInsuranceDbService;
        }

        /// <summary>
        /// https://learn.mattr.global/tutorials/verify/using-callback/callback-e-to-e
        /// </summary>
        /// <param name="callbackBaseUrl"></param>
        /// <returns></returns>
        public async Task<string> CreateVerifyCallback(string callbackBaseUrl)
        {
            callbackBaseUrl = callbackBaseUrl.Trim();
            if (!callbackBaseUrl.EndsWith('/'))
            {
                callbackBaseUrl = $"{callbackBaseUrl}/";
            }

            var callbackUrlFull = $"{callbackBaseUrl}{MATTR_CALLBACK_VERIFY_PATH}";
            var challenge = Guid.NewGuid().ToString();

            HttpClient client = _clientFactory.CreateClient();
            var accessToken = await _mattrTokenApiService.GetApiToken(client, "mattrAccessToken");

            client.DefaultRequestHeaders.Authorization =
                new AuthenticationHeaderValue("Bearer", accessToken);
            client.DefaultRequestHeaders.TryAddWithoutValidation("Content-Type", "application/json");

            var template = await _boInsuranceDbService.GetLastDriverLicensePrsentationTemplate();

            // Invoke the Presentation Request
            var invokePresentationResponse = await InvokePresentationRequest(
                client,
                template.DidId,
                template.TemplateId,
                challenge,
                callbackUrlFull);

            // Request DID 
            V1_GetDidResponse did = await RequestDID(template.DidId, client);

            // Sign and Encode the Presentation Request body
            var signAndEncodePresentationRequestBodyResponse = await SignAndEncodePresentationRequestBody(
                client, did, invokePresentationResponse);

            // fix strange DTO
            var jws = signAndEncodePresentationRequestBodyResponse.Replace("\"", "");

            // save to db // TODO add this back once working
            var drivingLicensePresentationVerify = new DrivingLicensePresentationVerify
            {
                DidId = template.DidId,
                TemplateId = template.TemplateId,
                CallbackUrl = callbackUrlFull,
                Challenge = challenge,
                InvokePresentationResponse = JsonConvert.SerializeObject(invokePresentationResponse),
                Did = JsonConvert.SerializeObject(did),
                SignAndEncodePresentationRequestBody = jws
            };
            await _boInsuranceDbService.CreateDrivingLicensePresentationVerify(drivingLicensePresentationVerify);

            var qrCodeUrl = $"didcomm://{MATTR_DOMAIN}/?request={jws}";

            return qrCodeUrl;
        }

        private async Task<V1_CreatePresentationRequestResponse> InvokePresentationRequest(
            HttpClient client,
            string didId,
            string templateId,
            string challenge,
            string callbackUrl)
        {
            var createDidUrl = $"https://{MATTR_SANDBOX}/v1/presentations/requests";

            var payload = new MattrOpenApiClient.V1_CreatePresentationRequestRequest
            {
                Did = didId,
                TemplateId = templateId,
                Challenge = challenge,
                CallbackUrl = new Uri(callbackUrl),
                ExpiresTime = 1638836401000 // 10 mins
            };
            var payloadJson = JsonConvert.SerializeObject(payload);
            var uri = new Uri(createDidUrl);

            using (var content = new StringContentWithoutCharset(payloadJson, "application/json"))
            {
                var response = await client.PostAsync(uri, content);

                if (response.StatusCode == System.Net.HttpStatusCode.Created)
                {
                    var v1CreatePresentationRequestResponse = JsonConvert.DeserializeObject<V1_CreatePresentationRequestResponse>(
                            await response.Content.ReadAsStringAsync());

                    return v1CreatePresentationRequestResponse;
                }

                var error = await response.Content.ReadAsStringAsync();
            }

            return null;
        }

        private async Task<V1_GetDidResponse> RequestDID(string didId, HttpClient client)
        {
            var requestUrl = $"{MATTR_DOMAIN}/core/v1/dids/{didId}";
            var uri = new Uri(requestUrl);

            var didResponse = await client.GetAsync(uri);

            if (didResponse.StatusCode == System.Net.HttpStatusCode.OK)
            {
                var v1CreateDidResponse = JsonConvert.DeserializeObject<V1_GetDidResponse>(
                        await didResponse.Content.ReadAsStringAsync());

                return v1CreateDidResponse;
            }

            var error = await didResponse.Content.ReadAsStringAsync();
            return null;
        }

        private async Task<string> SignAndEncodePresentationRequestBody(
            HttpClient client,
            V1_GetDidResponse did,
            V1_CreatePresentationRequestResponse v1CreatePresentationRequestResponse)
        {
            var createDidUrl = $"https://{MATTR_SANDBOX}/v1/messaging/sign";

            object didUrlArray;
            did.DidDocument.AdditionalProperties.TryGetValue("authentication", out didUrlArray);
            var didUrl = didUrlArray.ToString().Split("\"")[1];
            var payload = new MattrOpenApiClient.SignMessageRequest
            {
                DidUrl = didUrl,
                Payload = v1CreatePresentationRequestResponse.Request
            };
            var payloadJson = JsonConvert.SerializeObject(payload);
            var uri = new Uri(createDidUrl);

            using (var content = new StringContentWithoutCharset(payloadJson, "application/json"))
            {
                var response = await client.PostAsync(uri, content);

                if (response.StatusCode == System.Net.HttpStatusCode.OK)
                {
                    var result = await response.Content.ReadAsStringAsync();
                    return result;
                }

                var error = await response.Content.ReadAsStringAsync();
            }

            return null;
        }
    }
}
