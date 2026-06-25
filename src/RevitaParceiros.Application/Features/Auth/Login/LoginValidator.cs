using FluentValidation;

namespace RevitaParceiros.Application.Features.Auth.Login;

public sealed class LoginValidator : AbstractValidator<LoginRequest>
{
    public LoginValidator()
    {
        RuleFor(x => x.Login)
            .NotEmpty().WithMessage("O campo Login (Email) é obrigatório.");

        RuleFor(x => x.Senha)
            .NotEmpty().WithMessage("A Senha é obrigatória.")
            .MinimumLength(6).WithMessage("A Senha deve ter pelo menos 6 caracteres.");
    }
}
