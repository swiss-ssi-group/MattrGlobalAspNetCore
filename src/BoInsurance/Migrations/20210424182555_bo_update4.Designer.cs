// <auto-generated />
using BoInsurance.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace BoInsurance.Migrations
{
    [DbContext(typeof(BoInsuranceVerifyMattrContext))]
    [Migration("20210424182555_bo_update4")]
    partial class bo_update4
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("Relational:MaxIdentifierLength", 128)
                .HasAnnotation("ProductVersion", "5.0.4")
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("BoInsurance.Data.DrivingLicensePresentationTemplate", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("DidId")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("MattrPresentationTemplateReponse")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("TemplateId")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.ToTable("DrivingLicensePresentationTemplates");
                });

            modelBuilder.Entity("BoInsurance.Data.DrivingLicensePresentationVerify", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("CallbackUrl")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Challenge")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Did")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("DidId")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("InvokePresentationResponse")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("SignAndEncodePresentationRequestBody")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("TemplateId")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.ToTable("DrivingLicensePresentationVerifications");
                });

            modelBuilder.Entity("BoInsurance.Data.VerifiedDriverLicense", b =>
                {
                    b.Property<string>("ChallengeId")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("ClaimsId")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("Holder")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("PresentationType")
                        .HasColumnType("nvarchar(max)");

                    b.Property<bool>("Verified")
                        .HasColumnType("bit");

                    b.HasKey("ChallengeId");

                    b.HasIndex("ClaimsId");

                    b.ToTable("VerifiedDriverLicenses");
                });

            modelBuilder.Entity("BoInsurance.Data.VerifiedDriverLicenseClaims", b =>
                {
                    b.Property<string>("Id")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("DateOfBirth")
                        .HasColumnType("nvarchar(max)");

                    b.Property<bool>("FirstName")
                        .HasColumnType("bit");

                    b.Property<string>("LicenseIssuedAt")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("LicenseType")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Name")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.ToTable("VerifiedDriverLicenseClaims");
                });

            modelBuilder.Entity("BoInsurance.Data.VerifiedDriverLicense", b =>
                {
                    b.HasOne("BoInsurance.Data.VerifiedDriverLicenseClaims", "Claims")
                        .WithMany()
                        .HasForeignKey("ClaimsId");

                    b.Navigation("Claims");
                });
#pragma warning restore 612, 618
        }
    }
}
