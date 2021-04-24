using Microsoft.EntityFrameworkCore;

namespace BoInsurance.Data
{
    public class BoInsuranceVerifyMattrContext : DbContext
    {
        public BoInsuranceVerifyMattrContext(DbContextOptions<BoInsuranceVerifyMattrContext> options) : base(options)
        { }

        public DbSet<DrivingLicensePresentationTemplate> DrivingLicensePresentationTemplates { get; set; }

        public DbSet<DrivingLicensePresentationVerify> DrivingLicensePresentationVerifications { get; set; }

        public DbSet<VerifiedDriverLicense> VerifiedDriverLicenses { get; set; }

    }
}
