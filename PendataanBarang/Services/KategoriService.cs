using AutoMapper;
using PendataanBarang.DTOs;
using PendataanBarang.Helpers;
using PendataanBarang.Models;
using PendataanBarang.Repositories;

namespace PendataanBarang.Services
{
    public class KategoriService : IKategoriService
    {
        private readonly IGenericRepository<Kategori> _repo;
        private readonly IMapper _mapper;

        public KategoriService(IGenericRepository<Kategori> repo, IMapper mapper)
        {
            _repo = repo;
            _mapper = mapper;
        }

        public async Task<ServiceResult<IEnumerable<KategoriDTO>>> GetAllAsync()
        {
            var data = await _repo.GetAllAsync();
            var dto = _mapper.Map<IEnumerable<KategoriDTO>>(data);
            return ServiceResult<IEnumerable<KategoriDTO>>.SuccessResult(dto);
        }

        public async Task<ServiceResult<KategoriDTO>> GetByIdAsync(int id)
        {
            var data = await _repo.GetByIdAsync(id);
            if (data == null)
                return ServiceResult<KategoriDTO>.FailResult("Kategori tidak ditemukan.");

            return ServiceResult<KategoriDTO>.SuccessResult(_mapper.Map<KategoriDTO>(data));
        }

        public async Task<ServiceResult<KategoriDTO>> CreateAsync(KategoriCreateDTO dto)
        {
            var entity = _mapper.Map<Kategori>(dto);
            await _repo.InsertAsync(entity);
            await _repo.SaveAsync();
            return ServiceResult<KategoriDTO>.SuccessResult(_mapper.Map<KategoriDTO>(entity));
        }

        public async Task<ServiceResult<KategoriDTO>> UpdateAsync(int id, KategoriCreateDTO dto)
        {
            var entity = await _repo.GetByIdAsync(id);
            if (entity == null)
                return ServiceResult<KategoriDTO>.FailResult("Kategori tidak ditemukan.");

            entity.NamaKategori = dto.NamaKategori;
            _repo.Update(entity);
            await _repo.SaveAsync();
            return ServiceResult<KategoriDTO>.SuccessResult(_mapper.Map<KategoriDTO>(entity));
        }

        public async Task<ServiceResult<bool>> DeleteAsync(int id)
        {
            var entity = await _repo.GetByIdAsync(id);
            if (entity == null)
                return ServiceResult<bool>.FailResult("Kategori tidak ditemukan.");

            _repo.Delete(entity);
            await _repo.SaveAsync();
            return ServiceResult<bool>.SuccessResult(true, "Kategori berhasil dihapus.");
        }
    }
}