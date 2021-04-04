using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace NationalDrivingLicense
{
    public class MattrCredentialsService
    {
        private readonly IConfiguration _configuration;
        private readonly DriverLicenseService _driverLicenseService;

        public MattrCredentialsService(
            IConfiguration configuration,
            DriverLicenseService driverLicenseService)
        {
            _configuration = configuration;
            _driverLicenseService = driverLicenseService;
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

            // create vc

            driverLicense.DriverLicenseCredentials = string.Empty;
            await _driverLicenseService.UpdateDriverLicense(driverLicense);

            return "https://damienbod.com";
        }
    }
}
