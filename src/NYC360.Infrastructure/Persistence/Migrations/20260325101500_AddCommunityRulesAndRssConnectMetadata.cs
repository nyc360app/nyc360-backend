using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using NYC360.Infrastructure.Persistence.DbContexts;

#nullable disable

namespace NYC360.Infrastructure.Persistence.Migrations
{
    /// <inheritdoc />
    [DbContext(typeof(ApplicationDbContext))]
    [Migration("20260325101500_AddCommunityRulesAndRssConnectMetadata")]
    public partial class AddCommunityRulesAndRssConnectMetadata : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsFeatured",
                table: "Communities",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<string>(
                name: "Rules",
                table: "Communities",
                type: "nvarchar(4000)",
                maxLength: 4000,
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "AgreementAccepted",
                table: "RssFeedConnectionRequests",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<string>(
                name: "DivisionTag",
                table: "RssFeedConnectionRequests",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Language",
                table: "RssFeedConnectionRequests",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "LogoImageUrl",
                table: "RssFeedConnectionRequests",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "SourceCredibility",
                table: "RssFeedConnectionRequests",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "SourceWebsite",
                table: "RssFeedConnectionRequests",
                type: "nvarchar(max)",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsFeatured",
                table: "Communities");

            migrationBuilder.DropColumn(
                name: "Rules",
                table: "Communities");

            migrationBuilder.DropColumn(
                name: "AgreementAccepted",
                table: "RssFeedConnectionRequests");

            migrationBuilder.DropColumn(
                name: "DivisionTag",
                table: "RssFeedConnectionRequests");

            migrationBuilder.DropColumn(
                name: "Language",
                table: "RssFeedConnectionRequests");

            migrationBuilder.DropColumn(
                name: "LogoImageUrl",
                table: "RssFeedConnectionRequests");

            migrationBuilder.DropColumn(
                name: "SourceCredibility",
                table: "RssFeedConnectionRequests");

            migrationBuilder.DropColumn(
                name: "SourceWebsite",
                table: "RssFeedConnectionRequests");
        }
    }
}
