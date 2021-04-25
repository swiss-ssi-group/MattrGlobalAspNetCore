using Microsoft.EntityFrameworkCore.Migrations;

namespace BoInsurance.Migrations
{
    public partial class bo_update2 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "DrivingLicensePresentationVerifications",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    DidId = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    TemplateId = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CallbackUrl = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    InvokePresentationResponse = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Did = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    SignAndEncodePresentationRequestBody = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DrivingLicensePresentationVerifications", x => x.Id);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "DrivingLicensePresentationVerifications");
        }
    }
}
