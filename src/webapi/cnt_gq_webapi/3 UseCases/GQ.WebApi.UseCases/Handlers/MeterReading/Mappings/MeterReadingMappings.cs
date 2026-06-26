using GQ.WebApi.Entities;
using GQ.WebApi.Infrastructure.Interfaces.Repositories;
using GQ.WebApi.UseCases.Handlers.MeterReading.Dto;

using MeterReadingEntity = GQ.WebApi.Entities.MeterReading;

namespace GQ.WebApi.UseCases.Handlers.MeterReading.Mappings;

internal static class MeterReadingMappings
{
    internal static MeterReadingDto ToDto(MeterReadingEntity reading)
    {
        return new(
            reading.Id,
            reading.ApartmentId,
            reading.PeriodYear,
            reading.PeriodMonth,
            reading.Value);
    }

    internal static BuildingMeterReadingStatusDto ToDto(BuildingMeterReadingStatusReadModel model)
    {
        return new(
            model.ApartmentId,
            model.ApartmentNumber,
            model.OwnerFullName,
            model.ReadingValue,
            model.Submitted);
    }
}
