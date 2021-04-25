using System.ComponentModel.DataAnnotations;

namespace BoInsurance.Data
{
    public class DrivingLicensePresentationTemplate
    {
        [Key]
        public int Id { get; set; }
        public string DidId { get; set; }
        public string TemplateId { get; set; }
        public string MattrPresentationTemplateReponse { get; set; }
    }
}
