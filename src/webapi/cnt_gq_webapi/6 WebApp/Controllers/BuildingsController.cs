using GQ.WebApi.UseCases.Handlers.Apartment.Commands.CreateApartment;
using GQ.WebApi.UseCases.Handlers.Apartment.Queries.ListApartmentsWithOwners;
using GQ.WebApi.UseCases.Handlers.Building.Commands.CreateBuilding;
using GQ.WebApi.UseCases.Handlers.Building.Commands.UpdateBuilding;
using GQ.WebApi.UseCases.Handlers.Building.Queries.ListBuildings;

using MediatR;

using Microsoft.AspNetCore.Mvc;

namespace GQ.WebApi.WebApp.Controllers;

/// <summary>
/// REST API справочника домов и квартир.
/// </summary>
[ApiController]
[Route("api/v1/buildings")]
public sealed class BuildingsController(IMediator mediator): ControllerBase
{
    [HttpGet]
    [ProducesResponseType(typeof(ListBuildingsResponse), StatusCodes.Status200OK)]
    public Task<ListBuildingsResponse> List(CancellationToken cancellationToken)
    {
        return mediator.Send(new ListBuildingsQuery(), cancellationToken);
    }

    [HttpPost]
    [ProducesResponseType(typeof(CreateBuildingResponse), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<CreateBuildingResponse>> Create(
        [FromBody] CreateBuildingRequest request,
        CancellationToken cancellationToken)
    {
        CreateBuildingResponse response = await mediator.Send(
            new CreateBuildingCommand(request.Name, request.Address),
            cancellationToken);
        return CreatedAtAction(nameof(List), response);
    }

    [HttpPut("{id:guid}")]
    [ProducesResponseType(typeof(UpdateBuildingResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public Task<UpdateBuildingResponse> Update(
        Guid id,
        [FromBody] UpdateBuildingRequest request,
        CancellationToken cancellationToken)
    {
        return mediator.Send(new UpdateBuildingCommand(id, request.Name, request.Address), cancellationToken);
    }

    [HttpGet("{buildingId:guid}/apartments")]
    [ProducesResponseType(typeof(ListApartmentsWithOwnersResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public Task<ListApartmentsWithOwnersResponse> ListApartments(
        Guid buildingId,
        CancellationToken cancellationToken)
    {
        return mediator.Send(new ListApartmentsWithOwnersQuery(buildingId), cancellationToken);
    }

    [HttpPost("{buildingId:guid}/apartments")]
    [ProducesResponseType(typeof(CreateApartmentResponse), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status409Conflict)]
    public async Task<ActionResult<CreateApartmentResponse>> CreateApartment(
        Guid buildingId,
        [FromBody] CreateApartmentRequest request,
        CancellationToken cancellationToken)
    {
        CreateApartmentResponse response = await mediator.Send(
            new CreateApartmentCommand(buildingId, request.Number, request.Floor),
            cancellationToken);
        return CreatedAtAction(
            nameof(ListApartments),
            new { buildingId },
            response);
    }
}

/// <summary>Тело запроса создания дома.</summary>
public sealed record CreateBuildingRequest(string Name, string? Address);

/// <summary>Тело запроса обновления дома.</summary>
public sealed record UpdateBuildingRequest(string Name, string? Address);

/// <summary>Тело запроса создания квартиры.</summary>
public sealed record CreateApartmentRequest(string Number, int? Floor);
