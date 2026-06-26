namespace GQ.WebApi.Entities;

/// <summary>
/// Квартира в доме.
/// </summary>
public sealed class Apartment
{
    public Guid Id { get; private set; }

    public Guid BuildingId { get; private set; }

    public string Number { get; private set; } = string.Empty;

    public int? Floor { get; private set; }

    private Apartment()
    {
    }

    public static Apartment Create(Guid buildingId, string number, int? floor) =>
        Create(Guid.NewGuid(), buildingId, number, floor);

    public static Apartment Create(Guid id, Guid buildingId, string number, int? floor)
    {
        if (id == Guid.Empty)
        {
            throw new ArgumentException("Id is required.", nameof(id));
        }

        if (buildingId == Guid.Empty)
        {
            throw new ArgumentException("BuildingId is required.", nameof(buildingId));
        }

        if (string.IsNullOrWhiteSpace(number))
        {
            throw new ArgumentException("Number is required.", nameof(number));
        }

        return new Apartment
        {
            Id = id,
            BuildingId = buildingId,
            Number = number.Trim(),
            Floor = floor,
        };
    }
}
