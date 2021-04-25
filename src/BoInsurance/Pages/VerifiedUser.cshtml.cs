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

        public void OnGet(string challengeId)
        {
            // user query param to get challenge id and display data
        }
    }
}
