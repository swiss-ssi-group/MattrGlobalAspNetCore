using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace NationalDrivingLicense.Pages
{
    public class ViewLastCredentialsInfoModel : PageModel
    {
        private readonly DriverLicenseCredentialsService _driverLicenseCredentialsService;

        public string LatestDriverLicenseDid { get; set; }
        public string LatestDriverLicenseCallback { get; set; }

        public string CredentialOfferUrl { get; set; }
        public ViewLastCredentialsInfoModel(DriverLicenseCredentialsService driverLicenseCredentialsService)
        {
            _driverLicenseCredentialsService = driverLicenseCredentialsService;
        }
        public async Task OnGetAsync()
        {
            var driverLicenseCredentialIssuer = await _driverLicenseCredentialsService.GetLastDriverLicenseCredentialIssuer();
            LatestDriverLicenseCallback = driverLicenseCredentialIssuer.Callback;
            LatestDriverLicenseDid = driverLicenseCredentialIssuer.DidId;
        }
    }
}
