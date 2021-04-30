using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Threading.Tasks;

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
        public DriverLicenseClaimsDto VerifiedDriverLicenseClaims { get; private set; }

        public async Task OnGetAsync(string challengeId)
        {
            // user query param to get challenge id and display data
            if (challengeId != null)
            {
                var verifiedDriverLicenseUser = await _boInsuranceDbService.GetVerifiedUser(challengeId);
                VerifiedDriverLicenseClaims = new DriverLicenseClaimsDto
                {
                    DateOfBirth = verifiedDriverLicenseUser.DateOfBirth,
                    Name = verifiedDriverLicenseUser.Name,
                    LicenseType = verifiedDriverLicenseUser.LicenseType,
                    FirstName = verifiedDriverLicenseUser.FirstName,
                    LicenseIssuedAt = verifiedDriverLicenseUser.LicenseIssuedAt
                };
            }
        }
    }

    public class DriverLicenseClaimsDto
    {
        public string Name { get; set; }
        public string FirstName { get; set; }
        public string LicenseType { get; set; }
        public string DateOfBirth { get; set; }
        public string LicenseIssuedAt { get; set; }
    }
}
