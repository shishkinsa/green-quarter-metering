using GQ.WebApi.Entities;
using GQ.WebApi.Infrastructure.Interfaces.Repositories;
using GQ.WebApi.UseCases.Handlers.Building.Dto;
using BuildingEntity = GQ.WebApi.Entities.Building;
using OwnerEntity = GQ.WebApi.Entities.Owner;

namespace GQ.WebApi.UseCases.Handlers.Building.Mappings;

internal static class DirectoryMappings
{
    internal static BuildingDto ToDto(BuildingEntity building) =>
        new(building.Id, building.Name, building.Address);

    internal static ApartmentWithOwnerDto ToDto(ApartmentWithOwnerReadModel model) =>
        new(
            model.Id,
            model.BuildingId,
            model.Number,
            model.Floor,
            model.OwnerId,
            model.OwnerFullName,
            model.OwnerPhone);

    internal static OwnerDto ToDto(OwnerEntity owner) =>
        new(owner.Id, owner.ApartmentId, owner.FullName, owner.Phone);
}
