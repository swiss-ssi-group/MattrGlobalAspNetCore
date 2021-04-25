using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.RazorPages;
using NationalDrivingLicense.Data;

namespace NationalDrivingLicense.Pages
{
    public class DriverLicenseCredentialsModel : PageModel
    {
        private readonly DriverLicenseCredentialsService _driverLicenseCredentialsService;

        public string DriverLicenseMessage { get; set; } = "Loading credentials";
        public bool HasDriverLicense { get; set; } = false;
        public DriverLicense DriverLicense { get; set; }

        public string CredentialOfferUrl { get; set; }
        public DriverLicenseCredentialsModel(DriverLicenseCredentialsService driverLicenseCredentialsService)
        {
            _driverLicenseCredentialsService = driverLicenseCredentialsService;
        }
        public async Task OnGetAsync()
        {
            //"license_issued_at": "2021-03-02",
            //"license_type": "B1",
            //"name": "Bob",
            //"first_name": "Lammy",
            //"date_of_birth": "1953-07-21"

            var identityHasDriverLicenseClaims = true;
            var nameClaim = User.Claims.FirstOrDefault(t => t.Type == $"{Settings.MATTR_DOMAIN}/name");
            var firstNameClaim = User.Claims.FirstOrDefault(t => t.Type == $"{Settings.MATTR_DOMAIN}/first_name");
            var licenseTypeClaim = User.Claims.FirstOrDefault(t => t.Type == $"{Settings.MATTR_DOMAIN}/license_type");
            var dateOfBirthClaim = User.Claims.FirstOrDefault(t => t.Type == $"{Settings.MATTR_DOMAIN}/date_of_birth");
            var licenseIssuedAtClaim = User.Claims.FirstOrDefault(t => t.Type == $"{Settings.MATTR_DOMAIN}/license_issued_at");

            if (nameClaim == null
                || firstNameClaim == null
                || licenseTypeClaim == null
                || dateOfBirthClaim == null
                || licenseIssuedAtClaim == null)
            {
                identityHasDriverLicenseClaims = false;
            }

            if (identityHasDriverLicenseClaims)
            {
                DriverLicense = new DriverLicense
                {
                    Name = nameClaim.Value,
                    FirstName = firstNameClaim.Value,
                    LicenseType = licenseTypeClaim.Value,
                    DateOfBirth = dateOfBirthClaim.Value,
                    IssuedAt = licenseIssuedAtClaim.Value,
                    UserName = User.Identity.Name

                };
                // get per name
                //var offerUrl = await _driverLicenseCredentialsService.GetDriverLicenseCredentialIssuerUrl("ndlseven");

                // get the last one
                var offerUrl = await _driverLicenseCredentialsService.GetLastDriverLicenseCredentialIssuerUrl();

                DriverLicenseMessage = "Add your driver license credentials to your wallet";
                CredentialOfferUrl = offerUrl;
                HasDriverLicense = true;
            }
            else
            {
                DriverLicenseMessage = "You have no valid driver license";
            }
        }
    }
}
