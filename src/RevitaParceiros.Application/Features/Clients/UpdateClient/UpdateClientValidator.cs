using FluentValidation;

namespace RevitaParceiros.Application.Features.Clients.UpdateClient;

public class UpdateClientValidator : AbstractValidator<UpdateClientRequest>
{
    public UpdateClientValidator()
    {
        RuleFor(v => v.Name)
            .NotEmpty().WithMessage("O nome é obrigatório.")
            .MaximumLength(200).WithMessage("O nome deve ter no máximo 200 caracteres.");

        RuleFor(v => v.Email)
            .NotEmpty().WithMessage("O e-mail é obrigatório.")
            .EmailAddress().WithMessage("O e-mail deve ser válido.")
            .MaximumLength(150).WithMessage("O e-mail deve ter no máximo 150 caracteres.");

        RuleFor(v => v.Phone)
            .NotEmpty().WithMessage("O telefone é obrigatório.")
            .MaximumLength(20).WithMessage("O telefone deve ter no máximo 20 caracteres.");
    }
}
