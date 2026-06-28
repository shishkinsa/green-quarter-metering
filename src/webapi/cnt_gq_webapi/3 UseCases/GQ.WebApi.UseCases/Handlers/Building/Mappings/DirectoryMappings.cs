using GQ.WebApi.Entities;
using GQ.WebApi.Infrastructure.Interfaces.Queries;
using GQ.WebApi.UseCases.Handlers.Building.Dto;

using BuildingEntity = GQ.WebApi.Entities.Building;
using OwnerEntity = GQ.WebApi.Entities.Owner;
using ApartmentEntity = GQ.WebApi.Entities.Apartment;

namespace GQ.WebApi.UseCases.Handlers.Building.Mappings;

internal static class DirectoryMappings
{
    internal static BuildingDto ToDto(BuildingEntity building)
    {
        return new(building.Id, building.Name, building.Address);
    }

    internal static ApartmentWithOwnerDto ToDto(ApartmentWithOwnerReadModel model)
    {
        return new(
            model.Id,
            model.BuildingId,
            model.Number,
            model.Floor,
            model.MeterVerificationDate,
            model.OwnerId,
            model.OwnerFullName,
            model.OwnerPhone,
            model.LastReadingSubmittedAt,
            model.LastReadingValue,
            model.CurrentPeriodSubmitted);
    }

    internal static ApartmentWithOwnerDto ToApartmentDto(
        ApartmentEntity apartment,
        OwnerEntity? owner = null,
        DateTimeOffset? lastReadingSubmittedAt = null,
        decimal? lastReadingValue = null,
        bool currentPeriodSubmitted = false)
    {
        return new(
            apartment.Id,
            apartment.BuildingId,
            apartment.Number,
            apartment.Floor,
            apartment.MeterVerificationDate,
            owner?.Id,
            owner?.FullName,
            owner?.Phone,
            lastReadingSubmittedAt,
            lastReadingValue,
            currentPeriodSubmitted);
    }

    internal static OwnerDto ToDto(OwnerEntity owner)
    {
        return new(owner.Id, owner.ApartmentId, owner.FullName, owner.Phone);
    }
}
