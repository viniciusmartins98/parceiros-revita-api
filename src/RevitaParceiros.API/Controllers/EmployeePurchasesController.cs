using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RevitaParceiros.Application.Features.EmployeePurchases.RegisterEmployeePurchase;
using RevitaParceiros.Application.Features.EmployeePurchases.ListEmployeePurchases;
using RevitaParceiros.Application.Features.EmployeePurchases.ListAllEmployeePurchases;
using RevitaParceiros.Application.Features.EmployeePurchases;

namespace RevitaParceiros.API.Controllers;

[Route("api/[controller]")]
[ApiController]
public class EmployeePurchasesController(IServiceProvider provider) : ControllerBase<EmployeePurchasesController>(provider)
{
    [HttpPost]
    [Authorize(Roles = "Administrador,Funcionario")]
    public async Task<IActionResult> Register([FromBody] RegisterEmployeePurchaseDto request, CancellationToken cancellationToken)
    {
        var command = new RegisterEmployeePurchaseCommand(
            request.Amount,
            request.Description,
            request.PurchaseDate,
            request.EmployeeId,
            UserContext!.UserId
        );

        var result = await Mediator.Send(command, cancellationToken);
        return CreatedAtAction(null, new { id = result.Id }, result);
    }

    [HttpGet]
    [Authorize(Roles = "Administrador")]
    public async Task<IActionResult> GetAll(CancellationToken cancellationToken)
    {
        var result = await Mediator.Send(new ListAllEmployeePurchasesRequest(), cancellationToken);
        return Ok(result);
    }

    [HttpGet("my")]
    [Authorize(Roles = "Funcionario")]
    public async Task<IActionResult> GetMyPurchases(CancellationToken cancellationToken)
    {
        if (UserContext!.EmployeeId == null)
        {
            return Forbid();
        }
        var result = await Mediator.Send(new ListEmployeePurchasesRequest(UserContext.EmployeeId.Value), cancellationToken);
        return Ok(result);
    }

    [HttpGet("employee/{id:guid}")]
    [Authorize(Roles = "Administrador")]
    public async Task<IActionResult> GetByEmployee(Guid employeeId, CancellationToken cancellationToken)
    {
        var result = await Mediator.Send(new ListEmployeePurchasesRequest(employeeId), cancellationToken);
        return Ok(result);
    }
}
