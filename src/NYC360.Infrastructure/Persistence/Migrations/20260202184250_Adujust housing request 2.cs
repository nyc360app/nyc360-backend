using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace NYC360.Infrastructure.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class Adujusthousingrequest2 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "InPersonTour",
                table: "HousingRequests");

            migrationBuilder.DropColumn(
                name: "ScheduleVirtual",
                table: "HousingRequests");

            migrationBuilder.RenameColumn(
                name: "TimeWindow",
                table: "HousingRequests",
                newName: "ScheduleVirtualTimeWindow");

            migrationBuilder.AlterColumn<DateOnly>(
                name: "PreferredContactDate",
                table: "HousingRequests",
                type: "date",
                nullable: false,
                oldClrType: typeof(DateTime),
                oldType: "datetime2");

            migrationBuilder.AlterColumn<DateOnly>(
                name: "MoveOutDate",
                table: "HousingRequests",
                type: "date",
                nullable: true,
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldNullable: true);

            migrationBuilder.AlterColumn<DateOnly>(
                name: "MoveInDate",
                table: "HousingRequests",
                type: "date",
                nullable: false,
                oldClrType: typeof(DateTime),
                oldType: "datetime2");

            migrationBuilder.AddColumn<DateOnly>(
                name: "InPersonTourDate",
                table: "HousingRequests",
                type: "date",
                nullable: true);

            migrationBuilder.AddColumn<TimeOnly>(
                name: "InPersonTourTimeWindow",
                table: "HousingRequests",
                type: "time",
                nullable: true);

            migrationBuilder.AddColumn<TimeOnly>(
                name: "PreferredContactTime",
                table: "HousingRequests",
                type: "time",
                nullable: false,
                defaultValue: new TimeOnly(0, 0, 0));

            migrationBuilder.AddColumn<DateOnly>(
                name: "ScheduleVirtualDate",
                table: "HousingRequests",
                type: "date",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "InPersonTourDate",
                table: "HousingRequests");

            migrationBuilder.DropColumn(
                name: "InPersonTourTimeWindow",
                table: "HousingRequests");

            migrationBuilder.DropColumn(
                name: "PreferredContactTime",
                table: "HousingRequests");

            migrationBuilder.DropColumn(
                name: "ScheduleVirtualDate",
                table: "HousingRequests");

            migrationBuilder.RenameColumn(
                name: "ScheduleVirtualTimeWindow",
                table: "HousingRequests",
                newName: "TimeWindow");

            migrationBuilder.AlterColumn<DateTime>(
                name: "PreferredContactDate",
                table: "HousingRequests",
                type: "datetime2",
                nullable: false,
                oldClrType: typeof(DateOnly),
                oldType: "date");

            migrationBuilder.AlterColumn<DateTime>(
                name: "MoveOutDate",
                table: "HousingRequests",
                type: "datetime2",
                nullable: true,
                oldClrType: typeof(DateOnly),
                oldType: "date",
                oldNullable: true);

            migrationBuilder.AlterColumn<DateTime>(
                name: "MoveInDate",
                table: "HousingRequests",
                type: "datetime2",
                nullable: false,
                oldClrType: typeof(DateOnly),
                oldType: "date");

            migrationBuilder.AddColumn<DateTime>(
                name: "InPersonTour",
                table: "HousingRequests",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "ScheduleVirtual",
                table: "HousingRequests",
                type: "datetime2",
                nullable: true);
        }
    }
}
