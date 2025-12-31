using BrewInventory.App.Data.Entities;
using Microsoft.EntityFrameworkCore;

namespace BrewInventory.App.Data;

public class BrewInventoryContext(DbContextOptions<BrewInventoryContext> options) : DbContext(options)
{
    public DbSet<Fermentable> Fermentables { get; set; }
    public DbSet<Hop> Hops { get; set; }
    public DbSet<Yeast> Yeasts { get; set; }
    public DbSet<Misc> Miscs { get; set; }
    public DbSet<Recipe> Recipes { get; set; }
    public DbSet<RecipeFermentable> RecipeFermentables { get; set; }
    public DbSet<RecipeHop> RecipeHops { get; set; }
    public DbSet<RecipeYeast> RecipeYeasts { get; set; }
    public DbSet<RecipeMisc> RecipeMiscs { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseSqlite("Data Source=brewinventory.db");
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // Configure Recipe relationships
        modelBuilder.Entity<RecipeFermentable>()
            .HasOne(rf => rf.Recipe)
            .WithMany(r => r.RecipeFermentables)
            .HasForeignKey(rf => rf.RecipeId)
            .OnDelete(DeleteBehavior.Cascade);

        modelBuilder.Entity<RecipeFermentable>()
            .HasOne(rf => rf.Fermentable)
            .WithMany()
            .HasForeignKey(rf => rf.FermentableId)
            .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<RecipeHop>()
            .HasOne(rh => rh.Recipe)
            .WithMany(r => r.RecipeHops)
            .HasForeignKey(rh => rh.RecipeId)
            .OnDelete(DeleteBehavior.Cascade);

        modelBuilder.Entity<RecipeHop>()
            .HasOne(rh => rh.Hop)
            .WithMany()
            .HasForeignKey(rh => rh.HopId)
            .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<RecipeYeast>()
            .HasOne(ry => ry.Recipe)
            .WithMany(r => r.RecipeYeasts)
            .HasForeignKey(ry => ry.RecipeId)
            .OnDelete(DeleteBehavior.Cascade);

        modelBuilder.Entity<RecipeYeast>()
            .HasOne(ry => ry.Yeast)
            .WithMany()
            .HasForeignKey(ry => ry.YeastId)
            .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<RecipeMisc>()
            .HasOne(rm => rm.Recipe)
            .WithMany(r => r.RecipeMiscs)
            .HasForeignKey(rm => rm.RecipeId)
            .OnDelete(DeleteBehavior.Cascade);

        modelBuilder.Entity<RecipeMisc>()
            .HasOne(rm => rm.Misc)
            .WithMany()
            .HasForeignKey(rm => rm.MiscId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}
