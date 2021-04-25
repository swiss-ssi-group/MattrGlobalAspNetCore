using BoInsurance.Data;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace BoInsurance.Pages
{
    public class VerifiedUserModel : PageModel
    {
        private readonly BoInsuranceDbService _boInsuranceDbService;

        public VerifiedUserModel(BoInsuranceDbService boInsuranceDbService)
        {
            _boInsuranceDbService = boInsuranceDbService;
        }

        public string ChallengeId { get; set; }
        public VerifiedDriverLicenseClaims VerifiedDriverLicenseClaims { get; private set; }

        public async System.Threading.Tasks.Task OnGetAsync(string challengeId)
        {
            // user query param to get challenge id and display data
            if (challengeId != null)
            {
                var verifiedDriverLicenseUser = await _boInsuranceDbService.GetVerifiedUser(challengeId);
                VerifiedDriverLicenseClaims = verifiedDriverLicenseUser.Claims;
            }
        }
    }
}
