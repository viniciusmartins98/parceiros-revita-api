using Mediator;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RevitaParceiros.Application.Features.Partners.CreatePartner;
using RevitaParceiros.Application.Features.Partners.DeletePartner;
using RevitaParceiros.Application.Features.Partners.GetPartnerById;
using RevitaParceiros.Application.Features.Partners.ListPartners;
using RevitaParceiros.Application.Features.Partners.UpdatePartner;

namespace RevitaParceiros.API.Controllers;

[Authorize(Roles = "Administrador")]
public class PartnersController(IServiceProvider provider) : ControllerBase<PartnersController>(provider)
{
    [HttpGet]
    public async Task<IActionResult> GetAll(CancellationToken cancellationToken)
    {
        var result = await Mediator.Send(new ListPartnersRequest(), cancellationToken);
        return Ok(result);
    }

    [HttpGet("{id:guid}")]
    public async Task<IActionResult> GetById([FromRoute] Guid id, CancellationToken cancellationToken)
    {
        var result = await Mediator.Send(new GetPartnerByIdRequest(id), cancellationToken);
        return Ok(result);
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] CreatePartnerRequest request, CancellationToken cancellationToken)
    {
        var result = await Mediator.Send(request, cancellationToken);
        return CreatedAtAction(nameof(GetById), new { id = result.Id }, result);
    }

    [HttpPut("{id:guid}")]
    public async Task<IActionResult> Update([FromRoute] Guid id, [FromBody] UpdatePartnerRequest request, CancellationToken cancellationToken)
    {
        var requestWithId = request with { Id = id };
        var result = await Mediator.Send(requestWithId, cancellationToken);
        return Ok(result);
    }

    [HttpDelete("{id:guid}")]
    public async Task<IActionResult> Delete([FromRoute] Guid id, CancellationToken cancellationToken)
    {
        await Mediator.Send(new DeletePartnerRequest(id), cancellationToken);
        return NoContent();
    }
}
