using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace NYC360.Infrastructure.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class AddListingAuth : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
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

            migrationBuilder.AlterColumn<TimeOnly>(
                name: "TimeWindow",
                table: "HousingRequests",
                type: "time",
                nullable: true,
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "FloorLevel",
                table: "HouseInfos",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.CreateTable(
                name: "HouseListingAuthorizations",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    HouseInfoId = table.Column<int>(type: "int", nullable: false),
                    FullName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    OrganizationName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Email = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    PhoneNumber = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PreferredContactDate = table.Column<DateOnly>(type: "date", nullable: false),
                    PreferredContactTime = table.Column<TimeOnly>(type: "time", nullable: false),
                    PreferredVirtualTourDate = table.Column<DateOnly>(type: "date", nullable: false),
                    PreferredVirtualTourTime = table.Column<TimeOnly>(type: "time", nullable: false),
                    PreferredInPersonTourDate = table.Column<DateOnly>(type: "date", nullable: false),
                    PreferredInPersonTourTime = table.Column<TimeOnly>(type: "time", nullable: false),
                    AuthorizationType = table.Column<int>(type: "int", nullable: false),
                    ListingAuthorizationDocument = table.Column<int>(type: "int", nullable: false),
                    AuthorizationValidationDate = table.Column<DateOnly>(type: "date", nullable: false),
                    SaveThisAuthorizationForFutureListings = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_HouseListingAuthorizations", x => x.Id);
                    table.ForeignKey(
                        name: "FK_HouseListingAuthorizations_HouseInfos_HouseInfoId",
                        column: x => x.HouseInfoId,
                        principalTable: "HouseInfos",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "HouseListingAuthorizationAttachments",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Url = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    HouseListingAuthorizationId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_HouseListingAuthorizationAttachments", x => x.Id);
                    table.ForeignKey(
                        name: "FK_HouseListingAuthorizationAttachments_HouseListingAuthorizations_HouseListingAuthorizationId",
                        column: x => x.HouseListingAuthorizationId,
                        principalTable: "HouseListingAuthorizations",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_HouseListingAuthorizationAttachments_HouseListingAuthorizationId",
                table: "HouseListingAuthorizationAttachments",
                column: "HouseListingAuthorizationId");

            migrationBuilder.CreateIndex(
                name: "IX_HouseListingAuthorizations_HouseInfoId",
                table: "HouseListingAuthorizations",
                column: "HouseInfoId",
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_HousingAttachments_HouseInfos_HousingId",
                table: "HousingAttachments",
                column: "HousingId",
                principalTable: "HouseInfos",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_HousingAttachments_HouseInfos_HousingId",
                table: "HousingAttachments");

            migrationBuilder.DropTable(
                name: "HouseListingAuthorizationAttachments");

            migrationBuilder.DropTable(
                name: "HouseListingAuthorizations");

            migrationBuilder.AlterColumn<DateTime>(
                name: "TimeWindow",
                table: "HousingRequests",
                type: "datetime2",
                nullable: true,
                oldClrType: typeof(TimeOnly),
                oldType: "time",
                oldNullable: true);

            migrationBuilder.AddColumn<int>(
                name: "HousingId1",
                table: "HousingAttachments",
                type: "int",
                nullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "FloorLevel",
                table: "HouseInfos",
                type: "int",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

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
    }
}
