using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace NYC360.Infrastructure.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class Adduserid : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_HouseListingAuthorizations_UserProfiles_UserId",
                table: "HouseListingAuthorizations");

            migrationBuilder.AddForeignKey(
                name: "FK_HouseListingAuthorizations_UserProfiles_UserId",
                table: "HouseListingAuthorizations",
                column: "UserId",
                principalTable: "UserProfiles",
                principalColumn: "UserId",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_HouseListingAuthorizations_UserProfiles_UserId",
                table: "HouseListingAuthorizations");

            migrationBuilder.AddForeignKey(
                name: "FK_HouseListingAuthorizations_UserProfiles_UserId",
                table: "HouseListingAuthorizations",
                column: "UserId",
                principalTable: "UserProfiles",
                principalColumn: "UserId",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
