using System.ComponentModel.DataAnnotations;

namespace BoInsurance.Data
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
        public string presentationType { get; set; }
        [Key]
        public string challengeId { get; set; }
        public VerifiedDriverLicenseClaims claims { get; set; }
        public bool verified { get; set; }
        public string holder { get; set; }
    }
    public class VerifiedDriverLicenseClaims
    {
        public string id { get; set; }
        public string name { get; set; }
        public bool firstName { get; set; }
        public string licenseType { get; set; }
        public string dateOfBirth { get; set; }
        public string licenseIssuedAt { get; set; }
    }
}
