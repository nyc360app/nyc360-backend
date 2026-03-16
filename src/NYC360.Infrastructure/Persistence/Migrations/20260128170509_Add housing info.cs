using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace NYC360.Infrastructure.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class Addhousinginfo : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_HousingRequests_Posts_PostId",
                table: "HousingRequests");

            migrationBuilder.RenameColumn(
                name: "PostId",
                table: "HousingRequests",
                newName: "HouseInfoId");

            migrationBuilder.RenameIndex(
                name: "IX_HousingRequests_PostId",
                table: "HousingRequests",
                newName: "IX_HousingRequests_HouseInfoId");
            
            migrationBuilder.Sql("DELETE FROM [HousingRequests] WHERE [HouseInfoId] NOT IN (SELECT [Id] FROM [HouseInfos])");
            migrationBuilder.AddForeignKey(
                name: "FK_HousingRequests_HouseInfos_HouseInfoId",
                table: "HousingRequests",
                column: "HouseInfoId",
                principalTable: "HouseInfos",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_HousingRequests_HouseInfos_HouseInfoId",
                table: "HousingRequests");

            migrationBuilder.RenameColumn(
                name: "HouseInfoId",
                table: "HousingRequests",
                newName: "PostId");

            migrationBuilder.RenameIndex(
                name: "IX_HousingRequests_HouseInfoId",
                table: "HousingRequests",
                newName: "IX_HousingRequests_PostId");

            migrationBuilder.AddForeignKey(
                name: "FK_HousingRequests_Posts_PostId",
                table: "HousingRequests",
                column: "PostId",
                principalTable: "Posts",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
