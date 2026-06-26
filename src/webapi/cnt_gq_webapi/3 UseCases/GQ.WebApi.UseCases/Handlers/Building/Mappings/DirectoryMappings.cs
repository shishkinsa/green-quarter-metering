using GQ.WebApi.Entities;
using GQ.WebApi.Infrastructure.Interfaces.Repositories;
using GQ.WebApi.UseCases.Handlers.Building.Dto;

using BuildingEntity = GQ.WebApi.Entities.Building;
using OwnerEntity = GQ.WebApi.Entities.Owner;

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
            model.OwnerId,
            model.OwnerFullName,
            model.OwnerPhone,
            model.LastReadingSubmittedAt,
            model.LastReadingValue,
            model.CurrentPeriodSubmitted);
    }

    internal static OwnerDto ToDto(OwnerEntity owner)
    {
        return new(owner.Id, owner.ApartmentId, owner.FullName, owner.Phone);
    }
}
