using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BrewInventory.App.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddRecipeEntities : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Recipes",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Name = table.Column<string>(type: "TEXT", nullable: false),
                    BrewfatherId = table.Column<string>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Recipes", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "RecipeFermentables",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    RecipeId = table.Column<int>(type: "INTEGER", nullable: false),
                    FermentableId = table.Column<int>(type: "INTEGER", nullable: false),
                    Amount = table.Column<decimal>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RecipeFermentables", x => x.Id);
                    table.ForeignKey(
                        name: "FK_RecipeFermentables_Fermentables_FermentableId",
                        column: x => x.FermentableId,
                        principalTable: "Fermentables",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_RecipeFermentables_Recipes_RecipeId",
                        column: x => x.RecipeId,
                        principalTable: "Recipes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "RecipeHops",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    RecipeId = table.Column<int>(type: "INTEGER", nullable: false),
                    HopId = table.Column<int>(type: "INTEGER", nullable: false),
                    Amount = table.Column<decimal>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RecipeHops", x => x.Id);
                    table.ForeignKey(
                        name: "FK_RecipeHops_Hops_HopId",
                        column: x => x.HopId,
                        principalTable: "Hops",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_RecipeHops_Recipes_RecipeId",
                        column: x => x.RecipeId,
                        principalTable: "Recipes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "RecipeMiscs",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    RecipeId = table.Column<int>(type: "INTEGER", nullable: false),
                    MiscId = table.Column<int>(type: "INTEGER", nullable: false),
                    Amount = table.Column<decimal>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RecipeMiscs", x => x.Id);
                    table.ForeignKey(
                        name: "FK_RecipeMiscs_Miscs_MiscId",
                        column: x => x.MiscId,
                        principalTable: "Miscs",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_RecipeMiscs_Recipes_RecipeId",
                        column: x => x.RecipeId,
                        principalTable: "Recipes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "RecipeYeasts",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    RecipeId = table.Column<int>(type: "INTEGER", nullable: false),
                    YeastId = table.Column<int>(type: "INTEGER", nullable: false),
                    Amount = table.Column<decimal>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RecipeYeasts", x => x.Id);
                    table.ForeignKey(
                        name: "FK_RecipeYeasts_Recipes_RecipeId",
                        column: x => x.RecipeId,
                        principalTable: "Recipes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_RecipeYeasts_Yeasts_YeastId",
                        column: x => x.YeastId,
                        principalTable: "Yeasts",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_RecipeFermentables_FermentableId",
                table: "RecipeFermentables",
                column: "FermentableId");

            migrationBuilder.CreateIndex(
                name: "IX_RecipeFermentables_RecipeId",
                table: "RecipeFermentables",
                column: "RecipeId");

            migrationBuilder.CreateIndex(
                name: "IX_RecipeHops_HopId",
                table: "RecipeHops",
                column: "HopId");

            migrationBuilder.CreateIndex(
                name: "IX_RecipeHops_RecipeId",
                table: "RecipeHops",
                column: "RecipeId");

            migrationBuilder.CreateIndex(
                name: "IX_RecipeMiscs_MiscId",
                table: "RecipeMiscs",
                column: "MiscId");

            migrationBuilder.CreateIndex(
                name: "IX_RecipeMiscs_RecipeId",
                table: "RecipeMiscs",
                column: "RecipeId");

            migrationBuilder.CreateIndex(
                name: "IX_RecipeYeasts_RecipeId",
                table: "RecipeYeasts",
                column: "RecipeId");

            migrationBuilder.CreateIndex(
                name: "IX_RecipeYeasts_YeastId",
                table: "RecipeYeasts",
                column: "YeastId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "RecipeFermentables");

            migrationBuilder.DropTable(
                name: "RecipeHops");

            migrationBuilder.DropTable(
                name: "RecipeMiscs");

            migrationBuilder.DropTable(
                name: "RecipeYeasts");

            migrationBuilder.DropTable(
                name: "Recipes");
        }
    }
}
