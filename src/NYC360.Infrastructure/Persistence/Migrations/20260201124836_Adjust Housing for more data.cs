using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace NYC360.Infrastructure.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class AdjustHousingformoredata : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_HouseInfos_Addresses_AddressId",
                table: "HouseInfos");

            migrationBuilder.DropIndex(
                name: "IX_HouseInfos_AddressId",
                table: "HouseInfos");

            migrationBuilder.DropColumn(
                name: "RentingIsShared",
                table: "HouseInfos");

            migrationBuilder.DropColumn(
                name: "RentingIsSharedBathroom",
                table: "HouseInfos");

            migrationBuilder.RenameColumn(
                name: "UtilitiesIncluded",
                table: "HouseInfos",
                newName: "RentHousingPrograms");

            migrationBuilder.RenameColumn(
                name: "RentingIsSharedKitchen",
                table: "HouseInfos",
                newName: "AllowColisterEditing");

            migrationBuilder.RenameColumn(
                name: "NumberOfRooms",
                table: "HouseInfos",
                newName: "PropertyType");

            migrationBuilder.RenameColumn(
                name: "NumberOfBathrooms",
                table: "HouseInfos",
                newName: "HouseType");

            migrationBuilder.RenameColumn(
                name: "NearbySubwayLines",
                table: "HouseInfos",
                newName: "NearbyTransportation");

            migrationBuilder.RenameColumn(
                name: "LaundryType",
                table: "HouseInfos",
                newName: "Bedrooms");

            migrationBuilder.RenameColumn(
                name: "AddressId",
                table: "HouseInfos",
                newName: "Bathrooms");

            migrationBuilder.RenameColumn(
                name: "AcceptedHousingPrograms",
                table: "HouseInfos",
                newName: "FullAddress");

            migrationBuilder.AlterColumn<bool>(
                name: "IsShortTermStayAllowed",
                table: "HouseInfos",
                type: "bit",
                nullable: true,
                oldClrType: typeof(bool),
                oldType: "bit");

            migrationBuilder.AlterColumn<bool>(
                name: "IsShortStayEligible",
                table: "HouseInfos",
                type: "bit",
                nullable: true,
                oldClrType: typeof(bool),
                oldType: "bit");

            migrationBuilder.AddColumn<bool>(
                name: "AddDirectApplyLink",
                table: "HouseInfos",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<string>(
                name: "Borough",
                table: "HouseInfos",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "CoListingIds",
                table: "HouseInfos",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsPublished",
                table: "HouseInfos",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<string>(
                name: "LaundryTypes",
                table: "HouseInfos",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "[]");

            migrationBuilder.AddColumn<string>(
                name: "Neighborhood",
                table: "HouseInfos",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<int>(
                name: "RentBathroomType",
                table: "HouseInfos",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "RentKitchenType",
                table: "HouseInfos",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "RentPrivacyType",
                table: "HouseInfos",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ZipCode",
                table: "HouseInfos",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "AddDirectApplyLink",
                table: "HouseInfos");

            migrationBuilder.DropColumn(
                name: "Borough",
                table: "HouseInfos");

            migrationBuilder.DropColumn(
                name: "CoListingIds",
                table: "HouseInfos");

            migrationBuilder.DropColumn(
                name: "IsPublished",
                table: "HouseInfos");

            migrationBuilder.DropColumn(
                name: "LaundryTypes",
                table: "HouseInfos");

            migrationBuilder.DropColumn(
                name: "Neighborhood",
                table: "HouseInfos");

            migrationBuilder.DropColumn(
                name: "RentBathroomType",
                table: "HouseInfos");

            migrationBuilder.DropColumn(
                name: "RentKitchenType",
                table: "HouseInfos");

            migrationBuilder.DropColumn(
                name: "RentPrivacyType",
                table: "HouseInfos");

            migrationBuilder.DropColumn(
                name: "ZipCode",
                table: "HouseInfos");

            migrationBuilder.RenameColumn(
                name: "RentHousingPrograms",
                table: "HouseInfos",
                newName: "UtilitiesIncluded");

            migrationBuilder.RenameColumn(
                name: "PropertyType",
                table: "HouseInfos",
                newName: "NumberOfRooms");

            migrationBuilder.RenameColumn(
                name: "NearbyTransportation",
                table: "HouseInfos",
                newName: "NearbySubwayLines");

            migrationBuilder.RenameColumn(
                name: "HouseType",
                table: "HouseInfos",
                newName: "NumberOfBathrooms");

            migrationBuilder.RenameColumn(
                name: "FullAddress",
                table: "HouseInfos",
                newName: "AcceptedHousingPrograms");

            migrationBuilder.RenameColumn(
                name: "Bedrooms",
                table: "HouseInfos",
                newName: "LaundryType");

            migrationBuilder.RenameColumn(
                name: "Bathrooms",
                table: "HouseInfos",
                newName: "AddressId");

            migrationBuilder.RenameColumn(
                name: "AllowColisterEditing",
                table: "HouseInfos",
                newName: "RentingIsSharedKitchen");

            migrationBuilder.AlterColumn<bool>(
                name: "IsShortTermStayAllowed",
                table: "HouseInfos",
                type: "bit",
                nullable: false,
                defaultValue: false,
                oldClrType: typeof(bool),
                oldType: "bit",
                oldNullable: true);

            migrationBuilder.AlterColumn<bool>(
                name: "IsShortStayEligible",
                table: "HouseInfos",
                type: "bit",
                nullable: false,
                defaultValue: false,
                oldClrType: typeof(bool),
                oldType: "bit",
                oldNullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "RentingIsShared",
                table: "HouseInfos",
                type: "bit",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "RentingIsSharedBathroom",
                table: "HouseInfos",
                type: "bit",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_HouseInfos_AddressId",
                table: "HouseInfos",
                column: "AddressId");

            migrationBuilder.AddForeignKey(
                name: "FK_HouseInfos_Addresses_AddressId",
                table: "HouseInfos",
                column: "AddressId",
                principalTable: "Addresses",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
