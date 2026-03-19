using System;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using NYC360.Infrastructure.Persistence.DbContexts;

#nullable disable

namespace NYC360.Infrastructure.Persistence.Migrations
{
    /// <inheritdoc />
    [DbContext(typeof(ApplicationDbContext))]
    [Migration("20260319120000_AddCommunityLeaderApplications")]
    public partial class AddCommunityLeaderApplications : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "CommunityLeaderApplications",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserId = table.Column<int>(type: "int", nullable: false),
                    FullName = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    Email = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    PhoneNumber = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    CommunityName = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    Location = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    ProfileLink = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    Motivation = table.Column<string>(type: "nvarchar(2000)", maxLength: 2000, nullable: false),
                    Experience = table.Column<string>(type: "nvarchar(2000)", maxLength: 2000, nullable: false),
                    LedBefore = table.Column<bool>(type: "bit", nullable: false),
                    WeeklyAvailability = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: false),
                    AgreedToGuidelines = table.Column<bool>(type: "bit", nullable: false),
                    VerificationFileUrl = table.Column<string>(type: "nvarchar(260)", maxLength: 260, nullable: false),
                    Status = table.Column<byte>(type: "tinyint", nullable: false, defaultValue: (byte)1),
                    AdminNotes = table.Column<string>(type: "nvarchar(2000)", maxLength: 2000, nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ReviewedAt = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CommunityLeaderApplications", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CommunityLeaderApplications_UserProfiles_UserId",
                        column: x => x.UserId,
                        principalTable: "UserProfiles",
                        principalColumn: "UserId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_CommunityLeaderApplications_CreatedAt",
                table: "CommunityLeaderApplications",
                column: "CreatedAt");

            migrationBuilder.CreateIndex(
                name: "IX_CommunityLeaderApplications_Status",
                table: "CommunityLeaderApplications",
                column: "Status");

            migrationBuilder.CreateIndex(
                name: "IX_CommunityLeaderApplications_UserId_Pending",
                table: "CommunityLeaderApplications",
                column: "UserId",
                unique: true,
                filter: "[Status] = 1");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "CommunityLeaderApplications");
        }
    }
}
