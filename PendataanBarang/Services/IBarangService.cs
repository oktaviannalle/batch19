using PendataanBarang.DTOs;
using PendataanBarang.Helpers;

namespace PendataanBarang.Services
{
    public interface IBarangService
    {
        Task<ServiceResult<IEnumerable<BarangDTO>>> GetAllAsync();
        Task<ServiceResult<BarangDTO>> GetByIdAsync(int id);
        Task<ServiceResult<BarangDTO>> CreateAsync(BarangCreateDTO dto);
        Task<ServiceResult<BarangDTO>> UpdateAsync(int id, BarangCreateDTO dto);
        Task<ServiceResult<bool>> DeleteAsync(int id);
    }
}