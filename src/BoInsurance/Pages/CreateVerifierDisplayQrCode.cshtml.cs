using System;
using System.ComponentModel.DataAnnotations;
using System.Text;
using System.Threading.Tasks;
using BoInsurance.Controllers;
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
        public string ChallengeId { get; set; }

        [BindProperty]
        public string Base64ChallengeId { get; set; }

        [BindProperty]
        public CreateVerifierDisplayQrCodeCallbackUrl CallbackUrlDto { get; set; }
        public CreateVerifierDisplayQrCodeModel(MattrCredentialVerifyCallbackService mattrCredentialVerifyCallbackService)
        {
            _mattrCredentialVerifyCallbackService = mattrCredentialVerifyCallbackService;
        }
        public void OnGet()
        {
            CallbackUrlDto = new CreateVerifierDisplayQrCodeCallbackUrl();
            CallbackUrlDto.CallbackUrl = $"https://{HttpContext.Request.Host.Value}";
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            // Test the QR code
            //var jws = "eyJhbGciOiJFZERTQSIsImtpZCI6ImRpZDprZXk6ejZNa2ozdFVxUlNad01SOUJlWDZ2eHltYnk1VmVlbjdSekFvQlFqdEFER3ZZZjhUI3o2TWtqM3RVcVJTWndNUjlCZVg2dnh5bWJ5NVZlZW43UnpBb0JRanRBREd2WWY4VCJ9.eyJpZCI6ImYxYWE2MWJhLWEyNjAtNDg5NC05M2ZiLTVhYzRkY2M2NDU5NiIsInR5cGUiOiJodHRwczovL21hdHRyLmdsb2JhbC9zY2hlbWFzL3ZlcmlmaWFibGUtcHJlc2VudGF0aW9uL3JlcXVlc3QvUXVlcnlCeUZyYW1lIiwiZnJvbSI6ImRpZDprZXk6elVDN0xrcFd3ZDc2VUJUZlJKaVB3eHRHQzZ4UFh3M2txWjVkbkRra1pWcXVISzJNN1R0ZUN0VjROM0xvZGJGZmhTTkdlQVNGdEdEUmREb3N5MXVjZW00Q2RYM1VrVFVXV2JiV0p3enFXc2IydWZlU2dSeUE1RWtQZ1BiY1U1Tm5vdFlBa2VyIiwiY3JlYXRlZF90aW1lIjoxNjIxNjkyOTY3MDM0LCJleHBpcmVzX3RpbWUiOjE2Mzg4MzY0MDEwMDAsInJlcGx5X3VybCI6Imh0dHBzOi8vZGFtaWFuYm9kLXNhbmRib3gudmlpLm1hdHRyLmdsb2JhbC9jb3JlL3YxL3ByZXNlbnRhdGlvbnMvcmVzcG9uc2UiLCJyZXBseV90byI6WyJkaWQ6a2V5OnpVQzdMa3BXd2Q3NlVCVGZSSmlQd3h0R0M2eFBYdzNrcVo1ZG5Ea2taVnF1SEsyTTdUdGVDdFY0TjNMb2RiRmZoU05HZUFTRnRHRFJkRG9zeTF1Y2VtNENkWDNVa1RVV1diYldKd3pxV3NiMnVmZVNnUnlBNUVrUGdQYmNVNU5ub3RZQWtlciJdLCJib2R5Ijp7ImNoYWxsZW5nZSI6IlVlcEpNcVVMa0haZTVOaUlidWlhMU00czNMVVNQNDdIYUZJTmFvajMiLCJpZCI6ImZiODI1MjAyLWEwNmMtNGRmZi1iODliLWM5NjAzYzI2NDE3YiIsImRvbWFpbiI6ImRhbWlhbmJvZC1zYW5kYm94LnZpaS5tYXR0ci5nbG9iYWwiLCJuYW1lIjoiemtwLWNlcnRpZmljYXRlLXByZXNlbnRhdGlvbi0yIiwicXVlcnkiOlt7InR5cGUiOiJRdWVyeUJ5RnJhbWUiLCJjcmVkZW50aWFsUXVlcnkiOlt7ImZyYW1lIjp7InR5cGUiOiJWZXJpZmlhYmxlQ3JlZGVudGlhbCIsIkBjb250ZXh0IjpbImh0dHBzOi8vd3d3LnczLm9yZy8yMDE4L2NyZWRlbnRpYWxzL3YxIiwiaHR0cHM6Ly93M2lkLm9yZy92Yy1yZXZvY2F0aW9uLWxpc3QtMjAyMC92MSIsImh0dHBzOi8vdzNjLWNjZy5naXRodWIuaW8vbGRwLWJiczIwMjAvY29udGV4dC92MSIsImh0dHBzOi8vc2NoZW1hLm9yZyJdLCJjcmVkZW50aWFsU3ViamVjdCI6eyJAZXhwbGljaXQiOnRydWUsImdpdmVuX25hbWUiOnt9LCJmYW1pbHlfbmFtZSI6e30sImRhdGVfb2ZfYmlydGgiOnt9LCJudW1iZXJfb2ZfZG9zZXMiOnt9LCJ2YWNjaW5hdGlvbl9kYXRlIjp7fSwidG90YWxfbnVtYmVyX29mX2Rvc2VzIjp7fSwiY291bnRyeV9vZl92YWNjaW5hdGlvbiI6e30sIm1lZGljaW5hbF9wcm9kdWN0X2NvZGUiOnt9fX0sInJlYXNvbiI6IlBsZWFzZSBwcm92aWRlIHlvdXIgdmFjY2luYXRpb24gZGF0YSIsInJlcXVpcmVkIjp0cnVlLCJ0cnVzdGVkSXNzdWVyIjpbeyJpc3N1ZXIiOiJkaWQ6a2V5OnpVQzdMa3BXd2Q3NlVCVGZSSmlQd3h0R0M2eFBYdzNrcVo1ZG5Ea2taVnF1SEsyTTdUdGVDdFY0TjNMb2RiRmZoU05HZUFTRnRHRFJkRG9zeTF1Y2VtNENkWDNVa1RVV1diYldKd3pxV3NiMnVmZVNnUnlBNUVrUGdQYmNVNU5ub3RZQWtlciIsInJlcXVpcmVkIjp0cnVlfV19XX1dfX0.PHA0YGhAT6mfIwQYdM3wSlORcInAWdTy0vmM7FYLokaVRW3bxdchBgS - Ru_onglTT3O8FjfFYyL1Kg4JQFWQDw";
            //var challenge = "RhOtpTa8vNh1EId6sJ7AVD3prerMMDSkfWZrUPzt";
            //var qrCodeUrl = $"didcomm://https://damianbod-sandbox.vii.mattr.global/?request={jws}";
            //QrCodeUrl = qrCodeUrl.Trim();
            //ChallengeId = challenge;
            //return Page();

            var result = await _mattrCredentialVerifyCallbackService
                .CreateVerifyCallback(CallbackUrlDto.CallbackUrl);

            CreatingVerifier = false;

            var walletUrl = result.WalletUrl.Trim();
            ChallengeId = result.ChallengeId;
            var valueBytes = Encoding.UTF8.GetBytes(ChallengeId);
            Base64ChallengeId = Convert.ToBase64String(valueBytes);

            VerificationRedirectController.WalletUrls.Add(Base64ChallengeId, walletUrl);

            // https://learn.mattr.global/tutorials/verify/using-callback/callback-e-to-e#redirect-urls
            //var qrCodeUrl = $"didcomm://{walletUrl}";

            QrCodeUrl = $"didcomm://https://{HttpContext.Request.Host.Value}/VerificationRedirect/{Base64ChallengeId}";
            return Page();
        }
    }

    public class CreateVerifierDisplayQrCodeCallbackUrl
    {
        [Required]
        public string CallbackUrl { get; set; }
    }
}
