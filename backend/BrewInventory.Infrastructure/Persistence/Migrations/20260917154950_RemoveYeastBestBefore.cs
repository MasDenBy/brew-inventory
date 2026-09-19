using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BrewInventory.Infrastructure.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class RemoveYeastBestBefore : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "BestBefore",
                table: "Yeasts");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateOnly>(
                name: "BestBefore",
                table: "Yeasts",
                type: "TEXT",
                nullable: true);
        }
    }
}
