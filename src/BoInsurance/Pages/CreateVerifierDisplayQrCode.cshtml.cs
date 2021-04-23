using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace BoInsurance.Pages
{
    public class CreateVerifierDisplayQrCodeModel : PageModel
    {
        private readonly MattrCredentialVerifyCallbackService _mattrCredentialVerifyCallbackService;
        public bool CreatingVerifier { get; set; } = true;
        public string QrCodeUrl { get; set; }

        [BindProperty]
        public CreateVerifierDisplayQrCodeCallbackUrl CallbackUrlDto { get; set; }
        public CreateVerifierDisplayQrCodeModel(MattrCredentialVerifyCallbackService mattrCredentialVerifyCallbackService)
        {
            _mattrCredentialVerifyCallbackService = mattrCredentialVerifyCallbackService;
        }
        public void OnGet()
        {
            CallbackUrlDto = new CreateVerifierDisplayQrCodeCallbackUrl();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            QrCodeUrl = await _mattrCredentialVerifyCallbackService.CreateVerifyCallback(CallbackUrlDto.CallbackUrl);
            CreatingVerifier = false;
            return Page();
        }
    }

    public class CreateVerifierDisplayQrCodeCallbackUrl
    {
        [Required]
        public string CallbackUrl { get; set; }
    }
}
