using Microsoft.EntityFrameworkCore;

public class InventoryDbcontext : DbContext
{
    public DbSet<Barang>Barangs {get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseSqlite("Data Source=inventory.db");
    }
}