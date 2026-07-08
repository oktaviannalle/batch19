using FluentValidation;
using PendataanBarang.DTOs;

namespace PendataanBarang.Validators
{
    public class BarangCreateValidator : AbstractValidator<BarangCreateDTO>
    {
        public BarangCreateValidator()
        {
            RuleFor(x => x.Kode)
                .NotEmpty().WithMessage("Kode barang wajib diisi.")
                .MaximumLength(20).WithMessage("Kode maksimal 20 karakter.");

            RuleFor(x => x.Nama)
                .NotEmpty().WithMessage("Nama barang wajib diisi.")
                .Length(3, 100).WithMessage("Nama harus 3-100 karakter.");

            RuleFor(x => x.Stok)
                .GreaterThanOrEqualTo(0).WithMessage("Stok tidak boleh negatif.");


            RuleFor(x => x.KategoriId)
                .GreaterThan(0).WithMessage("Kategori wajib di isi.");
        }
    }
}