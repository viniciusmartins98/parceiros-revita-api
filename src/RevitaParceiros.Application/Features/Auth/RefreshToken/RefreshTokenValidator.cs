using FluentValidation;

namespace RevitaParceiros.Application.Features.Auth.RefreshToken;

public sealed class RefreshTokenValidator : AbstractValidator<RefreshTokenRequest>
{
    public RefreshTokenValidator()
    {
        RuleFor(x => x.AccessToken)
            .NotEmpty().WithMessage("O Access Token é obrigatório.");

        RuleFor(x => x.RefreshToken)
            .NotEmpty().WithMessage("O Refresh Token é obrigatório.");
    }
}
