using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace NYC360.Infrastructure.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class adjustuserlocationidtoaddressid : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_EventStaffs_AspNetUsers_UserId",
                table: "EventStaffs");

            migrationBuilder.DropForeignKey(
                name: "FK_UserProfiles_Locations_LocationId",
                table: "UserProfiles");

            migrationBuilder.DropIndex(
                name: "IX_UserProfiles_LocationId",
                table: "UserProfiles");

            migrationBuilder.DropIndex(
                name: "IX_Addresses_ManagedByUserId",
                table: "Addresses");

            migrationBuilder.DropColumn(
                name: "OrganizationType",
                table: "BusinessInfos");

            migrationBuilder.RenameColumn(
                name: "LocationId",
                table: "UserProfiles",
                newName: "AddressId");

            migrationBuilder.RenameColumn(
                name: "ProfitModel",
                table: "BusinessInfos",
                newName: "OwnershipType");

            migrationBuilder.AddColumn<bool>(
                name: "IsInsured",
                table: "BusinessInfos",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "IsLicensedInNyc",
                table: "BusinessInfos",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "IsPublic",
                table: "BusinessInfos",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.CreateTable(
                name: "VisitorInfos",
                columns: table => new
                {
                    UserId = table.Column<int>(type: "int", nullable: false),
                    CityOfOrigin = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CountryOfOrigin = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    VisitPurpose = table.Column<int>(type: "int", nullable: false),
                    LengthOfStay = table.Column<int>(type: "int", nullable: false),
                    ReceiveEventAndCultureRecommendations = table.Column<bool>(type: "bit", nullable: false),
                    EnableLocationBasedSuggestions = table.Column<bool>(type: "bit", nullable: false),
                    SavePlacesEventsGuides = table.Column<bool>(type: "bit", nullable: false),
                    DiscoverableProfile = table.Column<bool>(type: "bit", nullable: false),
                    AllowMessagesFromNycPartners = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_VisitorInfos", x => x.UserId);
                    table.ForeignKey(
                        name: "FK_VisitorInfos_UserProfiles_UserId",
                        column: x => x.UserId,
                        principalTable: "UserProfiles",
                        principalColumn: "UserId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Addresses_ManagedByUserId",
                table: "Addresses",
                column: "ManagedByUserId",
                unique: true,
                filter: "[ManagedByUserId] IS NOT NULL");

            migrationBuilder.AddForeignKey(
                name: "FK_EventStaffs_UserProfiles_UserId",
                table: "EventStaffs",
                column: "UserId",
                principalTable: "UserProfiles",
                principalColumn: "UserId",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_EventStaffs_UserProfiles_UserId",
                table: "EventStaffs");

            migrationBuilder.DropTable(
                name: "VisitorInfos");

            migrationBuilder.DropIndex(
                name: "IX_Addresses_ManagedByUserId",
                table: "Addresses");

            migrationBuilder.DropColumn(
                name: "IsInsured",
                table: "BusinessInfos");

            migrationBuilder.DropColumn(
                name: "IsLicensedInNyc",
                table: "BusinessInfos");

            migrationBuilder.DropColumn(
                name: "IsPublic",
                table: "BusinessInfos");

            migrationBuilder.RenameColumn(
                name: "AddressId",
                table: "UserProfiles",
                newName: "LocationId");

            migrationBuilder.RenameColumn(
                name: "OwnershipType",
                table: "BusinessInfos",
                newName: "ProfitModel");

            migrationBuilder.AddColumn<int>(
                name: "OrganizationType",
                table: "BusinessInfos",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_UserProfiles_LocationId",
                table: "UserProfiles",
                column: "LocationId");

            migrationBuilder.CreateIndex(
                name: "IX_Addresses_ManagedByUserId",
                table: "Addresses",
                column: "ManagedByUserId");

            migrationBuilder.AddForeignKey(
                name: "FK_EventStaffs_AspNetUsers_UserId",
                table: "EventStaffs",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_UserProfiles_Locations_LocationId",
                table: "UserProfiles",
                column: "LocationId",
                principalTable: "Locations",
                principalColumn: "Id");
        }
    }
}
