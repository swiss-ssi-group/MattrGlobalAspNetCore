using Microsoft.EntityFrameworkCore;
using BoInsurance.Data;
using System.Linq;
using System.Threading.Tasks;
using BoInsurance.Controllers;

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
            var data = new VerifiedDriverLicenseData
            {
                DateOfBirth = item.Claims.DateOfBirth,
                ChallengeId = item.ChallengeId,
                ClaimsId = item.Claims.Id,
                FirstName = item.Claims.FirstName,
                Holder = item.Holder,
                LicenseIssuedAt = item.Claims.LicenseIssuedAt,
                LicenseType = item.Claims.LicenseType,
                Name = item.Claims.Name,
                PresentationType = item.PresentationType,
                Verified = item.Verified
            };

            _boInsuranceVerifyMattrContext.VerifiedDriverLicenses.Add(data);
            await _boInsuranceVerifyMattrContext.SaveChangesAsync();
        }

        public async Task<VerifiedDriverLicenseData> GetVerifiedUser(string challengeId)
        {
            return await _boInsuranceVerifyMattrContext
                .VerifiedDriverLicenses
                .FirstOrDefaultAsync(v => v.ChallengeId == challengeId);
        }
    }
}
