using FluentValidation;
using PendataanBarang.DTOs;

namespace PendataanBarang.Validators
{
    public class LoginValidator : AbstractValidator<LoginDTO>
    {
        public LoginValidator()
        {
            RuleFor(x => x.Username)
                .NotEmpty().WithMessage("Username wajib diisi.");

            RuleFor(x => x.Password)
                .NotEmpty().WithMessage("Password wajib diisi.");
        }
    }
}