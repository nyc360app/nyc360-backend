using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using NYC360.Infrastructure.Persistence.DbContexts;

#nullable disable

namespace NYC360.Infrastructure.Persistence.Migrations
{
    /// <inheritdoc />
    [DbContext(typeof(ApplicationDbContext))]
    [Migration("20260328140000_AddRssFeedItemsIngestionPipeline")]
    public partial class AddRssFeedItemsIngestionPipeline : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "LastCheckedAt",
                table: "RssFeedSources",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "LastError",
                table: "RssFeedSources",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "LastSuccessAt",
                table: "RssFeedSources",
                type: "datetime2",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "RssFeedItems",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    SourceId = table.Column<int>(type: "int", nullable: false),
                    Category = table.Column<byte>(type: "tinyint", nullable: false),
                    Guid = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    Link = table.Column<string>(type: "nvarchar(2000)", maxLength: 2000, nullable: false),
                    LinkHash = table.Column<string>(type: "nvarchar(128)", maxLength: 128, nullable: false),
                    Title = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: false),
                    Summary = table.Column<string>(type: "nvarchar(4000)", maxLength: 4000, nullable: true),
                    ImageUrl = table.Column<string>(type: "nvarchar(2000)", maxLength: 2000, nullable: true),
                    PublishedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    FetchedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    RawMetadataJson = table.Column<string>(type: "nvarchar(4000)", maxLength: 4000, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RssFeedItems", x => x.Id);
                    table.ForeignKey(
                        name: "FK_RssFeedItems_RssFeedSources_SourceId",
                        column: x => x.SourceId,
                        principalTable: "RssFeedSources",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_RssFeedItems_Category_PublishedAt",
                table: "RssFeedItems",
                columns: new[] { "Category", "PublishedAt" },
                descending: new[] { false, true });

            migrationBuilder.CreateIndex(
                name: "IX_RssFeedItems_SourceId_Guid",
                table: "RssFeedItems",
                columns: new[] { "SourceId", "Guid" },
                unique: true,
                filter: "[Guid] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_RssFeedItems_SourceId_LinkHash",
                table: "RssFeedItems",
                columns: new[] { "SourceId", "LinkHash" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_RssFeedItems_SourceId_PublishedAt",
                table: "RssFeedItems",
                columns: new[] { "SourceId", "PublishedAt" },
                descending: new[] { false, true });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "RssFeedItems");

            migrationBuilder.DropColumn(
                name: "LastCheckedAt",
                table: "RssFeedSources");

            migrationBuilder.DropColumn(
                name: "LastError",
                table: "RssFeedSources");

            migrationBuilder.DropColumn(
                name: "LastSuccessAt",
                table: "RssFeedSources");
        }
    }
}
