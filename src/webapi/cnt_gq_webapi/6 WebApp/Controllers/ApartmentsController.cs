using GQ.WebApi.UseCases.Handlers.MeterReading.Commands.SubmitMeterReading;
using GQ.WebApi.UseCases.Handlers.Owner.Commands.UpsertApartmentOwner;

using MediatR;

using Microsoft.AspNetCore.Mvc;

namespace GQ.WebApi.WebApp.Controllers;

/// <summary>
/// REST API операций с квартирами.
/// </summary>
[ApiController]
[Route("api/v1/apartments")]
public sealed class ApartmentsController(IMediator mediator): ControllerBase
{
    [HttpPut("{apartmentId:guid}/owner")]
    [ProducesResponseType(typeof(UpsertApartmentOwnerResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public Task<UpsertApartmentOwnerResponse> UpsertOwner(
        Guid apartmentId,
        [FromBody] UpsertApartmentOwnerRequest request,
        CancellationToken cancellationToken)
    {
        return mediator.Send(
            new UpsertApartmentOwnerCommand(apartmentId, request.FullName, request.Phone),
            cancellationToken);
    }

    [HttpPost("{apartmentId:guid}/meter-readings")]
    [ProducesResponseType(typeof(SubmitMeterReadingResponse), StatusCodes.Status201Created)]
    [ProducesResponseType(typeof(SubmitMeterReadingResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<SubmitMeterReadingResponse>> SubmitMeterReading(
        Guid apartmentId,
        [FromBody] SubmitMeterReadingRequest request,
        CancellationToken cancellationToken)
    {
        SubmitMeterReadingResponse response = await mediator.Send(
            new SubmitMeterReadingCommand(apartmentId, request.PeriodYear, request.PeriodMonth, request.Value),
            cancellationToken);

        if(response.Created)
        {
            return CreatedAtAction(nameof(SubmitMeterReading), new { apartmentId }, response);
        }

        return Ok(response);
    }
}

/// <summary>Тело запроса назначения или обновления владельца квартиры.</summary>
public sealed record UpsertApartmentOwnerRequest(string FullName, string? Phone);

/// <summary>Тело запроса передачи показания.</summary>
public sealed record SubmitMeterReadingRequest(int PeriodYear, int PeriodMonth, decimal Value);
