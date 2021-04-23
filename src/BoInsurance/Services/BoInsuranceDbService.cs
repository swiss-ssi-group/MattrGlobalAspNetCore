using Microsoft.EntityFrameworkCore;
using BoInsurance.Data;
using System.Linq;
using System.Threading.Tasks;

namespace BoInsurance
{
    public class BoInsuranceDbService
    {
        private readonly BoInsuranceVerifyMattrContext _boInsuranceVerifyMattrContext;

        public BoInsuranceDbService(BoInsuranceVerifyMattrContext boInsuranceVerifyMattrContext)
        {
            _boInsuranceVerifyMattrContext = boInsuranceVerifyMattrContext;
        }

        public async Task<(string DidId, string TemplateId)> GetLastDriverLicensePrsentationTemplate()
        {
            var driverLicenseTemplate = await _boInsuranceVerifyMattrContext
                .DrivingLicensePresentationTemplates
                .OrderBy(u => u.Id)
                .LastOrDefaultAsync();

            if (driverLicenseTemplate != null)
            {
                var templateId = driverLicenseTemplate.TemplateId;
                return (driverLicenseTemplate.DidId, driverLicenseTemplate.TemplateId);
            }

            return (string.Empty, string.Empty);
        }

        public async Task CreateDriverLicensePresentationTemplate(DrivingLicensePresentationTemplate driverLicenseTemplate)
        {
            _boInsuranceVerifyMattrContext.DrivingLicensePresentationTemplates.Add(driverLicenseTemplate);
            await _boInsuranceVerifyMattrContext.SaveChangesAsync();
        }
    }
}
