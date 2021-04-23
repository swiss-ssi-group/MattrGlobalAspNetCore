using System;
using System.ComponentModel.DataAnnotations;

namespace BoInsurance.Data
{
    public class DrvingLicensePresentationTemplate
    {
        [Key]
        public int Id { get; set; }
        public string DidId { get; set; }
        public Guid TemplateId { get; set; }
        public string MattrPresentationTemplateReponse { get; set; }
    }
}
