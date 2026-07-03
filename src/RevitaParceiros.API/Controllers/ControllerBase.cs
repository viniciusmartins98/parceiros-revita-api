using Mediator;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RevitaParceiros.API.AuthContext;
using System.Security.Claims;

namespace RevitaParceiros.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class ControllerBase<T>(IServiceProvider serviceProvider) : ControllerBase
    {
        private readonly IServiceProvider _serviceProvider = serviceProvider;

        private ILogger<T> _logger;
        protected ILogger<T> Logger => _logger ??= _serviceProvider.GetRequiredService<ILogger<T>>();

        private IMediator _mediator;
        protected IMediator Mediator => _mediator ??= _serviceProvider.GetRequiredService<IMediator>();
        protected UserContext? UserContext => _userId != null ? new UserContext
        {
            UserId = new Guid(_userId),
            ClientId = _clientId != null ? new Guid(_clientId) : null,
            EmployeeId = _employeeId != null ? new Guid(_employeeId) : null,
            PartnerId = _partnerId != null ? new Guid(_partnerId) : null,
        } : null;
        private string? _userId => User.FindFirstValue(ClaimTypes.NameIdentifier);
        private string? _employeeId => User.FindFirstValue("EmployeeId");
        private string? _partnerId => User.FindFirstValue("PartnerId");
        private string? _clientId => User.FindFirstValue("ClientId");
    }
}
