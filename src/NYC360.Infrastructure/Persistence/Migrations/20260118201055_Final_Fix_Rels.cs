using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace NYC360.Infrastructure.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class Final_Fix_Rels : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Communities_AspNetUsers_OwnerId",
                table: "Communities");

            migrationBuilder.DropForeignKey(
                name: "FK_CommunityJoinRequests_Communities_CommunityId",
                table: "CommunityJoinRequests");

            migrationBuilder.DropForeignKey(
                name: "FK_CommunityMembers_Communities_CommunityId",
                table: "CommunityMembers");

            migrationBuilder.DropForeignKey(
                name: "FK_CommunityMembers_UserProfiles_UserId",
                table: "CommunityMembers");

            migrationBuilder.DropForeignKey(
                name: "FK_CommunityMembers_UserProfiles_UserProfileUserId",
                table: "CommunityMembers");

            migrationBuilder.DropForeignKey(
                name: "FK_Posts_Communities_CommunityId",
                table: "Posts");

            // migrationBuilder.DropForeignKey(
            //     name: "FK_UserPositions_UserProfiles_UserProfileId",
            //     table: "UserPositions");

            migrationBuilder.DropIndex(
                name: "IX_CommunityMembers_UserProfileUserId",
                table: "CommunityMembers");

            migrationBuilder.DropIndex(
                name: "IX_Communities_OwnerId",
                table: "Communities");

            migrationBuilder.DropColumn(
                name: "UserProfileUserId",
                table: "CommunityMembers");

            migrationBuilder.DropColumn(
                name: "OwnerId",
                table: "Communities");

            migrationBuilder.CreateTable(
                name: "CommunityDisbandRequests",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CommunityId = table.Column<int>(type: "int", nullable: false),
                    RequestedByUserId = table.Column<int>(type: "int", nullable: false),
                    Reason = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: false),
                    Status = table.Column<byte>(type: "tinyint", nullable: false),
                    RequestedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ProcessedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    ProcessedByUserId = table.Column<int>(type: "int", nullable: true),
                    AdminNotes = table.Column<string>(type: "nvarchar(2000)", maxLength: 2000, nullable: true),
                    CommunityId1 = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CommunityDisbandRequests", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CommunityDisbandRequests_Communities_CommunityId",
                        column: x => x.CommunityId,
                        principalTable: "Communities",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_CommunityDisbandRequests_Communities_CommunityId1",
                        column: x => x.CommunityId1,
                        principalTable: "Communities",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_CommunityDisbandRequests_UserProfiles_ProcessedByUserId",
                        column: x => x.ProcessedByUserId,
                        principalTable: "UserProfiles",
                        principalColumn: "UserId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_CommunityDisbandRequests_UserProfiles_RequestedByUserId",
                        column: x => x.RequestedByUserId,
                        principalTable: "UserProfiles",
                        principalColumn: "UserId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_CommunityDisbandRequests_CommunityId",
                table: "CommunityDisbandRequests",
                column: "CommunityId");

            migrationBuilder.CreateIndex(
                name: "IX_CommunityDisbandRequests_CommunityId1",
                table: "CommunityDisbandRequests",
                column: "CommunityId1");

            migrationBuilder.CreateIndex(
                name: "IX_CommunityDisbandRequests_ProcessedByUserId",
                table: "CommunityDisbandRequests",
                column: "ProcessedByUserId");

            migrationBuilder.CreateIndex(
                name: "IX_CommunityDisbandRequests_RequestedAt",
                table: "CommunityDisbandRequests",
                column: "RequestedAt");

            migrationBuilder.CreateIndex(
                name: "IX_CommunityDisbandRequests_RequestedByUserId",
                table: "CommunityDisbandRequests",
                column: "RequestedByUserId");

            migrationBuilder.CreateIndex(
                name: "IX_CommunityDisbandRequests_Status",
                table: "CommunityDisbandRequests",
                column: "Status");

            migrationBuilder.AddForeignKey(
                name: "FK_CommunityJoinRequests_Communities_CommunityId",
                table: "CommunityJoinRequests",
                column: "CommunityId",
                principalTable: "Communities",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_CommunityMembers_Communities_CommunityId",
                table: "CommunityMembers",
                column: "CommunityId",
                principalTable: "Communities",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_CommunityMembers_UserProfiles_UserId",
                table: "CommunityMembers",
                column: "UserId",
                principalTable: "UserProfiles",
                principalColumn: "UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_Posts_Communities_CommunityId",
                table: "Posts",
                column: "CommunityId",
                principalTable: "Communities",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_UserPositions_UserProfiles_UserProfileId",
                table: "UserPositions",
                column: "UserProfileId",
                principalTable: "UserProfiles",
                principalColumn: "UserId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CommunityJoinRequests_Communities_CommunityId",
                table: "CommunityJoinRequests");

            migrationBuilder.DropForeignKey(
                name: "FK_CommunityMembers_Communities_CommunityId",
                table: "CommunityMembers");

            migrationBuilder.DropForeignKey(
                name: "FK_CommunityMembers_UserProfiles_UserId",
                table: "CommunityMembers");

            migrationBuilder.DropForeignKey(
                name: "FK_Posts_Communities_CommunityId",
                table: "Posts");

            migrationBuilder.DropForeignKey(
                name: "FK_UserPositions_UserProfiles_UserProfileId",
                table: "UserPositions");

            migrationBuilder.DropTable(
                name: "CommunityDisbandRequests");

            migrationBuilder.AddColumn<int>(
                name: "UserProfileUserId",
                table: "CommunityMembers",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "OwnerId",
                table: "Communities",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_CommunityMembers_UserProfileUserId",
                table: "CommunityMembers",
                column: "UserProfileUserId");

            migrationBuilder.CreateIndex(
                name: "IX_Communities_OwnerId",
                table: "Communities",
                column: "OwnerId");

            migrationBuilder.AddForeignKey(
                name: "FK_Communities_AspNetUsers_OwnerId",
                table: "Communities",
                column: "OwnerId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_CommunityJoinRequests_Communities_CommunityId",
                table: "CommunityJoinRequests",
                column: "CommunityId",
                principalTable: "Communities",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_CommunityMembers_Communities_CommunityId",
                table: "CommunityMembers",
                column: "CommunityId",
                principalTable: "Communities",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_CommunityMembers_UserProfiles_UserId",
                table: "CommunityMembers",
                column: "UserId",
                principalTable: "UserProfiles",
                principalColumn: "UserId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_CommunityMembers_UserProfiles_UserProfileUserId",
                table: "CommunityMembers",
                column: "UserProfileUserId",
                principalTable: "UserProfiles",
                principalColumn: "UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_Posts_Communities_CommunityId",
                table: "Posts",
                column: "CommunityId",
                principalTable: "Communities",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_UserPositions_UserProfiles_UserProfileId",
                table: "UserPositions",
                column: "UserProfileId",
                principalTable: "UserProfiles",
                principalColumn: "UserId",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
