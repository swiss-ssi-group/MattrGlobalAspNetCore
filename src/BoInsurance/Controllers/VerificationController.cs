using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace BoInsurance.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class VerificationController : Controller
    {
        private readonly BoInsuranceDbService _boInsuranceDbService;

        public VerificationController(BoInsuranceDbService boInsuranceDbService)
        {
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
        public async Task<IActionResult> DrivingLicenseCallback([FromBody] VerifiedDriverLicense body)
        {
            var exists = await _boInsuranceDbService.ChallengeExists(body.ChallengeId);

            if(exists)
            {
                await _boInsuranceDbService.PersistVerification(body);
                // TODO send event to update UI with the data 
                // UI needs to switch to this page
                //$"/VerifiedUser?challengeid={body.ChallengeId}"

                return Ok();
            }

            return BadRequest("unknown verify request");
        }
    }
}