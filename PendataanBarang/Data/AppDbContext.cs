using Microsoft.EntityFrameworkCore;
using PendataanBarang.Models;

namespace PendataanBarang.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        public DbSet<Kategori> Kategoris { get; set; }
        public DbSet<Barang> Barangs { get; set; }
        public DbSet<User> Users { get; set; }
    }
}