using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace NYC360.Infrastructure.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class AdjustUserprofilerelations : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "UserId1",
                table: "UserSocialLinks",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_UserSocialLinks_UserId1",
                table: "UserSocialLinks",
                column: "UserId1");

            migrationBuilder.AddForeignKey(
                name: "FK_UserSocialLinks_UserProfiles_UserId1",
                table: "UserSocialLinks",
                column: "UserId1",
                principalTable: "UserProfiles",
                principalColumn: "UserId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_UserSocialLinks_UserProfiles_UserId1",
                table: "UserSocialLinks");

            migrationBuilder.DropIndex(
                name: "IX_UserSocialLinks_UserId1",
                table: "UserSocialLinks");

            migrationBuilder.DropColumn(
                name: "UserId1",
                table: "UserSocialLinks");
        }
    }
}
