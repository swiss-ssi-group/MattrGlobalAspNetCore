using Microsoft.EntityFrameworkCore;
using NationalDrivingLicense.Data;
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

        public async Task<string> GetDriverLicenseCredentialIssuerUrl(string name)
        {
            var driverLicense = await _nationalDrivingLicenseMattrContext.DriverLicenseCredentials.FirstOrDefaultAsync(
                    dl => dl.Name == name
                );
            
            var url = $"openid://discovery?issuer=https://{MattrCredentialsService.MATTR_SANDBOX}/ext/oidc/v1/issuers/{driverLicense.OidcIssuerId}";

            //var url = $"openid://discovery?issuer=https://tenant.vii.mattr.global/ext/oidc/v1/issuers/{driverLicense.OidcIssuerId}";
            return url;
        }

        public async Task CreateDriverLicense(DriverLicenseCredentials driverLicense)
        {
            _nationalDrivingLicenseMattrContext.DriverLicenseCredentials.Add(driverLicense);
            await _nationalDrivingLicenseMattrContext.SaveChangesAsync();
        }
    }
}
