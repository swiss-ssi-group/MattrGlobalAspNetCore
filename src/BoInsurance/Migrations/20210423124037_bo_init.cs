using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace BoInsurance.Migrations
{
    public partial class bo_init : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "DriverLicenseCredentials",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    DidId = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    TemplateId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    MattrPresentationTemplateReponse = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DriverLicenseCredentials", x => x.Id);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "DriverLicenseCredentials");
        }
    }
}
