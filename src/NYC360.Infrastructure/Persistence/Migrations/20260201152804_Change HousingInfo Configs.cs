using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace NYC360.Infrastructure.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class ChangeHousingInfoConfigs : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_HousingAttachments_HouseInfos_HousingId",
                table: "HousingAttachments");

            migrationBuilder.AddColumn<int>(
                name: "HousingId1",
                table: "HousingAttachments",
                type: "int",
                nullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "ZipCode",
                table: "HouseInfos",
                type: "nvarchar(10)",
                maxLength: 10,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AlterColumn<string>(
                name: "Borough",
                table: "HouseInfos",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.CreateIndex(
                name: "IX_HousingAttachments_HousingId1",
                table: "HousingAttachments",
                column: "HousingId1");

            migrationBuilder.AddForeignKey(
                name: "FK_HousingAttachments_HouseInfos_HousingId",
                table: "HousingAttachments",
                column: "HousingId",
                principalTable: "HouseInfos",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_HousingAttachments_HouseInfos_HousingId1",
                table: "HousingAttachments",
                column: "HousingId1",
                principalTable: "HouseInfos",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_HousingAttachments_HouseInfos_HousingId",
                table: "HousingAttachments");

            migrationBuilder.DropForeignKey(
                name: "FK_HousingAttachments_HouseInfos_HousingId1",
                table: "HousingAttachments");

            migrationBuilder.DropIndex(
                name: "IX_HousingAttachments_HousingId1",
                table: "HousingAttachments");

            migrationBuilder.DropColumn(
                name: "HousingId1",
                table: "HousingAttachments");

            migrationBuilder.AlterColumn<string>(
                name: "ZipCode",
                table: "HouseInfos",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(10)",
                oldMaxLength: 10);

            migrationBuilder.AlterColumn<string>(
                name: "Borough",
                table: "HouseInfos",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(50)",
                oldMaxLength: 50);

            migrationBuilder.AddForeignKey(
                name: "FK_HousingAttachments_HouseInfos_HousingId",
                table: "HousingAttachments",
                column: "HousingId",
                principalTable: "HouseInfos",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
