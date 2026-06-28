using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RevitaParceiros.Application.Features.Sales.RegisterSale;

namespace RevitaParceiros.API.Controllers;

/// <summary>
/// Controller responsável pelo gerenciamento de vendas (compras).
/// </summary>
public class SalesController(IServiceProvider provider) : ControllerBase<SalesController>(provider)
{
    /// <summary>
    /// Registra uma nova venda no sistema, calcula e atribui os pontos ao parceiro.
    /// Apenas Administrador e Funcionário podem acessar este endpoint.
    /// </summary>
    [HttpPost]
    [Authorize(Roles = "Administrador,Funcionario")]
    public async Task<IActionResult> RegisterSale([FromBody] RegisterSaleRequest request, CancellationToken cancellationToken)
    {
        var command = new RegisterSaleCommand(
            request.Amount,
            request.PartnerId,
            request.ClientId,
            UserId!.Value
        );

        var result = await Mediator.Send(command, cancellationToken);
        return CreatedAtAction(null, new { id = result.Id }, result);
    }
}

/// <summary>
/// DTO de entrada para o registro de venda.
/// </summary>
public class RegisterSaleRequest
{
    public decimal Amount { get; set; }
    public Guid PartnerId { get; set; }
    public Guid ClientId { get; set; }
}
