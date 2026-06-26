using GQ.WebApi.Entities;

namespace GQ.WebApi.Infrastructure.Interfaces.Repositories;

public sealed record ApartmentWithOwnerReadModel(
    Guid Id,
    Guid BuildingId,
    string Number,
    int? Floor,
    Guid? OwnerId,
    string? OwnerFullName,
    string? OwnerPhone);

public interface IApartmentRepository
{
    Task<IReadOnlyList<ApartmentWithOwnerReadModel>> ListByBuildingWithOwnersAsync(
        Guid buildingId,
        CancellationToken cancellationToken);

    Task<Apartment?> GetByIdAsync(Guid id, CancellationToken cancellationToken);

    Task<bool> ExistsByBuildingAndNumberAsync(
        Guid buildingId,
        string number,
        Guid? excludeApartmentId,
        CancellationToken cancellationToken);

    Task AddAsync(Apartment apartment, CancellationToken cancellationToken);
}
