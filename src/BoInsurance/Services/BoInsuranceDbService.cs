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

        public async Task CreateDrivingLicensePresentationVerify(DrivingLicensePresentationVerify drivingLicensePresentationVerify)
        {
            _boInsuranceVerifyMattrContext.DrivingLicensePresentationVerifications.Add(drivingLicensePresentationVerify);
            await _boInsuranceVerifyMattrContext.SaveChangesAsync();
        }

        public async Task PersistVerification(VerifiedDriverLicense item)
        {
            _boInsuranceVerifyMattrContext.VerifiedDriverLicenses.Add(item);
            await _boInsuranceVerifyMattrContext.SaveChangesAsync();
        }

        public async Task<VerifiedDriverLicense> GetVerifiedUser(string challengeId)
        {
            return await _boInsuranceVerifyMattrContext
                .VerifiedDriverLicenses
                .FirstOrDefaultAsync(v => v.ChallengeId == challengeId);
        }
    }
}
