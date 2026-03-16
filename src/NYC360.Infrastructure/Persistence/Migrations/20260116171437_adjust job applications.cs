using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace NYC360.Infrastructure.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class adjustjobapplications : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_JobApplications_UserProfiles_ApplicantId",
                table: "JobApplications");

            migrationBuilder.AddColumn<int>(
                name: "UserProfileUserId",
                table: "Posts",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Posts_UserProfileUserId",
                table: "Posts",
                column: "UserProfileUserId");

            migrationBuilder.AddForeignKey(
                name: "FK_JobApplications_UserProfiles_ApplicantId",
                table: "JobApplications",
                column: "ApplicantId",
                principalTable: "UserProfiles",
                principalColumn: "UserId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Posts_UserProfiles_UserProfileUserId",
                table: "Posts",
                column: "UserProfileUserId",
                principalTable: "UserProfiles",
                principalColumn: "UserId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_JobApplications_UserProfiles_ApplicantId",
                table: "JobApplications");

            migrationBuilder.DropForeignKey(
                name: "FK_Posts_UserProfiles_UserProfileUserId",
                table: "Posts");

            migrationBuilder.DropIndex(
                name: "IX_Posts_UserProfileUserId",
                table: "Posts");

            migrationBuilder.DropColumn(
                name: "UserProfileUserId",
                table: "Posts");

            migrationBuilder.AddForeignKey(
                name: "FK_JobApplications_UserProfiles_ApplicantId",
                table: "JobApplications",
                column: "ApplicantId",
                principalTable: "UserProfiles",
                principalColumn: "UserId",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
