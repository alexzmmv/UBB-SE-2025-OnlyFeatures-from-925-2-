using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WinUIApp.Data.Migrations
{
    /// <inheritdoc />
    public partial class FixedCategory : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_DrinkCategories_Categories_CategoryId1",
                table: "DrinkCategories");

            migrationBuilder.DropForeignKey(
                name: "FK_DrinkCategories_Drinks_DrinkId1",
                table: "DrinkCategories");

            migrationBuilder.DropIndex(
                name: "IX_DrinkCategories_CategoryId1",
                table: "DrinkCategories");

            migrationBuilder.DropIndex(
                name: "IX_DrinkCategories_DrinkId1",
                table: "DrinkCategories");

            migrationBuilder.DropColumn(
                name: "CategoryId1",
                table: "DrinkCategories");

            migrationBuilder.DropColumn(
                name: "DrinkId1",
                table: "DrinkCategories");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "CategoryId1",
                table: "DrinkCategories",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "DrinkId1",
                table: "DrinkCategories",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_DrinkCategories_CategoryId1",
                table: "DrinkCategories",
                column: "CategoryId1");

            migrationBuilder.CreateIndex(
                name: "IX_DrinkCategories_DrinkId1",
                table: "DrinkCategories",
                column: "DrinkId1");

            migrationBuilder.AddForeignKey(
                name: "FK_DrinkCategories_Categories_CategoryId1",
                table: "DrinkCategories",
                column: "CategoryId1",
                principalTable: "Categories",
                principalColumn: "CategoryId");

            migrationBuilder.AddForeignKey(
                name: "FK_DrinkCategories_Drinks_DrinkId1",
                table: "DrinkCategories",
                column: "DrinkId1",
                principalTable: "Drinks",
                principalColumn: "DrinkId");
        }
    }
}
