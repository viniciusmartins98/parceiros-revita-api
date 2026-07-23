using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RevitaParceiros.Application.Features.EmployeePurchases.RegisterEmployeePurchase;
using RevitaParceiros.Application.Features.EmployeePurchases.ListEmployeePurchases;
using RevitaParceiros.Application.Features.EmployeePurchases.ListAllEmployeePurchases;
using RevitaParceiros.Application.Features.EmployeePurchases.DeleteEmployeePurchase;
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
    public async Task<IActionResult> GetAll(
        [FromQuery] string? period = null,
        [FromQuery] DateTime? startDate = null,
        [FromQuery] DateTime? endDate = null,
        CancellationToken cancellationToken = default)
    {
        var result = await Mediator.Send(new ListAllEmployeePurchasesRequest(period, startDate, endDate), cancellationToken);
        return Ok(result);
    }

    [HttpGet("my")]
    [Authorize(Roles = "Funcionario")]
    public async Task<IActionResult> GetMyPurchases(
        [FromQuery] string? period = null,
        [FromQuery] DateTime? startDate = null,
        [FromQuery] DateTime? endDate = null,
        CancellationToken cancellationToken = default)
    {
        if (UserContext!.EmployeeId == null)
        {
            return Forbid();
        }
        var result = await Mediator.Send(new ListEmployeePurchasesRequest(UserContext.EmployeeId.Value, period, startDate, endDate), cancellationToken);
        return Ok(result);
    }

    [HttpGet("employee/{id:guid}")]
    [Authorize(Roles = "Administrador")]
    public async Task<IActionResult> GetByEmployee(
        Guid id,
        [FromQuery] string? period = null,
        [FromQuery] DateTime? startDate = null,
        [FromQuery] DateTime? endDate = null,
        CancellationToken cancellationToken = default)
    {
        var result = await Mediator.Send(new ListEmployeePurchasesRequest(id, period, startDate, endDate), cancellationToken);
        return Ok(result);
    }

    [HttpDelete("{id:guid}")]
    [Authorize(Roles = "Administrador")]
    public async Task<IActionResult> Delete(Guid id, CancellationToken cancellationToken)
    {
        await Mediator.Send(new DeleteEmployeePurchaseCommand(id), cancellationToken);
        return NoContent();
    }
}
