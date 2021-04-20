using Microsoft.EntityFrameworkCore;

namespace NationalDrivingLicense.Data
{
    public class NationalDrivingLicenseMattrContext : DbContext
    {
        public NationalDrivingLicenseMattrContext(DbContextOptions<NationalDrivingLicenseMattrContext> options) : base(options)
        { }

        public DbSet<DriverLicenseCredentials> DriverLicenseCredentials { get; set; }
    }
}
