using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RevitaParceiros.Application.Features.Auth.Login;
using RevitaParceiros.Application.Features.Auth.Logout;
using RevitaParceiros.Application.Features.Auth.RefreshToken;
using RevitaParceiros.Application.Features.Auth.RegistrarCliente;
using RevitaParceiros.Application.Features.Auth.RegistrarParceiro;

namespace RevitaParceiros.API.Controllers;

[Route("api/auth")]
[ApiController]
public class AuthController(IServiceProvider serviceProvider) : ControllerBase<AuthController>(serviceProvider)
{
    [HttpPost("login")]
    [AllowAnonymous]
    [ProducesResponseType(typeof(LoginResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> Login([FromBody] LoginRequest request)
    {
        var response = await Mediator.Send(request);
        return Ok(response);
    }

    [HttpPost("refresh")]
    [AllowAnonymous]
    [ProducesResponseType(typeof(LoginResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status401Unauthorized)]
    public async Task<IActionResult> RefreshToken([FromBody] RefreshTokenRequest request)
    {
        var response = await Mediator.Send(request);
        return Ok(response);
    }

    [HttpPost("logout")]
    [Authorize]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status401Unauthorized)]
    public async Task<IActionResult> Logout([FromBody] LogoutRequest request)
    {
        await Mediator.Send(request);
        return NoContent();
    }

    [HttpPost("register/client")]
    [AllowAnonymous]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status409Conflict)]
    public async Task<IActionResult> RegisterClient([FromBody] RegistrarClienteRequest request)
    {
        await Mediator.Send(request);
        return Ok();
    }

    [HttpPost("register/partner")]
    [AllowAnonymous]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status409Conflict)]
    public async Task<IActionResult> RegisterPartner([FromBody] RegistrarParceiroRequest request)
    {
        await Mediator.Send(request);
        return Ok();
    }
}
