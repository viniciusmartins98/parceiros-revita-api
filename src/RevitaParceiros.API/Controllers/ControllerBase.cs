using Mediator;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

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
    }
}
