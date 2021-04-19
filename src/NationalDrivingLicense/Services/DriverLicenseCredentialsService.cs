using Microsoft.EntityFrameworkCore;
using NationalDrivingLicense.Data;
using System.Threading.Tasks;

namespace NationalDrivingLicense
{
    public class DriverLicenseCredentialsService
    {
        private readonly NationalDrivingLicenseMattrContext _nationalDrivingLicenseMattrContext;

        public DriverLicenseCredentialsService(NationalDrivingLicenseMattrContext nationalDrivingLicenseMattrContext)
        {
            _nationalDrivingLicenseMattrContext = nationalDrivingLicenseMattrContext;
        }

        public async Task<bool> HasIdentityDriverLicense(string username)
        {
            if (!string.IsNullOrEmpty(username))
            {
                var driverLicense = await _nationalDrivingLicenseMattrContext.DriverLicenseCredentials.FirstOrDefaultAsync(
                    dl => dl.UserName == username
                );

                if (driverLicense != null)
                {
                    return true;
                }
            }

            return false;
        }

        public async Task<DriverLicenseCredentials> GetDriverLicense(string username)
        {
            var driverLicense = await _nationalDrivingLicenseMattrContext.DriverLicenseCredentials.FirstOrDefaultAsync(
                    dl => dl.UserName == username
                );

            return driverLicense;
        }

        public async Task UpdateDriverLicense(DriverLicenseCredentials driverLicense)
        {
            _nationalDrivingLicenseMattrContext.DriverLicenseCredentials.Update(driverLicense);
            await _nationalDrivingLicenseMattrContext.SaveChangesAsync();
        }
    }
}
