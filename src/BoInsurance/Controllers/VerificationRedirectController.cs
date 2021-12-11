using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;

namespace BoInsurance.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class VerificationRedirectController : Controller
    {
        public static Dictionary<string, string> WalletUrls { get; set; } = new Dictionary<string, string>();

        [HttpGet]
        [Route("{challengeId}")]
        public IActionResult WalletUrl(string challengeId)
        {
            var walletUrl = WalletUrls.GetValueOrDefault(challengeId);
            // var didcommUrl = `didcomm://${ngrokUrl}/qr`;
            return Redirect(walletUrl);

        }
    }
}