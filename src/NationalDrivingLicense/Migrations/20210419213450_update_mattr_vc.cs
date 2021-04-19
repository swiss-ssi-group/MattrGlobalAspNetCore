using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace NationalDrivingLicense.Migrations
{
    public partial class update_mattr_vc : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_DriverLicenseCredentials",
                table: "DriverLicenseCredentials");

            migrationBuilder.DropColumn(
                name: "UserName",
                table: "DriverLicenseCredentials");

            migrationBuilder.RenameColumn(
                name: "OfferUrl",
                table: "DriverLicenseCredentials",
                newName: "OidcIssuer");

            migrationBuilder.AddColumn<int>(
                name: "Id",
                table: "DriverLicenseCredentials",
                type: "int",
                nullable: false,
                defaultValue: 0)
                .Annotation("SqlServer:Identity", "1, 1");

            migrationBuilder.AddColumn<string>(
                name: "Name",
                table: "DriverLicenseCredentials",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "OidcIssuerId",
                table: "DriverLicenseCredentials",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddPrimaryKey(
                name: "PK_DriverLicenseCredentials",
                table: "DriverLicenseCredentials",
                column: "Id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_DriverLicenseCredentials",
                table: "DriverLicenseCredentials");

            migrationBuilder.DropColumn(
                name: "Id",
                table: "DriverLicenseCredentials");

            migrationBuilder.DropColumn(
                name: "Name",
                table: "DriverLicenseCredentials");

            migrationBuilder.DropColumn(
                name: "OidcIssuerId",
                table: "DriverLicenseCredentials");

            migrationBuilder.RenameColumn(
                name: "OidcIssuer",
                table: "DriverLicenseCredentials",
                newName: "OfferUrl");

            migrationBuilder.AddColumn<string>(
                name: "UserName",
                table: "DriverLicenseCredentials",
                type: "nvarchar(450)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddPrimaryKey(
                name: "PK_DriverLicenseCredentials",
                table: "DriverLicenseCredentials",
                column: "UserName");
        }
    }
}
