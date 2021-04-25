using System.ComponentModel.DataAnnotations;

namespace BoInsurance.Data
{
    public class VerifiedDriverLicenseData
    {
        [Key]
        public string ChallengeId { get; set; }
        public string PresentationType { get; set; }
        public string ClaimsId { get; set; }
        public string Name { get; set; }
        public string FirstName { get; set; }
        public string LicenseType { get; set; }
        public string DateOfBirth { get; set; }
        public string LicenseIssuedAt { get; set; }
        public bool Verified { get; set; }
        public string Holder { get; set; }
    }
}
