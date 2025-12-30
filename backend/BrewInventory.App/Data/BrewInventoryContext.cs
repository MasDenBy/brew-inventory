using BrewInventory.App.Data.Entities;
using Microsoft.EntityFrameworkCore;

namespace BrewInventory.App.Data;

public class BrewInventoryContext(DbContextOptions<BrewInventoryContext> options) : DbContext(options)
{
    public DbSet<Fermentable> Fermentables { get; set; }
    public DbSet<Hop> Hops { get; set; }
    public DbSet<Yeast> Yeasts { get; set; }
    public DbSet<Misc> Miscs { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseSqlite("Data Source=brewinventory.db");
    }
}
