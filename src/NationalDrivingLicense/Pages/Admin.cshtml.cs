using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using NationalDrivingLicense.Data;

namespace NationalDrivingLicense.Pages
{
    public class AdminModel : PageModel
    {
        private readonly MattrCredentialsService _mattrCredentialsService;

        public bool CreatingDriverLicense { get; set; } = true;
        public string Callback { get; set; }

        [BindProperty]
        public IssuerCredential IssuerCredential { get; set; }
        public AdminModel(MattrCredentialsService mattrCredentialsService)
        {
            _mattrCredentialsService = mattrCredentialsService;
        }
        public void OnGet()
        {
            IssuerCredential = new IssuerCredential();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            Callback = await _mattrCredentialsService.CreateCredentialsAndCallback(IssuerCredential.CredentialName);
            CreatingDriverLicense = false;
            return Page();
        }
    }

    public class IssuerCredential
    {
        [Required]
        public string CredentialName { get; set; }
    }
}
