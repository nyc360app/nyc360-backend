using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace NYC360.Infrastructure.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class Addnewyorkerinfo : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "NewYorkerInfo",
                columns: table => new
                {
                    UserId = table.Column<int>(type: "int", nullable: false),
                    IsInterestedInVolunteering = table.Column<bool>(type: "bit", nullable: false),
                    IsOpenToAttendingLocalEvents = table.Column<bool>(type: "bit", nullable: false),
                    FollowNeighborhoodUpdates = table.Column<bool>(type: "bit", nullable: false),
                    MakeProfilePublic = table.Column<bool>(type: "bit", nullable: false),
                    DisplayNeighborhood = table.Column<bool>(type: "bit", nullable: false),
                    AllowMessagesFromVerifiedOrganizations = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_NewYorkerInfo", x => x.UserId);
                    table.ForeignKey(
                        name: "FK_NewYorkerInfo_UserProfiles_UserId",
                        column: x => x.UserId,
                        principalTable: "UserProfiles",
                        principalColumn: "UserId",
                        onDelete: ReferentialAction.Cascade);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "NewYorkerInfo");
        }
    }
}
