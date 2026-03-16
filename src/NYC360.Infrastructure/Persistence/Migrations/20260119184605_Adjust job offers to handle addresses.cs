using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace NYC360.Infrastructure.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class Adjustjobofferstohandleaddresses : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_JobOffers_Locations_LocationId",
                table: "JobOffers");

            migrationBuilder.DropIndex(
                name: "IX_JobOffers_LocationId",
                table: "JobOffers");

            migrationBuilder.DropColumn(
                name: "LocationId",
                table: "JobOffers");

            migrationBuilder.AddColumn<int>(
                name: "AddressId",
                table: "JobOffers",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ZipCode",
                table: "Addresses",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_JobOffers_AddressId",
                table: "JobOffers",
                column: "AddressId");

            migrationBuilder.AddForeignKey(
                name: "FK_JobOffers_Addresses_AddressId",
                table: "JobOffers",
                column: "AddressId",
                principalTable: "Addresses",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_JobOffers_Addresses_AddressId",
                table: "JobOffers");

            migrationBuilder.DropIndex(
                name: "IX_JobOffers_AddressId",
                table: "JobOffers");

            migrationBuilder.DropColumn(
                name: "AddressId",
                table: "JobOffers");

            migrationBuilder.DropColumn(
                name: "ZipCode",
                table: "Addresses");

            migrationBuilder.AddColumn<int>(
                name: "LocationId",
                table: "JobOffers",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_JobOffers_LocationId",
                table: "JobOffers",
                column: "LocationId");

            migrationBuilder.AddForeignKey(
                name: "FK_JobOffers_Locations_LocationId",
                table: "JobOffers",
                column: "LocationId",
                principalTable: "Locations",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
