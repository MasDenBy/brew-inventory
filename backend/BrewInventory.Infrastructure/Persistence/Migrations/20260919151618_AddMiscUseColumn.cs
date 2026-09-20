using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BrewInventory.Infrastructure.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class AddMiscUseColumn : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "Unit",
                table: "Miscs",
                type: "TEXT",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "INTEGER");

            migrationBuilder.AddColumn<string>(
                name: "Use",
                table: "Miscs",
                type: "TEXT",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Use",
                table: "Miscs");

            migrationBuilder.AlterColumn<int>(
                name: "Unit",
                table: "Miscs",
                type: "INTEGER",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "TEXT");
        }
    }
}
