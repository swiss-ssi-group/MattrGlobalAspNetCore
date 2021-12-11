using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace BoInsurance.Controllers
{
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
    public class VerifiedDriverLicense
    {
        [JsonPropertyName("presentationType")]
        public string PresentationType { get; set; }

        [Key]
        [JsonPropertyName("challengeId")]
        public string ChallengeId { get; set; }

        [JsonPropertyName("claims")]
        public VerifiedDriverLicenseClaims Claims { get; set; } = new VerifiedDriverLicenseClaims();

        [JsonPropertyName("verified")]
        public bool Verified { get; set; }

        [JsonPropertyName("holder")]
        public string Holder { get; set; }
    }
}
