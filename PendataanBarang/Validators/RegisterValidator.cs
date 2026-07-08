using FluentValidation;
using PendataanBarang.DTOs;

namespace PendataanBarang.Validators
{
    public class RegisterValidator : AbstractValidator<RegisterDTO>
    {
        public RegisterValidator()
        {
            RuleFor(x => x.Username)
                .NotEmpty().WithMessage("Username wajib diisi.")
                .Length(3, 50).WithMessage("Username harus 3-50 karakter.");

            RuleFor(x => x.Password)
                .NotEmpty().WithMessage("Password wajib diisi.")
                .MinimumLength(6).WithMessage("Password minimal 6 karakter.");
        }
    }
}