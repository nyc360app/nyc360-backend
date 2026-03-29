using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using NYC360.Infrastructure.Persistence.DbContexts;

#nullable disable

namespace NYC360.Infrastructure.Persistence.Migrations
{
    /// <inheritdoc />
    [DbContext(typeof(ApplicationDbContext))]
    [Migration("20260328110000_AddRssRequestReviewAuditAndFinalCategory")]
    public partial class AddRssRequestReviewAuditAndFinalCategory : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<byte>(
                name: "FinalCategory",
                table: "RssFeedConnectionRequests",
                type: "tinyint",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "LogoFileName",
                table: "RssFeedConnectionRequests",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "ProcessedByUserId",
                table: "RssFeedConnectionRequests",
                type: "int",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "FinalCategory",
                table: "RssFeedConnectionRequests");

            migrationBuilder.DropColumn(
                name: "LogoFileName",
                table: "RssFeedConnectionRequests");

            migrationBuilder.DropColumn(
                name: "ProcessedByUserId",
                table: "RssFeedConnectionRequests");
        }
    }
}
