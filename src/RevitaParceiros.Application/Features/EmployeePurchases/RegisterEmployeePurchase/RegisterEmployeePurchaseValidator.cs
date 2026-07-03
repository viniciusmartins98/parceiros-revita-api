using FluentValidation;

namespace RevitaParceiros.Application.Features.EmployeePurchases.RegisterEmployeePurchase;

public sealed class RegisterEmployeePurchaseValidator : AbstractValidator<RegisterEmployeePurchaseCommand>
{
    public RegisterEmployeePurchaseValidator()
    {
        RuleFor(x => x.Amount)
            .GreaterThan(0)
            .WithMessage("O valor da compra deve ser maior que zero.");

        RuleFor(x => x.EmployeeId)
            .NotEmpty()
            .WithMessage("O identificador do funcionário é obrigatório.");

        RuleFor(x => x.RegisteredByUserId)
            .NotEmpty()
            .WithMessage("O identificador de quem registrou a compra é obrigatório.");

        RuleFor(x => x.PurchaseDate)
            .NotEmpty()
            .WithMessage("A data da compra é obrigatória.");
    }
}
