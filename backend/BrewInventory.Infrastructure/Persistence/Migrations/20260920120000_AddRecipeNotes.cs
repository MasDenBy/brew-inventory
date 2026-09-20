using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BrewInventory.Infrastructure.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class AddRecipeNotes : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Notes",
                table: "Recipes",
                type: "TEXT",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Notes",
                table: "Recipes");
        }
    }
}
