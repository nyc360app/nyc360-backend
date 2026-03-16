using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace NYC360.Infrastructure.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class Addsaledatatohousing : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Title",
                table: "HouseInfos");

            migrationBuilder.RenameColumn(
                name: "AcceptedBuyerPrograms",
                table: "HouseInfos",
                newName: "BuyerHousingProgram");

            migrationBuilder.AddColumn<int>(
                name: "LegalUnitCount",
                table: "HouseInfos",
                type: "int",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "LegalUnitCount",
                table: "HouseInfos");

            migrationBuilder.RenameColumn(
                name: "BuyerHousingProgram",
                table: "HouseInfos",
                newName: "AcceptedBuyerPrograms");

            migrationBuilder.AddColumn<string>(
                name: "Title",
                table: "HouseInfos",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }
    }
}
