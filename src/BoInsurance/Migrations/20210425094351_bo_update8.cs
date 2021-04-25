using Microsoft.EntityFrameworkCore.Migrations;

namespace BoInsurance.Migrations
{
    public partial class bo_update8 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_VerifiedDriverLicenses_VerifiedDriverLicenseClaims_ClaimsId",
                table: "VerifiedDriverLicenses");

            migrationBuilder.DropTable(
                name: "VerifiedDriverLicenseClaims");

            migrationBuilder.DropIndex(
                name: "IX_VerifiedDriverLicenses_ClaimsId",
                table: "VerifiedDriverLicenses");

            migrationBuilder.AlterColumn<string>(
                name: "ClaimsId",
                table: "VerifiedDriverLicenses",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)",
                oldNullable: true);

            migrationBuilder.AddColumn<string>(
                name: "DateOfBirth",
                table: "VerifiedDriverLicenses",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "FirstName",
                table: "VerifiedDriverLicenses",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "LicenseIssuedAt",
                table: "VerifiedDriverLicenses",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "LicenseType",
                table: "VerifiedDriverLicenses",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Name",
                table: "VerifiedDriverLicenses",
                type: "nvarchar(max)",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "DateOfBirth",
                table: "VerifiedDriverLicenses");

            migrationBuilder.DropColumn(
                name: "FirstName",
                table: "VerifiedDriverLicenses");

            migrationBuilder.DropColumn(
                name: "LicenseIssuedAt",
                table: "VerifiedDriverLicenses");

            migrationBuilder.DropColumn(
                name: "LicenseType",
                table: "VerifiedDriverLicenses");

            migrationBuilder.DropColumn(
                name: "Name",
                table: "VerifiedDriverLicenses");

            migrationBuilder.AlterColumn<string>(
                name: "ClaimsId",
                table: "VerifiedDriverLicenses",
                type: "nvarchar(450)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.CreateTable(
                name: "VerifiedDriverLicenseClaims",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    DateOfBirth = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    FirstName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    LicenseIssuedAt = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    LicenseType = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_VerifiedDriverLicenseClaims", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_VerifiedDriverLicenses_ClaimsId",
                table: "VerifiedDriverLicenses",
                column: "ClaimsId");

            migrationBuilder.AddForeignKey(
                name: "FK_VerifiedDriverLicenses_VerifiedDriverLicenseClaims_ClaimsId",
                table: "VerifiedDriverLicenses",
                column: "ClaimsId",
                principalTable: "VerifiedDriverLicenseClaims",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
