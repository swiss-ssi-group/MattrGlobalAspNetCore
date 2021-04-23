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
        /// <param name="callbackUrl"></param>
        /// <returns></returns>
        public async Task<string> CreateVerifyCallback(string callbackUrl)
        {
            HttpClient client = _clientFactory.CreateClient();
            var accessToken = await _mattrTokenApiService.GetApiToken(client, "mattrAccessToken");

            client.DefaultRequestHeaders.Authorization =
                new AuthenticationHeaderValue("Bearer", accessToken);
            client.DefaultRequestHeaders.TryAddWithoutValidation("Content-Type", "application/json");

            var template = await _boInsuranceDbService.GetLastDriverLicensePrsentationTemplate();

            // Invoke the Presentation Request
            var invokePresentationResponse = await InvokePresentationRequest();

            // Request DID 
            var did = await RequestDID();

            // Sign and Encode the Presentation Request body
            var signAndEncodePresentationRequestBody = SignAndEncodePresentationRequestBody();

            // save to db // TODO add this back once working
            //var drivingLicensePresentationVerify = new DrivingLicensePresentationVerify
            //{
            //    DidId = template.DidId,
            //    TemplateId = template.TemplateId,
            //    CallbackUrl = callbackUrl,
            //    InvokePresentationResponse = JsonConvert.SerializeObject(invokePresentationResponse),
            //    Did = JsonConvert.SerializeObject(did),
            //    SignAndEncodePresentationRequestBody = JsonConvert.SerializeObject(signAndEncodePresentationRequestBody)
            //};
            //await _boInsuranceDbService.CreateDrivingLicensePresentationVerify(drivingLicensePresentationVerify);

            var jws = "sometest";
            var qrCodeUrl = $"didcomm://{MATTR_DOMAIN}/?request={jws}";

            return qrCodeUrl;
        }

        private async Task<string> InvokePresentationRequest()
        {
            return string.Empty;
        }

        private async Task<string> RequestDID()
        {
            return string.Empty;
        }

        private async Task<string> SignAndEncodePresentationRequestBody()
        {
            return string.Empty;
        }
    }
}
