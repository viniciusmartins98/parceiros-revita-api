using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RevitaParceiros.Application.Features.Clients.CreateClient;
using RevitaParceiros.Application.Features.Clients.DeleteClient;
using RevitaParceiros.Application.Features.Clients.GetClientById;
using RevitaParceiros.Application.Features.Clients.ListClients;
using RevitaParceiros.Application.Features.Clients.UpdateClient;
using RevitaParceiros.Domain.Enums;
using System.Security.Claims;

namespace RevitaParceiros.API.Controllers;

public class ClientsController(IServiceProvider provider) : ControllerBase<ClientsController>(provider)
{
    [HttpGet]
    [Authorize(Roles = "Administrador,Funcionario")]
    public async Task<IActionResult> GetAll(CancellationToken cancellationToken)
    {
        var result = await Mediator.Send(new ListClientsRequest(), cancellationToken);
        return Ok(result);
    }

    [HttpGet("{id:guid}")]
    [Authorize(Roles = "Administrador,Funcionario")]
    public async Task<IActionResult> GetById([FromRoute] Guid id, CancellationToken cancellationToken)
    {
        var result = await Mediator.Send(new GetClientByIdRequest(id), cancellationToken);
        return Ok(result);
    }

    [HttpPost]
    [Authorize(Roles = "Administrador,Funcionario")]
    public async Task<IActionResult> Create([FromBody] CreateClientRequest request, CancellationToken cancellationToken)
    {
        var result = await Mediator.Send(request, cancellationToken);
        return CreatedAtAction(nameof(GetById), new { id = result.Id }, result);
    }

    [HttpPut("{id:guid}")]
    [Authorize(Roles = "Administrador,Funcionario")]
    public async Task<IActionResult> Update([FromRoute] Guid id, [FromBody] UpdateClientRequest request, CancellationToken cancellationToken)
    {
        var requestWithId = request with { Id = id };
        var result = await Mediator.Send(requestWithId, cancellationToken);
        return Ok(result);
    }

    [HttpDelete("{id:guid}")]
    [Authorize(Roles = "Administrador")]
    public async Task<IActionResult> Delete([FromRoute] Guid id, CancellationToken cancellationToken)
    {
        await Mediator.Send(new DeleteClientRequest(id), cancellationToken);
        return NoContent();
    }
}
