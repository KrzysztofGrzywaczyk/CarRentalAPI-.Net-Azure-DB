using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CarRentalAPI.Migrations
{
    /// <inheritdoc />
    public partial class AddUserFieldsToCarDbModel : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "CreatedById",
                table: "cars",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "ManagedById",
                table: "cars",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "ManagerId",
                table: "cars",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_cars_CreatedById",
                table: "cars",
                column: "CreatedById");

            migrationBuilder.CreateIndex(
                name: "IX_cars_ManagerId",
                table: "cars",
                column: "ManagerId");

            migrationBuilder.AddForeignKey(
                name: "FK_cars_users_CreatedById",
                table: "cars",
                column: "CreatedById",
                principalTable: "users",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_cars_users_ManagerId",
                table: "cars",
                column: "ManagerId",
                principalTable: "users",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_cars_users_CreatedById",
                table: "cars");

            migrationBuilder.DropForeignKey(
                name: "FK_cars_users_ManagerId",
                table: "cars");

            migrationBuilder.DropIndex(
                name: "IX_cars_CreatedById",
                table: "cars");

            migrationBuilder.DropIndex(
                name: "IX_cars_ManagerId",
                table: "cars");

            migrationBuilder.DropColumn(
                name: "CreatedById",
                table: "cars");

            migrationBuilder.DropColumn(
                name: "ManagedById",
                table: "cars");

            migrationBuilder.DropColumn(
                name: "ManagerId",
                table: "cars");
        }
    }
}
