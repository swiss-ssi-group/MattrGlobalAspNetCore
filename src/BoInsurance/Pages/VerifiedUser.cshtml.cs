using Microsoft.AspNetCore.Mvc.RazorPages;
using System;
using System.Text;
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

        public DriverLicenseClaimsDto VerifiedDriverLicenseClaims { get; private set; }

        public string Base64ChallengeId { get; set; }

        public async Task OnGetAsync(string base64ChallengeId)
        {
            // user query param to get challenge id and display data
            if (base64ChallengeId != null)
            {
                var valueBytes = Convert.FromBase64String(base64ChallengeId);
                var challengeId = Encoding.UTF8.GetString(valueBytes);
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
