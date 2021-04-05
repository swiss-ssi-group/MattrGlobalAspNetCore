using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;

namespace NationalDrivingLicense
{
    public class MattrCredentialsService
    {
        private readonly IConfiguration _configuration;
        private readonly DriverLicenseService _driverLicenseService;
        private readonly IHttpClientFactory _clientFactory;

        public MattrCredentialsService(IConfiguration configuration,
            DriverLicenseService driverLicenseService,
            IHttpClientFactory clientFactory)
        {
            _configuration = configuration;
            _driverLicenseService = driverLicenseService;
            _clientFactory = clientFactory;
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

            CreateMattrVc(credentialValues);

            driverLicense.DriverLicenseCredentials = string.Empty;
            await _driverLicenseService.UpdateDriverLicense(driverLicense);

            return "https://damienbod.com";
        }

        private void CreateMattrVc(IDictionary<string, string> credentialValues)
        {
            HttpClient client = _clientFactory.CreateClient();
            CreateMattrDid(client);
            // create vc, post to credentials api
        }

        private void CreateMattrDid(HttpClient client)
        {
            // create did , post to dids 
        }

    }
}
