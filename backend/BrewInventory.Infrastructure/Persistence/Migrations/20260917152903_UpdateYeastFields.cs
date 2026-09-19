using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BrewInventory.Infrastructure.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class UpdateYeastFields : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Labaratory",
                table: "Yeasts",
                newName: "Laboratory");

            migrationBuilder.AddColumn<string>(
                name: "Unit",
                table: "Yeasts",
                type: "TEXT",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "ProductId",
                table: "Yeasts",
                type: "TEXT",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Unit",
                table: "Yeasts");

            migrationBuilder.DropColumn(
                name: "ProductId",
                table: "Yeasts");

            migrationBuilder.RenameColumn(
                name: "Laboratory",
                table: "Yeasts",
                newName: "Labaratory");
        }
    }
}
