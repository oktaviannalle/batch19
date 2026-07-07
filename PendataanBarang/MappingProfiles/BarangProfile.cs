using AutoMapper;
using PendataanBarang.DTOs;
using PendataanBarang.Models;

namespace PendataanBarang.MappingProfiles
{
    public class BarangProfile : Profile
    {
        public BarangProfile()
        {
            CreateMap<Barang, BarangDTO>()
                .ForMember(dest => dest.NamaKategori, opt => opt.MapFrom(src => src.Kategori!.NamaKategori));

            CreateMap<BarangCreateDTO, Barang>();
            CreateMap<Kategori, KategoriDTO>();
            CreateMap<KategoriCreateDTO, Kategori>();
        }
    }
}