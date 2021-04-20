using Microsoft.EntityFrameworkCore.Migrations;

namespace NationalDrivingLicense.Migrations
{
    public partial class init_ndl : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "DriverLicenseCredentials",
                columns: table => new
                {
                    UserName = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    OfferUrl = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Did = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DriverLicenseCredentials", x => x.UserName);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "DriverLicenseCredentials");
        }
    }
}
