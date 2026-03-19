using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace NYC360.Infrastructure.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class ConfigureHousingRequestCascadeDelete : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_HousingRequests_UserProfiles_UserId",
                table: "HousingRequests");

            migrationBuilder.AddForeignKey(
                name: "FK_HousingRequests_UserProfiles_UserId",
                table: "HousingRequests",
                column: "UserId",
                principalTable: "UserProfiles",
                principalColumn: "UserId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_HousingRequests_UserProfiles_UserId",
                table: "HousingRequests");

            migrationBuilder.AddForeignKey(
                name: "FK_HousingRequests_UserProfiles_UserId",
                table: "HousingRequests",
                column: "UserId",
                principalTable: "UserProfiles",
                principalColumn: "UserId");
        }
    }
}
