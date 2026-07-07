using AutoMapper;
using FluentValidation;
using PendataanBarang.DTOs;
using PendataanBarang.Helpers;
using PendataanBarang.Models;
using PendataanBarang.Repositories;

namespace PendataanBarang.Services
{
    public class BarangService : IBarangService
    {
        private readonly IBarangRepository _repo;
        private readonly IMapper _mapper;
        private readonly IValidator<BarangCreateDTO> _validator;

        public BarangService(IBarangRepository repo, IMapper mapper, IValidator<BarangCreateDTO> validator)
        {
            _repo = repo;
            _mapper = mapper;
            _validator = validator;
        }

        public async Task<ServiceResult<IEnumerable<BarangDTO>>> GetAllAsync()
        {
            var data = await _repo.GetAllWithKategoriAsync();
            var dto = _mapper.Map<IEnumerable<BarangDTO>>(data);
            return ServiceResult<IEnumerable<BarangDTO>>.SuccessResult(dto);
        }

        public async Task<ServiceResult<BarangDTO>> GetByIdAsync(int id)
        {
            var data = await _repo.GetByIdWithKategoriAsync(id);
            if (data == null)
                return ServiceResult<BarangDTO>.FailResult("Barang tidak ditemukan.");

            return ServiceResult<BarangDTO>.SuccessResult(_mapper.Map<BarangDTO>(data));
        }

        public async Task<ServiceResult<BarangDTO>> CreateAsync(BarangCreateDTO dto)
        {
            var validation = await _validator.ValidateAsync(dto);
            if (!validation.IsValid)
                return ServiceResult<BarangDTO>.FailResult(string.Join(", ", validation.Errors.Select(e => e.ErrorMessage)));

            var entity = _mapper.Map<Barang>(dto);
            await _repo.InsertAsync(entity);
            await _repo.SaveAsync();

            var created = await _repo.GetByIdWithKategoriAsync(entity.Id);
            return ServiceResult<BarangDTO>.SuccessResult(_mapper.Map<BarangDTO>(created));
        }

        public async Task<ServiceResult<BarangDTO>> UpdateAsync(int id, BarangCreateDTO dto)
        {
            var validation = await _validator.ValidateAsync(dto);
            if (!validation.IsValid)
                return ServiceResult<BarangDTO>.FailResult(string.Join(", ", validation.Errors.Select(e => e.ErrorMessage)));

            var entity = await _repo.GetByIdAsync(id);
            if (entity == null)
                return ServiceResult<BarangDTO>.FailResult("Barang tidak ditemukan.");

            entity.Kode = dto.Kode;
            entity.Nama = dto.Nama;
            entity.Stok = dto.Stok;
            entity.KategoriId = dto.KategoriId;

            _repo.Update(entity);
            await _repo.SaveAsync();

            var updated = await _repo.GetByIdWithKategoriAsync(id);
            return ServiceResult<BarangDTO>.SuccessResult(_mapper.Map<BarangDTO>(updated));
        }

        public async Task<ServiceResult<bool>> DeleteAsync(int id)
        {
            var entity = await _repo.GetByIdAsync(id);
            if (entity == null)
                return ServiceResult<bool>.FailResult("Barang tidak ditemukan.");

            _repo.Delete(entity);
            await _repo.SaveAsync();
            return ServiceResult<bool>.SuccessResult(true, "Barang berhasil dihapus.");
        }
    }
}