using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace NYC360.Infrastructure.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class AddOrganizationinfo : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "OrganizationInfos",
                columns: table => new
                {
                    UserId = table.Column<int>(type: "int", nullable: false),
                    OrganizationType = table.Column<int>(type: "int", nullable: false),
                    ServiceArea = table.Column<int>(type: "int", nullable: false),
                    FundType = table.Column<int>(type: "int", nullable: false),
                    IsTaxExempt = table.Column<bool>(type: "bit", nullable: false),
                    IsNysRegistered = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_OrganizationInfos", x => x.UserId);
                    table.ForeignKey(
                        name: "FK_OrganizationInfos_UserProfiles_UserId",
                        column: x => x.UserId,
                        principalTable: "UserProfiles",
                        principalColumn: "UserId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "OrganizationServices",
                columns: table => new
                {
                    OrganizationId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    OrganizationUserId = table.Column<int>(type: "int", nullable: false),
                    Service = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_OrganizationServices", x => x.OrganizationId);
                    table.ForeignKey(
                        name: "FK_OrganizationServices_OrganizationInfos_OrganizationUserId",
                        column: x => x.OrganizationUserId,
                        principalTable: "OrganizationInfos",
                        principalColumn: "UserId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_OrganizationServices_OrganizationUserId",
                table: "OrganizationServices",
                column: "OrganizationUserId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "OrganizationServices");

            migrationBuilder.DropTable(
                name: "OrganizationInfos");
        }
    }
}
