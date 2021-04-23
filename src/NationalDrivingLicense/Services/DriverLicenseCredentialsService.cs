﻿using Microsoft.EntityFrameworkCore;
using NationalDrivingLicense.Data;
using System.Linq;
using System.Threading.Tasks;

namespace NationalDrivingLicense
{
    public class DriverLicenseCredentialsService
    {
        private readonly NationalDrivingLicenseMattrContext _nationalDrivingLicenseMattrContext;

        public DriverLicenseCredentialsService(NationalDrivingLicenseMattrContext nationalDrivingLicenseMattrContext)
        {
            _nationalDrivingLicenseMattrContext = nationalDrivingLicenseMattrContext;
        }

        public async Task<(string Callback, string Template, string DidId)> GetLastDriverLicenseCredentialIssuer()
        {
            var driverLicenseCredentials = await _nationalDrivingLicenseMattrContext
                .DriverLicenseCredentials
                .OrderBy(u => u.Id)
                .LastOrDefaultAsync();

            if (driverLicenseCredentials != null)
            {
                var callback = $"https://{MattrCredentialsService.MATTR_SANDBOX}/ext/oidc/v1/issuers/{driverLicenseCredentials.OidcIssuerId}/federated/callback";

                return (callback, "temp", "did:...");
            }

            return (string.Empty, string.Empty, string.Empty);
        }

        public async Task<string> GetLastDriverLicenseCredentialIssuerUrl()
        {
            var driverLicense = await _nationalDrivingLicenseMattrContext
                .DriverLicenseCredentials
                .OrderBy(u => u.Id)
                .LastOrDefaultAsync();

            if(driverLicense != null)
            {
                var url = $"openid://discovery?issuer=https://{MattrCredentialsService.MATTR_SANDBOX}/ext/oidc/v1/issuers/{driverLicense.OidcIssuerId}";
                return url;
            }

            return string.Empty;
        }

        public async Task<string> GetDriverLicenseCredentialIssuerUrl(string name)
        {
            var driverLicense = await _nationalDrivingLicenseMattrContext
                .DriverLicenseCredentials
                .FirstOrDefaultAsync(dl => dl.Name == name);

            if (driverLicense != null)
            {
                var url = $"openid://discovery?issuer=https://{MattrCredentialsService.MATTR_SANDBOX}/ext/oidc/v1/issuers/{driverLicense.OidcIssuerId}";
                return url;
            }

            return string.Empty;
        }

        public async Task CreateDriverLicense(DriverLicenseCredentials driverLicense)
        {
            _nationalDrivingLicenseMattrContext.DriverLicenseCredentials.Add(driverLicense);
            await _nationalDrivingLicenseMattrContext.SaveChangesAsync();
        }
    }
}
