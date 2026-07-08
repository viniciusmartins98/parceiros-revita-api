using FluentValidation;

namespace RevitaParceiros.Application.Features.Sales.RegisterSale;

/// <summary>
/// Validador para o comando de registro de venda.
/// </summary>
public sealed class RegisterSaleValidator : AbstractValidator<RegisterSaleCommand>
{
    public RegisterSaleValidator()
    {
        RuleFor(x => x.Amount)
            .GreaterThan(0)
            .WithMessage("O valor da venda deve ser maior que zero.");

        RuleFor(x => x.ClientId)
            .NotEmpty()
            .WithMessage("O cliente é obrigatório.");

        RuleFor(x => x.RegisteredByUserId)
            .NotEmpty()
            .WithMessage("O usuário que registrou a venda é obrigatório.");
    }
}
