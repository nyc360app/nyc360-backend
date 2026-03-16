using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace NYC360.Infrastructure.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class AdjustEventstaffpermissions : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "CanViewSales",
                table: "EventStaffs",
                newName: "IsPerformanceBasedOnly");

            migrationBuilder.RenameColumn(
                name: "CanManageTickets",
                table: "EventStaffs",
                newName: "HasFullAnalyticsAccess");

            migrationBuilder.RenameColumn(
                name: "CanEditEvent",
                table: "EventStaffs",
                newName: "HasFinancialAuthority");

            migrationBuilder.AddColumn<double>(
                name: "BarRevenuePercentage",
                table: "EventStaffs",
                type: "float",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "CanEnforceCompliance",
                table: "EventStaffs",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "CanManageCampaigns",
                table: "EventStaffs",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "CanManageTicketing",
                table: "EventStaffs",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "CanViewRevenueAttribution",
                table: "EventStaffs",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<decimal>(
                name: "FixedFee",
                table: "EventStaffs",
                type: "decimal(18,2)",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "HasApprovalAuthority",
                table: "EventStaffs",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<double>(
                name: "TicketPercentage",
                table: "EventStaffs",
                type: "float",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "BarRevenuePercentage",
                table: "EventStaffs");

            migrationBuilder.DropColumn(
                name: "CanEnforceCompliance",
                table: "EventStaffs");

            migrationBuilder.DropColumn(
                name: "CanManageCampaigns",
                table: "EventStaffs");

            migrationBuilder.DropColumn(
                name: "CanManageTicketing",
                table: "EventStaffs");

            migrationBuilder.DropColumn(
                name: "CanViewRevenueAttribution",
                table: "EventStaffs");

            migrationBuilder.DropColumn(
                name: "FixedFee",
                table: "EventStaffs");

            migrationBuilder.DropColumn(
                name: "HasApprovalAuthority",
                table: "EventStaffs");

            migrationBuilder.DropColumn(
                name: "TicketPercentage",
                table: "EventStaffs");

            migrationBuilder.RenameColumn(
                name: "IsPerformanceBasedOnly",
                table: "EventStaffs",
                newName: "CanViewSales");

            migrationBuilder.RenameColumn(
                name: "HasFullAnalyticsAccess",
                table: "EventStaffs",
                newName: "CanManageTickets");

            migrationBuilder.RenameColumn(
                name: "HasFinancialAuthority",
                table: "EventStaffs",
                newName: "CanEditEvent");
        }
    }
}
