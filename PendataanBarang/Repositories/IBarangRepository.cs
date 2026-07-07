using PendataanBarang.Models;

namespace PendataanBarang.Repositories
{
    public interface IBarangRepository : IGenericRepository<Barang>
    {
        Task<IEnumerable<Barang>> GetAllWithKategoriAsync();
        Task<Barang?> GetByIdWithKategoriAsync(int id);
    }
}