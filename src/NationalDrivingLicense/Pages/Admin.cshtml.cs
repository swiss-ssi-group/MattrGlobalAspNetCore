using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.RazorPages;
using NationalDrivingLicense.Data;

namespace NationalDrivingLicense.Pages
{
    public class AdminModel : PageModel
    {
        private readonly MattrCredentialsService _mattrCredentialsService;

        public string DriverLicenseMessage { get; set; } = "Loading credentials";
        public string Callback { get; set; }
        public AdminModel(MattrCredentialsService mattrCredentialsService)
        {
            _mattrCredentialsService = mattrCredentialsService;
        }
        public async Task OnGetAsync()
        {

        }

        public async Task OnPostAsync()
        {
            Callback = await _mattrCredentialsService.CreateCredentialsAndCallback("ndl");
        }
    }
}
