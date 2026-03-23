using System;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using NYC360.Infrastructure.Persistence.DbContexts;

#nullable disable

namespace NYC360.Infrastructure.Persistence.Migrations
{
    /// <inheritdoc />
    [DbContext(typeof(ApplicationDbContext))]
    [Migration("20260323120000_AddSpaceListings")]
    public partial class AddSpaceListings : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "SpaceListings",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    SubmitterUserId = table.Column<int>(type: "int", nullable: false),
                    Department = table.Column<byte>(type: "tinyint", nullable: false),
                    EntityType = table.Column<byte>(type: "tinyint", nullable: false),
                    Status = table.Column<byte>(type: "tinyint", nullable: false),
                    OwnershipStatus = table.Column<byte>(type: "tinyint", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: false),
                    NameNormalized = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(2000)", maxLength: 2000, nullable: false),
                    LocationId = table.Column<int>(type: "int", nullable: true),
                    LocationName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Borough = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Neighborhood = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Street = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    BuildingNumber = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ZipCode = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: true),
                    Website = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    WebsiteNormalized = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PhoneNumber = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PhoneNumberNormalized = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PublicEmail = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PublicEmailNormalized = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ContactName = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    SubmitterNote = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: true),
                    IsClaimingOwnership = table.Column<bool>(type: "bit", nullable: false),
                    ClaimedByUserId = table.Column<int>(type: "int", nullable: true),
                    Categories = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Tags = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    BusinessIndustry = table.Column<int>(type: "int", nullable: true),
                    BusinessSize = table.Column<int>(type: "int", nullable: true),
                    BusinessServiceArea = table.Column<int>(type: "int", nullable: true),
                    BusinessServices = table.Column<int>(type: "int", nullable: true),
                    BusinessOwnershipType = table.Column<int>(type: "int", nullable: true),
                    BusinessIsLicensedInNyc = table.Column<bool>(type: "bit", nullable: true),
                    BusinessIsInsured = table.Column<bool>(type: "bit", nullable: true),
                    OrganizationType = table.Column<int>(type: "int", nullable: true),
                    OrganizationFundType = table.Column<int>(type: "int", nullable: true),
                    OrganizationServices = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    OrganizationIsTaxExempt = table.Column<bool>(type: "bit", nullable: true),
                    OrganizationIsNysRegistered = table.Column<bool>(type: "bit", nullable: true),
                    SpaceItemId = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    SpaceEntityType = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    SpaceSlug = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    SpacePublishedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    LastPublishAttemptAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    LastPublishError = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    AssignedReviewerUserId = table.Column<int>(type: "int", nullable: true),
                    ModerationNote = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ReviewedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    ReviewedByUserId = table.Column<int>(type: "int", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SpaceListings", x => x.Id);
                    table.ForeignKey(
                        name: "FK_SpaceListings_UserProfiles_AssignedReviewerUserId",
                        column: x => x.AssignedReviewerUserId,
                        principalTable: "UserProfiles",
                        principalColumn: "UserId");
                    table.ForeignKey(
                        name: "FK_SpaceListings_UserProfiles_ClaimedByUserId",
                        column: x => x.ClaimedByUserId,
                        principalTable: "UserProfiles",
                        principalColumn: "UserId");
                    table.ForeignKey(
                        name: "FK_SpaceListings_UserProfiles_ReviewedByUserId",
                        column: x => x.ReviewedByUserId,
                        principalTable: "UserProfiles",
                        principalColumn: "UserId");
                    table.ForeignKey(
                        name: "FK_SpaceListings_UserProfiles_SubmitterUserId",
                        column: x => x.SubmitterUserId,
                        principalTable: "UserProfiles",
                        principalColumn: "UserId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "SpaceListingAttachments",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    SpaceListingId = table.Column<int>(type: "int", nullable: false),
                    Url = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Type = table.Column<byte>(type: "tinyint", nullable: false),
                    FileName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ContentType = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    SizeBytes = table.Column<long>(type: "bigint", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SpaceListingAttachments", x => x.Id);
                    table.ForeignKey(
                        name: "FK_SpaceListingAttachments_SpaceListings_SpaceListingId",
                        column: x => x.SpaceListingId,
                        principalTable: "SpaceListings",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "SpaceListingHours",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    SpaceListingId = table.Column<int>(type: "int", nullable: false),
                    DayOfWeek = table.Column<int>(type: "int", nullable: false),
                    OpenTime = table.Column<TimeOnly>(type: "time", nullable: true),
                    CloseTime = table.Column<TimeOnly>(type: "time", nullable: true),
                    IsClosed = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SpaceListingHours", x => x.Id);
                    table.ForeignKey(
                        name: "FK_SpaceListingHours_SpaceListings_SpaceListingId",
                        column: x => x.SpaceListingId,
                        principalTable: "SpaceListings",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "SpaceListingReviewEntries",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    SpaceListingId = table.Column<int>(type: "int", nullable: false),
                    ReviewerUserId = table.Column<int>(type: "int", nullable: false),
                    FromStatus = table.Column<byte>(type: "tinyint", nullable: false),
                    ToStatus = table.Column<byte>(type: "tinyint", nullable: false),
                    Note = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SpaceListingReviewEntries", x => x.Id);
                    table.ForeignKey(
                        name: "FK_SpaceListingReviewEntries_SpaceListings_SpaceListingId",
                        column: x => x.SpaceListingId,
                        principalTable: "SpaceListings",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_SpaceListingReviewEntries_UserProfiles_ReviewerUserId",
                        column: x => x.ReviewerUserId,
                        principalTable: "UserProfiles",
                        principalColumn: "UserId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "SpaceListingSocialLinks",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    SpaceListingId = table.Column<int>(type: "int", nullable: false),
                    Platform = table.Column<byte>(type: "tinyint", nullable: false),
                    Url = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SpaceListingSocialLinks", x => x.Id);
                    table.ForeignKey(
                        name: "FK_SpaceListingSocialLinks_SpaceListings_SpaceListingId",
                        column: x => x.SpaceListingId,
                        principalTable: "SpaceListings",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_SpaceListingAttachments_SpaceListingId",
                table: "SpaceListingAttachments",
                column: "SpaceListingId");

            migrationBuilder.CreateIndex(
                name: "IX_SpaceListingHours_SpaceListingId",
                table: "SpaceListingHours",
                column: "SpaceListingId");

            migrationBuilder.CreateIndex(
                name: "IX_SpaceListingReviewEntries_ReviewerUserId",
                table: "SpaceListingReviewEntries",
                column: "ReviewerUserId");

            migrationBuilder.CreateIndex(
                name: "IX_SpaceListingReviewEntries_SpaceListingId",
                table: "SpaceListingReviewEntries",
                column: "SpaceListingId");

            migrationBuilder.CreateIndex(
                name: "IX_SpaceListings_AssignedReviewerUserId",
                table: "SpaceListings",
                column: "AssignedReviewerUserId");

            migrationBuilder.CreateIndex(
                name: "IX_SpaceListings_ClaimedByUserId",
                table: "SpaceListings",
                column: "ClaimedByUserId");

            migrationBuilder.CreateIndex(
                name: "IX_SpaceListings_ReviewedByUserId",
                table: "SpaceListings",
                column: "ReviewedByUserId");

            migrationBuilder.CreateIndex(
                name: "IX_SpaceListings_SubmitterUserId",
                table: "SpaceListings",
                column: "SubmitterUserId");

            migrationBuilder.CreateIndex(
                name: "IX_SpaceListingSocialLinks_SpaceListingId",
                table: "SpaceListingSocialLinks",
                column: "SpaceListingId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "SpaceListingAttachments");

            migrationBuilder.DropTable(
                name: "SpaceListingHours");

            migrationBuilder.DropTable(
                name: "SpaceListingReviewEntries");

            migrationBuilder.DropTable(
                name: "SpaceListingSocialLinks");

            migrationBuilder.DropTable(
                name: "SpaceListings");
        }
    }
}
