using Microsoft.EntityFrameworkCore;
using PendataanPerangkat.Model; 

namespace PendataanPerangkat
{
    public class AppDbContext : DbContext
    {
        public DbSet<Perangkat> Perangkats { get; set; }
        public DbSet<Kategori> Kategoris { get; set; } 

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlite("Data Source=aset_it.db");
        }
    }
}