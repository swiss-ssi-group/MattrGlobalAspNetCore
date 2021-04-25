using Microsoft.EntityFrameworkCore;
using NationalDrivingLicense.Data;
using NationalDrivingLicense.MattrOpenApiClient;
using Newtonsoft.Json;
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

        public async Task<(string Callback, string DidId)> GetLastDriverLicenseCredentialIssuer()
        {
            var driverLicenseCredentials = await _nationalDrivingLicenseMattrContext
                .DriverLicenseCredentials
                .OrderBy(u => u.Id)
                .LastOrDefaultAsync();

            if (driverLicenseCredentials != null)
            {
                var callback = $"{Settings.MATTR_DOMAIN}/ext/oidc/v1/issuers/{driverLicenseCredentials.OidcIssuerId}/federated/callback";
                var oidcCredentialIssuer = JsonConvert.DeserializeObject<V1_CreateOidcIssuerResponse>(driverLicenseCredentials.OidcIssuer);
                return (callback, oidcCredentialIssuer.Credential.IssuerDid);
            }

            return (string.Empty, string.Empty);
        }

        public async Task<string> GetLastDriverLicenseCredentialIssuerUrl()
        {
            var driverLicense = await _nationalDrivingLicenseMattrContext
                .DriverLicenseCredentials
                .OrderBy(u => u.Id)
                .LastOrDefaultAsync();

            if (driverLicense != null)
            {
                var url = $"openid://discovery?issuer={Settings.MATTR_DOMAIN}/ext/oidc/v1/issuers/{driverLicense.OidcIssuerId}";
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
                var url = $"openid://discovery?issuer={Settings.MATTR_DOMAIN}/ext/oidc/v1/issuers/{driverLicense.OidcIssuerId}";
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
