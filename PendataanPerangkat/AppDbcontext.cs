using Microsoft.EntityFrameworkCore;

public class AppDbContext : DbContext
{
    public DbSet<Perangkat> Perangkats {get; set;}
    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseSqlite("Data Source=aset_it.db");
    }
}