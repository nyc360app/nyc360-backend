using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace NYC360.Infrastructure.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class AddAdminprofiletable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AdminProfile_AspNetUsers_UserId",
                table: "AdminProfile");

            migrationBuilder.DropPrimaryKey(
                name: "PK_AdminProfile",
                table: "AdminProfile");

            migrationBuilder.RenameTable(
                name: "AdminProfile",
                newName: "AdminProfiles");

            migrationBuilder.AddPrimaryKey(
                name: "PK_AdminProfiles",
                table: "AdminProfiles",
                column: "UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_AdminProfiles_AspNetUsers_UserId",
                table: "AdminProfiles",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AdminProfiles_AspNetUsers_UserId",
                table: "AdminProfiles");

            migrationBuilder.DropPrimaryKey(
                name: "PK_AdminProfiles",
                table: "AdminProfiles");

            migrationBuilder.RenameTable(
                name: "AdminProfiles",
                newName: "AdminProfile");

            migrationBuilder.AddPrimaryKey(
                name: "PK_AdminProfile",
                table: "AdminProfile",
                column: "UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_AdminProfile_AspNetUsers_UserId",
                table: "AdminProfile",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
