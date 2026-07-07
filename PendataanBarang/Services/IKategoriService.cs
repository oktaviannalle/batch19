using PendataanBarang.DTOs;
using PendataanBarang.Helpers;

namespace PendataanBarang.Services
{
    public interface IKategoriService
    {
        Task<ServiceResult<IEnumerable<KategoriDTO>>> GetAllAsync();
        Task<ServiceResult<KategoriDTO>> GetByIdAsync(int id);
        Task<ServiceResult<KategoriDTO>> CreateAsync(KategoriCreateDTO dto);
        Task<ServiceResult<KategoriDTO>> UpdateAsync(int id, KategoriCreateDTO dto);
        Task<ServiceResult<bool>> DeleteAsync(int id);
    }
}