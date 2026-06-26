using Microsoft.AspNetCore.Mvc;
using MediatR;
using GQ.WebApi.UseCases.Handlers.Owner.Commands.UpsertApartmentOwner;

namespace GQ.WebApi.WebApp.Controllers;

/// <summary>
/// REST API операций с квартирами.
/// </summary>
[ApiController]
[Route("api/v1/apartments")]
public sealed class ApartmentsController(IMediator mediator) : ControllerBase
{
    [HttpPut("{apartmentId:guid}/owner")]
    [ProducesResponseType(typeof(UpsertApartmentOwnerResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public Task<UpsertApartmentOwnerResponse> UpsertOwner(
        Guid apartmentId,
        [FromBody] UpsertApartmentOwnerRequest request,
        CancellationToken cancellationToken) =>
        mediator.Send(
            new UpsertApartmentOwnerCommand(apartmentId, request.FullName, request.Phone),
            cancellationToken);
}

public sealed record UpsertApartmentOwnerRequest(string FullName, string? Phone);
