using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace NYC360.Infrastructure.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class adjustprofilerels : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_JobOffers_Locations_LocationId",
                table: "JobOffers");

            migrationBuilder.DropForeignKey(
                name: "FK_JobOffers_UserProfiles_AuthorId",
                table: "JobOffers");

            migrationBuilder.DropForeignKey(
                name: "FK_UserSocialLinks_UserProfiles_UserId1",
                table: "UserSocialLinks");

            migrationBuilder.DropIndex(
                name: "IX_UserSocialLinks_UserId1",
                table: "UserSocialLinks");

            migrationBuilder.DropColumn(
                name: "UserId1",
                table: "UserSocialLinks");

            migrationBuilder.AddColumn<int>(
                name: "UserProfileUserId",
                table: "JobOffers",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_JobOffers_UserProfileUserId",
                table: "JobOffers",
                column: "UserProfileUserId");

            migrationBuilder.AddForeignKey(
                name: "FK_JobOffers_Locations_LocationId",
                table: "JobOffers",
                column: "LocationId",
                principalTable: "Locations",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_JobOffers_UserProfiles_AuthorId",
                table: "JobOffers",
                column: "AuthorId",
                principalTable: "UserProfiles",
                principalColumn: "UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_JobOffers_UserProfiles_UserProfileUserId",
                table: "JobOffers",
                column: "UserProfileUserId",
                principalTable: "UserProfiles",
                principalColumn: "UserId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_JobOffers_Locations_LocationId",
                table: "JobOffers");

            migrationBuilder.DropForeignKey(
                name: "FK_JobOffers_UserProfiles_AuthorId",
                table: "JobOffers");

            migrationBuilder.DropForeignKey(
                name: "FK_JobOffers_UserProfiles_UserProfileUserId",
                table: "JobOffers");

            migrationBuilder.DropIndex(
                name: "IX_JobOffers_UserProfileUserId",
                table: "JobOffers");

            migrationBuilder.DropColumn(
                name: "UserProfileUserId",
                table: "JobOffers");

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
                name: "FK_JobOffers_Locations_LocationId",
                table: "JobOffers",
                column: "LocationId",
                principalTable: "Locations",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_JobOffers_UserProfiles_AuthorId",
                table: "JobOffers",
                column: "AuthorId",
                principalTable: "UserProfiles",
                principalColumn: "UserId",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_UserSocialLinks_UserProfiles_UserId1",
                table: "UserSocialLinks",
                column: "UserId1",
                principalTable: "UserProfiles",
                principalColumn: "UserId");
        }
    }
}
