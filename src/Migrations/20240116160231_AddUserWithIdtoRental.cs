using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CarRentalAPI.Migrations
{
    /// <inheritdoc />
    public partial class AddUserWithIdtoRental : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "OwnerId",
                table: "rentalOffices",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_rentalOffices_OwnerId",
                table: "rentalOffices",
                column: "OwnerId");

            migrationBuilder.AddForeignKey(
                name: "FK_rentalOffices_users_OwnerId",
                table: "rentalOffices",
                column: "OwnerId",
                principalTable: "users",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_rentalOffices_users_OwnerId",
                table: "rentalOffices");

            migrationBuilder.DropIndex(
                name: "IX_rentalOffices_OwnerId",
                table: "rentalOffices");

            migrationBuilder.DropColumn(
                name: "OwnerId",
                table: "rentalOffices");
        }
    }
}
