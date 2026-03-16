using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace NYC360.Infrastructure.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class Adjusthouseauthoringavailability : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "PreferredContactDate",
                table: "HouseListingAuthorizations");

            migrationBuilder.DropColumn(
                name: "PreferredContactTime",
                table: "HouseListingAuthorizations");

            migrationBuilder.DropColumn(
                name: "PreferredInPersonTourDate",
                table: "HouseListingAuthorizations");

            migrationBuilder.DropColumn(
                name: "PreferredInPersonTourTime",
                table: "HouseListingAuthorizations");

            migrationBuilder.DropColumn(
                name: "PreferredVirtualTourDate",
                table: "HouseListingAuthorizations");

            migrationBuilder.DropColumn(
                name: "PreferredVirtualTourTime",
                table: "HouseListingAuthorizations");

            migrationBuilder.AlterColumn<DateOnly>(
                name: "AuthorizationValidationDate",
                table: "HouseListingAuthorizations",
                type: "date",
                nullable: true,
                oldClrType: typeof(DateOnly),
                oldType: "date");

            migrationBuilder.CreateTable(
                name: "HouseListingAuthorizationAvailabilities",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    HouseListingAuthorizationId = table.Column<int>(type: "int", nullable: false),
                    AvailabilityType = table.Column<int>(type: "int", nullable: false),
                    Dates = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    TimeFrom = table.Column<TimeOnly>(type: "time", nullable: false),
                    TimeTo = table.Column<TimeOnly>(type: "time", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_HouseListingAuthorizationAvailabilities", x => x.Id);
                    table.ForeignKey(
                        name: "FK_HouseListingAuthorizationAvailabilities_HouseListingAuthorizations_HouseListingAuthorizationId",
                        column: x => x.HouseListingAuthorizationId,
                        principalTable: "HouseListingAuthorizations",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_HouseListingAuthorizationAvailabilities_HouseListingAuthorizationId",
                table: "HouseListingAuthorizationAvailabilities",
                column: "HouseListingAuthorizationId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "HouseListingAuthorizationAvailabilities");

            migrationBuilder.AlterColumn<DateOnly>(
                name: "AuthorizationValidationDate",
                table: "HouseListingAuthorizations",
                type: "date",
                nullable: false,
                defaultValue: new DateOnly(1, 1, 1),
                oldClrType: typeof(DateOnly),
                oldType: "date",
                oldNullable: true);

            migrationBuilder.AddColumn<DateOnly>(
                name: "PreferredContactDate",
                table: "HouseListingAuthorizations",
                type: "date",
                nullable: false,
                defaultValue: new DateOnly(1, 1, 1));

            migrationBuilder.AddColumn<TimeOnly>(
                name: "PreferredContactTime",
                table: "HouseListingAuthorizations",
                type: "time",
                nullable: false,
                defaultValue: new TimeOnly(0, 0, 0));

            migrationBuilder.AddColumn<DateOnly>(
                name: "PreferredInPersonTourDate",
                table: "HouseListingAuthorizations",
                type: "date",
                nullable: false,
                defaultValue: new DateOnly(1, 1, 1));

            migrationBuilder.AddColumn<TimeOnly>(
                name: "PreferredInPersonTourTime",
                table: "HouseListingAuthorizations",
                type: "time",
                nullable: false,
                defaultValue: new TimeOnly(0, 0, 0));

            migrationBuilder.AddColumn<DateOnly>(
                name: "PreferredVirtualTourDate",
                table: "HouseListingAuthorizations",
                type: "date",
                nullable: false,
                defaultValue: new DateOnly(1, 1, 1));

            migrationBuilder.AddColumn<TimeOnly>(
                name: "PreferredVirtualTourTime",
                table: "HouseListingAuthorizations",
                type: "time",
                nullable: false,
                defaultValue: new TimeOnly(0, 0, 0));
        }
    }
}
