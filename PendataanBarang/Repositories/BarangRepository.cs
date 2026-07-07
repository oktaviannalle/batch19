using Microsoft.EntityFrameworkCore;
using PendataanBarang.Data;
using PendataanBarang.Models;

namespace PendataanBarang.Repositories
{
    public class BarangRepository : GenericRepository<Barang>, IBarangRepository
    {
        private readonly AppDbContext _context;

        public BarangRepository(AppDbContext context) : base(context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Barang>> GetAllWithKategoriAsync() =>
            await _context.Barangs.Include(b => b.Kategori).ToListAsync();

        public async Task<Barang?> GetByIdWithKategoriAsync(int id) =>
            await _context.Barangs.Include(b => b.Kategori).FirstOrDefaultAsync(b => b.Id == id);
    }
}