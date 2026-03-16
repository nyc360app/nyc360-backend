using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace NYC360.Infrastructure.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class AddHousing : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "HouseInfos",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Title = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    IsRenting = table.Column<bool>(type: "bit", nullable: false),
                    MoveInDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    MoveOutDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    HouseholdType = table.Column<int>(type: "int", nullable: false),
                    MaxOccupants = table.Column<int>(type: "int", nullable: false),
                    UnitNumber = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    GoogleMapLink = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    NumberOfRooms = table.Column<int>(type: "int", nullable: false),
                    NumberOfBathrooms = table.Column<int>(type: "int", nullable: false),
                    StartingPrice = table.Column<int>(type: "int", nullable: false),
                    SecurityDeposit = table.Column<int>(type: "int", nullable: true),
                    BrokerFee = table.Column<int>(type: "int", nullable: true),
                    MonthlyCostRange = table.Column<int>(type: "int", nullable: true),
                    BuildingType = table.Column<int>(type: "int", nullable: false),
                    YearBuilt = table.Column<int>(type: "int", nullable: true),
                    RenovatedIn = table.Column<int>(type: "int", nullable: true),
                    Size = table.Column<int>(type: "int", nullable: true),
                    FloorLevel = table.Column<int>(type: "int", nullable: true),
                    HeatingSystem = table.Column<int>(type: "int", nullable: false),
                    CoolingSystem = table.Column<int>(type: "int", nullable: false),
                    TemperatureControl = table.Column<int>(type: "int", nullable: false),
                    LaundryType = table.Column<int>(type: "int", nullable: false),
                    AcceptedHousingPrograms = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    AcceptedBuyerPrograms = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    NearbySubwayLines = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    UtilitiesIncluded = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IsShortTermStayAllowed = table.Column<bool>(type: "bit", nullable: false),
                    IsShortStayEligible = table.Column<bool>(type: "bit", nullable: false),
                    IsFurnished = table.Column<bool>(type: "bit", nullable: false),
                    IsAcceptsHousingVouchers = table.Column<bool>(type: "bit", nullable: false),
                    IsFamilyAndKidsFriendly = table.Column<bool>(type: "bit", nullable: false),
                    IsPetsFriendly = table.Column<bool>(type: "bit", nullable: false),
                    IsAccessibilityFriendly = table.Column<bool>(type: "bit", nullable: false),
                    IsSmokingAllowed = table.Column<bool>(type: "bit", nullable: false),
                    RentingLeaseType = table.Column<int>(type: "int", nullable: true),
                    RentingIsShared = table.Column<bool>(type: "bit", nullable: true),
                    RentingIsSharedBathroom = table.Column<bool>(type: "bit", nullable: true),
                    RentingIsSharedKitchen = table.Column<bool>(type: "bit", nullable: true),
                    RentingAboutCurrentResident = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    RentingRulesAndPolicies = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    RentingRoommateGroupChat = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    UserId = table.Column<int>(type: "int", nullable: false),
                    AddressId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_HouseInfos", x => x.Id);
                    table.ForeignKey(
                        name: "FK_HouseInfos_Addresses_AddressId",
                        column: x => x.AddressId,
                        principalTable: "Addresses",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_HouseInfos_UserProfiles_UserId",
                        column: x => x.UserId,
                        principalTable: "UserProfiles",
                        principalColumn: "UserId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "HousingAttachments",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Url = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    HousingId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_HousingAttachments", x => x.Id);
                    table.ForeignKey(
                        name: "FK_HousingAttachments_HouseInfos_HousingId",
                        column: x => x.HousingId,
                        principalTable: "HouseInfos",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_HouseInfos_AddressId",
                table: "HouseInfos",
                column: "AddressId");

            migrationBuilder.CreateIndex(
                name: "IX_HouseInfos_UserId",
                table: "HouseInfos",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_HousingAttachments_HousingId",
                table: "HousingAttachments",
                column: "HousingId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "HousingAttachments");

            migrationBuilder.DropTable(
                name: "HouseInfos");
        }
    }
}
