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

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<Barang>()
                .HasOne(b => b.Kategori)              // satu Barang punya satu Kategori
                .WithMany(k => k.Barangs)              // satu Kategori punya banyak Barang
                .HasForeignKey(b => b.KategoriId)      
                .OnDelete(DeleteBehavior.Restrict);    

            modelBuilder.Entity<Kategori>()
                .Property(k => k.NamaKategori)
                .IsRequired()
                .HasMaxLength(100);

            modelBuilder.Entity<Barang>()
                .Property(b => b.Kode)
                .IsRequired()
                .HasMaxLength(20);

            modelBuilder.Entity<Barang>()
                .HasIndex(b => b.Kode)
                .IsUnique();   

            modelBuilder.Entity<Barang>()
                .Property(b => b.Nama)
                .IsRequired()
                .HasMaxLength(100);

            modelBuilder.Entity<User>()
                .HasIndex(u => u.Username)
                .IsUnique();

            modelBuilder.Entity<User>()
                .Property(u => u.Username)
                .IsRequired()
                .HasMaxLength(50);
        }
    }
}