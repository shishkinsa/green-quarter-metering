using FluentValidation;
using FluentValidation.Results;

using GQ.WebApi.Entities;
using GQ.WebApi.Infrastructure.Interfaces.Repositories;
using GQ.WebApi.UseCases.Exceptions;
using GQ.WebApi.UseCases.Handlers.MeterReading.Mappings;

using MediatR;

using MeterReadingEntity = GQ.WebApi.Entities.MeterReading;

namespace GQ.WebApi.UseCases.Handlers.MeterReading.Commands.SubmitMeterReading;

/// <summary>
/// Передаёт или обновляет показание электросчётчика по квартире.
/// </summary>
/// <exception cref="UseCaseNotFoundException">Квартира не найдена.</exception>
/// <exception cref="ValidationException">Показание меньше предыдущего или понижение в том же месяце.</exception>
public sealed class SubmitMeterReadingCommandHandler(
    IApartmentRepository apartmentRepository,
    IMeterReadingRepository meterReadingRepository,
    IValidator<SubmitMeterReadingCommand> validator)
    : IRequestHandler<SubmitMeterReadingCommand, SubmitMeterReadingResponse>
{
    public async Task<SubmitMeterReadingResponse> Handle(
        SubmitMeterReadingCommand command,
        CancellationToken cancellationToken)
    {
        await validator.ValidateAndThrowAsync(command, cancellationToken);

        Entities.Apartment apartment = await apartmentRepository.GetByIdAsync(command.ApartmentId, cancellationToken) ?? throw new UseCaseNotFoundException($"Apartment '{command.ApartmentId}' was not found.");

        decimal? maxBefore = await meterReadingRepository.GetMaxValueBeforePeriodAsync(
            command.ApartmentId,
            command.PeriodYear,
            command.PeriodMonth,
            cancellationToken);

        if(maxBefore is not null && command.Value < maxBefore.Value)
        {
            throw new ValidationException(new[]
            {
                new ValidationFailure(
                    nameof(command.Value),
                    $"Reading value cannot be less than the previous reading ({maxBefore.Value}).")
            });
        }

        MeterReadingEntity? existing = await meterReadingRepository.GetByApartmentAndPeriodAsync(
            command.ApartmentId,
            command.PeriodYear,
            command.PeriodMonth,
            cancellationToken);

        if(existing is not null)
        {
            if(command.Value < existing.Value)
            {
                throw new ValidationException(new[]
                {
                    new ValidationFailure(
                        nameof(command.Value),
                        $"Reading value cannot be less than the value already submitted for this period ({existing.Value}).")
                });
            }

            existing.UpdateValue(command.Value);
            await meterReadingRepository.UpdateAsync(existing, cancellationToken);
            return new SubmitMeterReadingResponse(MeterReadingMappings.ToDto(existing), Created: false);
        }

        MeterReadingEntity created = MeterReadingEntity.Create(
            command.ApartmentId,
            command.PeriodYear,
            command.PeriodMonth,
            command.Value);
        await meterReadingRepository.AddAsync(created, cancellationToken);
        return new SubmitMeterReadingResponse(MeterReadingMappings.ToDto(created), Created: true);
    }
}
