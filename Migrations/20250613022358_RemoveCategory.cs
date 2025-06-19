using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Efood_Menu.Migrations
{
    /// <inheritdoc />
    public partial class RemoveCategory : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Categories_Menus_MenuId",
                table: "Categories");

            migrationBuilder.DropIndex(
                name: "IX_Categories_MenuId",
                table: "Categories");

            migrationBuilder.DropColumn(
                name: "MenuId",
                table: "Categories");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "MenuId",
                table: "Categories",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Categories_MenuId",
                table: "Categories",
                column: "MenuId");

            migrationBuilder.AddForeignKey(
                name: "FK_Categories_Menus_MenuId",
                table: "Categories",
                column: "MenuId",
                principalTable: "Menus",
                principalColumn: "Id");
        }
    }
}
