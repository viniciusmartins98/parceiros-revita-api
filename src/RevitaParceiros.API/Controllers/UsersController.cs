using Mediator;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RevitaParceiros.Application.Features.Users.CreateUser;
using RevitaParceiros.Application.Features.Users.DeleteUser;
using RevitaParceiros.Application.Features.Users.GetUserById;
using RevitaParceiros.Application.Features.Users.GetUsers;
using RevitaParceiros.Application.Features.Users.UpdateUser;

namespace RevitaParceiros.API.Controllers;

[Authorize(Roles = "Administrador")]
public class UsersController(IServiceProvider provider) : ControllerBase<UsersController>(provider)
{
    [HttpGet]
    public async Task<IActionResult> GetAll(CancellationToken cancellationToken)
    {
        var result = await Mediator.Send(new GetUsersQuery(), cancellationToken);
        return Ok(result);
    }

    [HttpGet("{id:guid}")]
    public async Task<IActionResult> GetById([FromRoute] Guid id, CancellationToken cancellationToken)
    {
        var result = await Mediator.Send(new GetUserByIdQuery(id), cancellationToken);
        return Ok(result);
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] CreateUserCommand request, CancellationToken cancellationToken)
    {
        var result = await Mediator.Send(request, cancellationToken);
        return CreatedAtAction(nameof(GetById), new { id = result.Id }, result);
    }

    [HttpPut("{id:guid}")]
    public async Task<IActionResult> Update([FromRoute] Guid id, [FromBody] UpdateUserCommand request, CancellationToken cancellationToken)
    {
        var requestWithId = request with { Id = id };
        var result = await Mediator.Send(requestWithId, cancellationToken);
        return Ok(result);
    }

    [HttpDelete("{id:guid}")]
    public async Task<IActionResult> Delete([FromRoute] Guid id, CancellationToken cancellationToken)
    {
        await Mediator.Send(new DeleteUserCommand(id), cancellationToken);
        return NoContent();
    }
}
