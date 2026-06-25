using Mediator;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RevitaParceiros.Application.Features.Employees.CreateEmployee;
using RevitaParceiros.Application.Features.Employees.DeleteEmployee;
using RevitaParceiros.Application.Features.Employees.GetEmployeeById;
using RevitaParceiros.Application.Features.Employees.ListEmployees;
using RevitaParceiros.Application.Features.Employees.UpdateEmployee;

namespace RevitaParceiros.API.Controllers;

[Authorize(Roles = "Administrador")]
public class EmployeesController(IServiceProvider provider) : ControllerBase<EmployeesController>(provider)
{
    [HttpGet]
    public async Task<IActionResult> GetAll(CancellationToken cancellationToken)
    {
        var result = await Mediator.Send(new ListEmployeesRequest(), cancellationToken);
        return Ok(result);
    }

    [HttpGet("{id:guid}")]
    public async Task<IActionResult> GetById([FromRoute] Guid id, CancellationToken cancellationToken)
    {
        var result = await Mediator.Send(new GetEmployeeByIdRequest(id), cancellationToken);
        return Ok(result);
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] CreateEmployeeRequest request, CancellationToken cancellationToken)
    {
        var result = await Mediator.Send(request, cancellationToken);
        return CreatedAtAction(nameof(GetById), new { id = result.Id }, result);
    }

    [HttpPut("{id:guid}")]
    public async Task<IActionResult> Update([FromRoute] Guid id, [FromBody] UpdateEmployeeRequest request, CancellationToken cancellationToken)
    {
        var requestWithId = request with { Id = id };
        var result = await Mediator.Send(requestWithId, cancellationToken);
        return Ok(result);
    }

    [HttpDelete("{id:guid}")]
    public async Task<IActionResult> Delete([FromRoute] Guid id, CancellationToken cancellationToken)
    {
        await Mediator.Send(new DeleteEmployeeRequest(id), cancellationToken);
        return NoContent();
    }
}
