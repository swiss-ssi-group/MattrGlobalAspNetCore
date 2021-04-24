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
            // Test the QR code
            var jws = "didcomm://https://damianbod-sandbox.vii.mattr.global/?request=eyJhbGciOiJFZERTQSIsImtpZCI6ImRpZDprZXk6ejZNa3FvSDNFQlNDUHlIWHJ5Qmdnd0J1bWFrN1YxTDlCUUg3VDRpNXNwWERwbVM2I3o2TWtxb0gzRUJTQ1B5SFhyeUJnZ3dCdW1hazdWMUw5QlFIN1Q0aTVzcFhEcG1TNiJ9.eyJpZCI6ImNhMGJiMTNkLWY2OTMtNDdmYS1hZTMzLWY5MzVlYmEwOWE2OSIsInR5cGUiOiJodHRwczovL21hdHRyLmdsb2JhbC9zY2hlbWFzL3ZlcmlmaWFibGUtcHJlc2VudGF0aW9uL3JlcXVlc3QvUXVlcnlCeUV4YW1wbGUiLCJmcm9tIjoiZGlkOmtleTp6Nk1rcW9IM0VCU0NQeUhYcnlCZ2d3QnVtYWs3VjFMOUJRSDdUNGk1c3BYRHBtUzYiLCJjcmVhdGVkX3RpbWUiOjE2MTkyNzUxMTgxNTgsImV4cGlyZXNfdGltZSI6MTYzODgzNjQwMTAwMCwicmVwbHlfdXJsIjoiaHR0cHM6Ly9kYW1pYW5ib2Qtc2FuZGJveC52aWkubWF0dHIuZ2xvYmFsL2NvcmUvdjEvcHJlc2VudGF0aW9ucy9yZXNwb25zZSIsInJlcGx5X3RvIjpbImRpZDprZXk6ejZNa3FvSDNFQlNDUHlIWHJ5Qmdnd0J1bWFrN1YxTDlCUUg3VDRpNXNwWERwbVM2Il0sImJvZHkiOnsiY2hhbGxlbmdlIjoiOTljNTQ3NmItZTg0Yi00Njg4LWI1Y2ItMWMwMzc0Yjc1N2JiIiwiaWQiOiJkYTA1NTI3YS1mNDc1LTRhNjMtYTc3Yi1lNTNlZmEwMzM1MmMiLCJkb21haW4iOiJkYW1pYW5ib2Qtc2FuZGJveC52aWkubWF0dHIuZ2xvYmFsIiwibmFtZSI6ImNlcnRpZmljYXRlLXByZXNlbnRhdGlvbiIsInF1ZXJ5IjpbeyJ0eXBlIjoiUXVlcnlCeUV4YW1wbGUiLCJjcmVkZW50aWFsUXVlcnkiOlt7InJlYXNvbiI6IlBsZWFzZSBwcm92aWRlIHlvdXIgZHJpdmluZyBsaWNlbnNlIiwiZXhhbXBsZSI6eyJ0eXBlIjoiVmVyaWZpYWJsZUNyZWRlbnRpYWwiLCJAY29udGV4dCI6WyJodHRwczovL3NjaGVtYS5vcmciXSwidHJ1c3RlZElzc3VlciI6W3siaXNzdWVyIjoiZGlkOmtleTp6Nk1rcW9IM0VCU0NQeUhYcnlCZ2d3QnVtYWs3VjFMOUJRSDdUNGk1c3BYRHBtUzYiLCJyZXF1aXJlZCI6dHJ1ZX1dfSwicmVxdWlyZWQiOnRydWV9XX1dfX0.3pgkqBnK2NmNSDVd-bD1RP9P9AAzXzEn3AJ-ANkz3CtWhCHSjT8ddIspjO77U1JWOrp9Zu4hdJN9BW-o_KSYBA";

            var qrCodeUrl = $"didcomm://{MattrCredentialVerifyCallbackService.MATTR_DOMAIN}/?request={jws}";
            QrCodeUrl = qrCodeUrl;

            //QrCodeUrl = await _mattrCredentialVerifyCallbackService.CreateVerifyCallback(CallbackUrlDto.CallbackUrl);
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
