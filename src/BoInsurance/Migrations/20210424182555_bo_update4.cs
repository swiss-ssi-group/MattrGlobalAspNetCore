using Microsoft.EntityFrameworkCore.Migrations;

namespace BoInsurance.Migrations
{
    public partial class bo_update4 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "VerifiedDriverLicenseClaims",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    FirstName = table.Column<bool>(type: "bit", nullable: false),
                    LicenseType = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    DateOfBirth = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    LicenseIssuedAt = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_VerifiedDriverLicenseClaims", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "VerifiedDriverLicenses",
                columns: table => new
                {
                    ChallengeId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    PresentationType = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ClaimsId = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    Verified = table.Column<bool>(type: "bit", nullable: false),
                    Holder = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_VerifiedDriverLicenses", x => x.ChallengeId);
                    table.ForeignKey(
                        name: "FK_VerifiedDriverLicenses_VerifiedDriverLicenseClaims_ClaimsId",
                        column: x => x.ClaimsId,
                        principalTable: "VerifiedDriverLicenseClaims",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_VerifiedDriverLicenses_ClaimsId",
                table: "VerifiedDriverLicenses",
                column: "ClaimsId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "VerifiedDriverLicenses");

            migrationBuilder.DropTable(
                name: "VerifiedDriverLicenseClaims");
        }
    }
}
