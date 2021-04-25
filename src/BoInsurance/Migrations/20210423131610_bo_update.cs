using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace BoInsurance.Migrations
{
    public partial class bo_update : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "DriverLicenseCredentials");

            migrationBuilder.CreateTable(
                name: "DrivingLicensePresentationTemplates",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    DidId = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    TemplateId = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    MattrPresentationTemplateReponse = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DrivingLicensePresentationTemplates", x => x.Id);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "DrivingLicensePresentationTemplates");

            migrationBuilder.CreateTable(
                name: "DriverLicenseCredentials",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    DidId = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    MattrPresentationTemplateReponse = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    TemplateId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DriverLicenseCredentials", x => x.Id);
                });
        }
    }
}
