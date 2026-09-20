using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BrewInventory.Infrastructure.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class DenormalizeRecipeEntities : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            // Drop foreign keys
            migrationBuilder.DropForeignKey(
                name: "FK_RecipeFermentables_Fermentables_FermentableId",
                table: "RecipeFermentables");

            migrationBuilder.DropForeignKey(
                name: "FK_RecipeHops_Hops_HopId",
                table: "RecipeHops");

            migrationBuilder.DropForeignKey(
                name: "FK_RecipeMiscs_Miscs_MiscId",
                table: "RecipeMiscs");

            migrationBuilder.DropForeignKey(
                name: "FK_RecipeYeasts_Yeasts_YeastId",
                table: "RecipeYeasts");

            // Drop indexes
            migrationBuilder.DropIndex(
                name: "IX_RecipeYeasts_YeastId",
                table: "RecipeYeasts");

            migrationBuilder.DropIndex(
                name: "IX_RecipeMiscs_MiscId",
                table: "RecipeMiscs");

            migrationBuilder.DropIndex(
                name: "IX_RecipeHops_HopId",
                table: "RecipeHops");

            migrationBuilder.DropIndex(
                name: "IX_RecipeFermentables_FermentableId",
                table: "RecipeFermentables");

            // Drop old FK columns
            migrationBuilder.DropColumn(
                name: "FermentableId",
                table: "RecipeFermentables");

            migrationBuilder.DropColumn(
                name: "HopId",
                table: "RecipeHops");

            migrationBuilder.DropColumn(
                name: "YeastId",
                table: "RecipeYeasts");

            migrationBuilder.DropColumn(
                name: "MiscId",
                table: "RecipeMiscs");

            // Add Style to Recipes
            migrationBuilder.AddColumn<string>(
                name: "Style",
                table: "Recipes",
                type: "TEXT",
                nullable: true);

            // Add denormalized columns to RecipeFermentables
            migrationBuilder.AddColumn<string>(
                name: "Name",
                table: "RecipeFermentables",
                type: "TEXT",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<int>(
                name: "Type",
                table: "RecipeFermentables",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "Supplier",
                table: "RecipeFermentables",
                type: "TEXT",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Origin",
                table: "RecipeFermentables",
                type: "TEXT",
                nullable: true);

            migrationBuilder.AddColumn<double>(
                name: "Color",
                table: "RecipeFermentables",
                type: "REAL",
                nullable: true);

            migrationBuilder.AddColumn<double>(
                name: "Potential",
                table: "RecipeFermentables",
                type: "REAL",
                nullable: true);

            // Add denormalized columns to RecipeHops
            migrationBuilder.AddColumn<string>(
                name: "Name",
                table: "RecipeHops",
                type: "TEXT",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<double>(
                name: "Alpha",
                table: "RecipeHops",
                type: "REAL",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "Type",
                table: "RecipeHops",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "Origin",
                table: "RecipeHops",
                type: "TEXT",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "Use",
                table: "RecipeHops",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<double>(
                name: "Time",
                table: "RecipeHops",
                type: "REAL",
                nullable: true);

            // Add denormalized columns to RecipeYeasts
            migrationBuilder.AddColumn<string>(
                name: "Name",
                table: "RecipeYeasts",
                type: "TEXT",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Laboratory",
                table: "RecipeYeasts",
                type: "TEXT",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "Type",
                table: "RecipeYeasts",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "Form",
                table: "RecipeYeasts",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<double>(
                name: "Attenuation",
                table: "RecipeYeasts",
                type: "REAL",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Unit",
                table: "RecipeYeasts",
                type: "TEXT",
                nullable: true);

            // Add denormalized columns to RecipeMiscs
            migrationBuilder.AddColumn<string>(
                name: "Name",
                table: "RecipeMiscs",
                type: "TEXT",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<int>(
                name: "Type",
                table: "RecipeMiscs",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "Unit",
                table: "RecipeMiscs",
                type: "TEXT",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Use",
                table: "RecipeMiscs",
                type: "TEXT",
                nullable: true);

            migrationBuilder.AddColumn<double>(
                name: "Time",
                table: "RecipeMiscs",
                type: "REAL",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            // Remove denormalized columns from RecipeMiscs
            migrationBuilder.DropColumn(name: "Time", table: "RecipeMiscs");
            migrationBuilder.DropColumn(name: "Use", table: "RecipeMiscs");
            migrationBuilder.DropColumn(name: "Unit", table: "RecipeMiscs");
            migrationBuilder.DropColumn(name: "Type", table: "RecipeMiscs");
            migrationBuilder.DropColumn(name: "Name", table: "RecipeMiscs");

            // Remove denormalized columns from RecipeYeasts
            migrationBuilder.DropColumn(name: "Unit", table: "RecipeYeasts");
            migrationBuilder.DropColumn(name: "Attenuation", table: "RecipeYeasts");
            migrationBuilder.DropColumn(name: "Form", table: "RecipeYeasts");
            migrationBuilder.DropColumn(name: "Type", table: "RecipeYeasts");
            migrationBuilder.DropColumn(name: "Laboratory", table: "RecipeYeasts");
            migrationBuilder.DropColumn(name: "Name", table: "RecipeYeasts");

            // Remove denormalized columns from RecipeHops
            migrationBuilder.DropColumn(name: "Time", table: "RecipeHops");
            migrationBuilder.DropColumn(name: "Use", table: "RecipeHops");
            migrationBuilder.DropColumn(name: "Origin", table: "RecipeHops");
            migrationBuilder.DropColumn(name: "Type", table: "RecipeHops");
            migrationBuilder.DropColumn(name: "Alpha", table: "RecipeHops");
            migrationBuilder.DropColumn(name: "Name", table: "RecipeHops");

            // Remove denormalized columns from RecipeFermentables
            migrationBuilder.DropColumn(name: "Potential", table: "RecipeFermentables");
            migrationBuilder.DropColumn(name: "Color", table: "RecipeFermentables");
            migrationBuilder.DropColumn(name: "Origin", table: "RecipeFermentables");
            migrationBuilder.DropColumn(name: "Supplier", table: "RecipeFermentables");
            migrationBuilder.DropColumn(name: "Type", table: "RecipeFermentables");
            migrationBuilder.DropColumn(name: "Name", table: "RecipeFermentables");

            // Remove Style from Recipes
            migrationBuilder.DropColumn(name: "Style", table: "Recipes");

            // Restore FK columns
            migrationBuilder.AddColumn<int>(
                name: "FermentableId",
                table: "RecipeFermentables",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "HopId",
                table: "RecipeHops",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "YeastId",
                table: "RecipeYeasts",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "MiscId",
                table: "RecipeMiscs",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0);

            // Restore indexes
            migrationBuilder.CreateIndex(
                name: "IX_RecipeFermentables_FermentableId",
                table: "RecipeFermentables",
                column: "FermentableId");

            migrationBuilder.CreateIndex(
                name: "IX_RecipeHops_HopId",
                table: "RecipeHops",
                column: "HopId");

            migrationBuilder.CreateIndex(
                name: "IX_RecipeMiscs_MiscId",
                table: "RecipeMiscs",
                column: "MiscId");

            migrationBuilder.CreateIndex(
                name: "IX_RecipeYeasts_YeastId",
                table: "RecipeYeasts",
                column: "YeastId");

            // Restore foreign keys
            migrationBuilder.AddForeignKey(
                name: "FK_RecipeFermentables_Fermentables_FermentableId",
                table: "RecipeFermentables",
                column: "FermentableId",
                principalTable: "Fermentables",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_RecipeHops_Hops_HopId",
                table: "RecipeHops",
                column: "HopId",
                principalTable: "Hops",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_RecipeMiscs_Miscs_MiscId",
                table: "RecipeMiscs",
                column: "MiscId",
                principalTable: "Miscs",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_RecipeYeasts_Yeasts_YeastId",
                table: "RecipeYeasts",
                column: "YeastId",
                principalTable: "Yeasts",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
