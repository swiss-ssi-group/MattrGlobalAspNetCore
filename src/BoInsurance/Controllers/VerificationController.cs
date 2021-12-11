using BoInsurance.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using System.Text.Json;
using System.Threading.Tasks;

namespace BoInsurance.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class VerificationController : Controller
    {
        private readonly BoInsuranceDbService _boInsuranceDbService;

        private readonly IHubContext<MattrVerifiedSuccessHub> _hubContext;

        public VerificationController(BoInsuranceDbService boInsuranceDbService,
            IHubContext<MattrVerifiedSuccessHub> hubContext)
        {
            _hubContext = hubContext;
            _boInsuranceDbService = boInsuranceDbService;
        }

        /// <summary>
        /// {
        ///  "presentationType": "QueryByExample",
        ///  "challengeId": "GW8FGpP6jhFrl37yQZIM6w",
        ///  "claims": {
        ///      "id": "did:key:z6MkfxQU7dy8eKxyHpG267FV23agZQu9zmokd8BprepfHALi",
        ///      "name": "Chris",
        ///      "firstName": "Shin",
        ///      "licenseType": "Certificate Name",
        ///      "dateOfBirth": "some data",
        ///      "licenseIssuedAt": "dda"
        ///  },
        ///  "verified": true,
        ///  "holder": "did:key:z6MkgmEkNM32vyFeMXcQA7AfQDznu47qHCZpy2AYH2Dtdu1d"
        /// }
        /// </summary>
        /// <param name="body"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("[action]")]
        public async Task<IActionResult> VerificationDataCallback()
        {
            string content = await new System.IO.StreamReader(Request.Body).ReadToEndAsync();
            var body = JsonSerializer.Deserialize<VerifiedDriverLicense>(content);

            string connectionId;
            var found = MattrVerifiedSuccessHub.Challenges
                .TryGetValue(body.ChallengeId, out connectionId);

            // test Signalr
            //await _hubContext.Clients.Client(connectionId).SendAsync("MattrCallbackSuccess", $"{body.ChallengeId}");
            //return Ok();

            var exists = await _boInsuranceDbService.ChallengeExists(body.ChallengeId);

            if (exists)
            {
                await _boInsuranceDbService.PersistVerification(body);

                if (found)
                {
                    //$"/VerifiedUser?challengeid={body.ChallengeId}"
                    await _hubContext.Clients
                        .Client(connectionId)
                        .SendAsync("MattrCallbackSuccess", $"{body.ChallengeId}");
                }

                return Ok();
            }

            return BadRequest("unknown verify request");
        }
    }
}