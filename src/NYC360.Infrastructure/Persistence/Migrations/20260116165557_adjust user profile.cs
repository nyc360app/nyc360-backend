using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace NYC360.Infrastructure.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class adjustuserprofile : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Addresses_AspNetUsers_ManagedByUserId",
                table: "Addresses");

            migrationBuilder.DropForeignKey(
                name: "FK_CommunityJoinRequests_AspNetUsers_UserId",
                table: "CommunityJoinRequests");

            migrationBuilder.DropForeignKey(
                name: "FK_CommunityMembers_AspNetUsers_UserId",
                table: "CommunityMembers");

            migrationBuilder.DropForeignKey(
                name: "FK_JobApplications_AspNetUsers_ApplicantId",
                table: "JobApplications");

            migrationBuilder.DropForeignKey(
                name: "FK_JobOffers_AspNetUsers_AuthorId",
                table: "JobOffers");

            migrationBuilder.DropForeignKey(
                name: "FK_PostComments_AspNetUsers_UserId",
                table: "PostComments");

            migrationBuilder.DropForeignKey(
                name: "FK_Posts_AspNetUsers_AuthorId",
                table: "Posts");

            migrationBuilder.DropForeignKey(
                name: "FK_PostUserInteractions_AspNetUsers_UserId",
                table: "PostUserInteractions");

            migrationBuilder.DropForeignKey(
                name: "FK_UserInterests_AspNetUsers_UserId",
                table: "UserInterests");

            migrationBuilder.DropForeignKey(
                name: "FK_UserSavedPosts_AspNetUsers_UserId",
                table: "UserSavedPosts");

            migrationBuilder.DropForeignKey(
                name: "FK_UserSocialLinks_OrganizationProfiles_OrganizationProfileUserId",
                table: "UserSocialLinks");

            migrationBuilder.DropForeignKey(
                name: "FK_UserSocialLinks_UserProfiles_UserProfileUserId",
                table: "UserSocialLinks");

            migrationBuilder.DropForeignKey(
                name: "FK_UserStats_UserProfiles_ProfileId",
                table: "UserStats");

            migrationBuilder.DropForeignKey(
                name: "FK_UserTags_AspNetUsers_UserId",
                table: "UserTags");

            migrationBuilder.DropTable(
                name: "AdminProfiles");

            migrationBuilder.DropTable(
                name: "OrganizationProfiles");

            migrationBuilder.DropIndex(
                name: "IX_UserSocialLinks_OrganizationProfileUserId",
                table: "UserSocialLinks");

            migrationBuilder.DropIndex(
                name: "IX_UserSocialLinks_UserProfileUserId",
                table: "UserSocialLinks");

            migrationBuilder.DropColumn(
                name: "OrganizationProfileUserId",
                table: "UserSocialLinks");

            migrationBuilder.DropColumn(
                name: "UserProfileUserId",
                table: "UserSocialLinks");

            migrationBuilder.DropColumn(
                name: "CoverImageUrl",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "ImageUrl",
                table: "AspNetUsers");

            migrationBuilder.RenameColumn(
                name: "ProfileId",
                table: "UserStats",
                newName: "UserId");

            migrationBuilder.AlterColumn<string>(
                name: "LastName",
                table: "UserProfiles",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(100)",
                oldMaxLength: 100);

            migrationBuilder.AlterColumn<string>(
                name: "Headline",
                table: "UserProfiles",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AlterColumn<string>(
                name: "Bio",
                table: "UserProfiles",
                type: "nvarchar(500)",
                maxLength: 500,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(500)",
                oldMaxLength: 500);

            migrationBuilder.AddColumn<string>(
                name: "AvatarUrl",
                table: "UserProfiles",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "CoverImageUrl",
                table: "UserProfiles",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "TagId",
                table: "UserProfiles",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "MemberCount",
                table: "Communities",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_UserSocialLinks_UserId",
                table: "UserSocialLinks",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_UserProfiles_TagId",
                table: "UserProfiles",
                column: "TagId");

            migrationBuilder.AddForeignKey(
                name: "FK_Addresses_UserProfiles_ManagedByUserId",
                table: "Addresses",
                column: "ManagedByUserId",
                principalTable: "UserProfiles",
                principalColumn: "UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_CommunityJoinRequests_UserProfiles_UserId",
                table: "CommunityJoinRequests",
                column: "UserId",
                principalTable: "UserProfiles",
                principalColumn: "UserId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_CommunityMembers_UserProfiles_UserId",
                table: "CommunityMembers",
                column: "UserId",
                principalTable: "UserProfiles",
                principalColumn: "UserId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_JobApplications_UserProfiles_ApplicantId",
                table: "JobApplications",
                column: "ApplicantId",
                principalTable: "UserProfiles",
                principalColumn: "UserId",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_JobOffers_UserProfiles_AuthorId",
                table: "JobOffers",
                column: "AuthorId",
                principalTable: "UserProfiles",
                principalColumn: "UserId",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_PostComments_UserProfiles_UserId",
                table: "PostComments",
                column: "UserId",
                principalTable: "UserProfiles",
                principalColumn: "UserId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Posts_UserProfiles_AuthorId",
                table: "Posts",
                column: "AuthorId",
                principalTable: "UserProfiles",
                principalColumn: "UserId",
                onDelete: ReferentialAction.SetNull);

            migrationBuilder.AddForeignKey(
                name: "FK_PostUserInteractions_UserProfiles_UserId",
                table: "PostUserInteractions",
                column: "UserId",
                principalTable: "UserProfiles",
                principalColumn: "UserId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_UserInterests_UserProfiles_UserId",
                table: "UserInterests",
                column: "UserId",
                principalTable: "UserProfiles",
                principalColumn: "UserId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_UserProfiles_Tags_TagId",
                table: "UserProfiles",
                column: "TagId",
                principalTable: "Tags",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_UserSavedPosts_UserProfiles_UserId",
                table: "UserSavedPosts",
                column: "UserId",
                principalTable: "UserProfiles",
                principalColumn: "UserId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_UserSocialLinks_UserProfiles_UserId",
                table: "UserSocialLinks",
                column: "UserId",
                principalTable: "UserProfiles",
                principalColumn: "UserId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_UserStats_UserProfiles_UserId",
                table: "UserStats",
                column: "UserId",
                principalTable: "UserProfiles",
                principalColumn: "UserId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_UserTags_UserProfiles_UserId",
                table: "UserTags",
                column: "UserId",
                principalTable: "UserProfiles",
                principalColumn: "UserId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Addresses_UserProfiles_ManagedByUserId",
                table: "Addresses");

            migrationBuilder.DropForeignKey(
                name: "FK_CommunityJoinRequests_UserProfiles_UserId",
                table: "CommunityJoinRequests");

            migrationBuilder.DropForeignKey(
                name: "FK_CommunityMembers_UserProfiles_UserId",
                table: "CommunityMembers");

            migrationBuilder.DropForeignKey(
                name: "FK_JobApplications_UserProfiles_ApplicantId",
                table: "JobApplications");

            migrationBuilder.DropForeignKey(
                name: "FK_JobOffers_UserProfiles_AuthorId",
                table: "JobOffers");

            migrationBuilder.DropForeignKey(
                name: "FK_PostComments_UserProfiles_UserId",
                table: "PostComments");

            migrationBuilder.DropForeignKey(
                name: "FK_Posts_UserProfiles_AuthorId",
                table: "Posts");

            migrationBuilder.DropForeignKey(
                name: "FK_PostUserInteractions_UserProfiles_UserId",
                table: "PostUserInteractions");

            migrationBuilder.DropForeignKey(
                name: "FK_UserInterests_UserProfiles_UserId",
                table: "UserInterests");

            migrationBuilder.DropForeignKey(
                name: "FK_UserProfiles_Tags_TagId",
                table: "UserProfiles");

            migrationBuilder.DropForeignKey(
                name: "FK_UserSavedPosts_UserProfiles_UserId",
                table: "UserSavedPosts");

            migrationBuilder.DropForeignKey(
                name: "FK_UserSocialLinks_UserProfiles_UserId",
                table: "UserSocialLinks");

            migrationBuilder.DropForeignKey(
                name: "FK_UserStats_UserProfiles_UserId",
                table: "UserStats");

            migrationBuilder.DropForeignKey(
                name: "FK_UserTags_UserProfiles_UserId",
                table: "UserTags");

            migrationBuilder.DropIndex(
                name: "IX_UserSocialLinks_UserId",
                table: "UserSocialLinks");

            migrationBuilder.DropIndex(
                name: "IX_UserProfiles_TagId",
                table: "UserProfiles");

            migrationBuilder.DropColumn(
                name: "AvatarUrl",
                table: "UserProfiles");

            migrationBuilder.DropColumn(
                name: "CoverImageUrl",
                table: "UserProfiles");

            migrationBuilder.DropColumn(
                name: "TagId",
                table: "UserProfiles");

            migrationBuilder.DropColumn(
                name: "MemberCount",
                table: "Communities");

            migrationBuilder.RenameColumn(
                name: "UserId",
                table: "UserStats",
                newName: "ProfileId");

            migrationBuilder.AddColumn<int>(
                name: "OrganizationProfileUserId",
                table: "UserSocialLinks",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "UserProfileUserId",
                table: "UserSocialLinks",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AlterColumn<string>(
                name: "LastName",
                table: "UserProfiles",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(100)",
                oldMaxLength: 100,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Headline",
                table: "UserProfiles",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Bio",
                table: "UserProfiles",
                type: "nvarchar(500)",
                maxLength: 500,
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(500)",
                oldMaxLength: 500,
                oldNullable: true);

            migrationBuilder.AddColumn<string>(
                name: "CoverImageUrl",
                table: "AspNetUsers",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ImageUrl",
                table: "AspNetUsers",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "AdminProfiles",
                columns: table => new
                {
                    UserId = table.Column<int>(type: "int", nullable: false),
                    AvatarUrl = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    FirstName = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    LastName = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AdminProfiles", x => x.UserId);
                    table.ForeignKey(
                        name: "FK_AdminProfiles_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "OrganizationProfiles",
                columns: table => new
                {
                    UserId = table.Column<int>(type: "int", nullable: false),
                    StatsProfileId = table.Column<int>(type: "int", nullable: true),
                    Bio = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: false),
                    Email = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    LicenseNumber = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    Name = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    PhoneNumber = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_OrganizationProfiles", x => x.UserId);
                    table.ForeignKey(
                        name: "FK_OrganizationProfiles_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_OrganizationProfiles_UserStats_StatsProfileId",
                        column: x => x.StatsProfileId,
                        principalTable: "UserStats",
                        principalColumn: "ProfileId");
                });

            migrationBuilder.CreateIndex(
                name: "IX_UserSocialLinks_OrganizationProfileUserId",
                table: "UserSocialLinks",
                column: "OrganizationProfileUserId");

            migrationBuilder.CreateIndex(
                name: "IX_UserSocialLinks_UserProfileUserId",
                table: "UserSocialLinks",
                column: "UserProfileUserId");

            migrationBuilder.CreateIndex(
                name: "IX_OrganizationProfiles_StatsProfileId",
                table: "OrganizationProfiles",
                column: "StatsProfileId");

            migrationBuilder.AddForeignKey(
                name: "FK_Addresses_AspNetUsers_ManagedByUserId",
                table: "Addresses",
                column: "ManagedByUserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_CommunityJoinRequests_AspNetUsers_UserId",
                table: "CommunityJoinRequests",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_CommunityMembers_AspNetUsers_UserId",
                table: "CommunityMembers",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_JobApplications_AspNetUsers_ApplicantId",
                table: "JobApplications",
                column: "ApplicantId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_JobOffers_AspNetUsers_AuthorId",
                table: "JobOffers",
                column: "AuthorId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_PostComments_AspNetUsers_UserId",
                table: "PostComments",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Posts_AspNetUsers_AuthorId",
                table: "Posts",
                column: "AuthorId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.SetNull);

            migrationBuilder.AddForeignKey(
                name: "FK_PostUserInteractions_AspNetUsers_UserId",
                table: "PostUserInteractions",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_UserInterests_AspNetUsers_UserId",
                table: "UserInterests",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_UserSavedPosts_AspNetUsers_UserId",
                table: "UserSavedPosts",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_UserSocialLinks_OrganizationProfiles_OrganizationProfileUserId",
                table: "UserSocialLinks",
                column: "OrganizationProfileUserId",
                principalTable: "OrganizationProfiles",
                principalColumn: "UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_UserSocialLinks_UserProfiles_UserProfileUserId",
                table: "UserSocialLinks",
                column: "UserProfileUserId",
                principalTable: "UserProfiles",
                principalColumn: "UserId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_UserStats_UserProfiles_ProfileId",
                table: "UserStats",
                column: "ProfileId",
                principalTable: "UserProfiles",
                principalColumn: "UserId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_UserTags_AspNetUsers_UserId",
                table: "UserTags",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
