using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Efood_Menu.Migrations
{
    /// <inheritdoc />
    public partial class RemoveMenu : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_FoodItems_Menus_MenuId",
                table: "FoodItems");

            migrationBuilder.DropTable(
                name: "Menus");

            migrationBuilder.DropIndex(
                name: "IX_FoodItems_MenuId",
                table: "FoodItems");

            migrationBuilder.DropColumn(
                name: "MenuId",
                table: "FoodItems");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "MenuId",
                table: "FoodItems",
                type: "int",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "Menus",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Menus", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_FoodItems_MenuId",
                table: "FoodItems",
                column: "MenuId");

            migrationBuilder.AddForeignKey(
                name: "FK_FoodItems_Menus_MenuId",
                table: "FoodItems",
                column: "MenuId",
                principalTable: "Menus",
                principalColumn: "Id");
        }
    }
}
