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

    public DateOnly? MeterVerificationDate { get; private set; }

    private Apartment()
    {
    }

    public static Apartment Create(Guid buildingId, string number, int? floor, DateOnly? meterVerificationDate = null)
    {
        return Create(Guid.NewGuid(), buildingId, number, floor, meterVerificationDate);
    }

    public static Apartment Create(
        Guid id,
        Guid buildingId,
        string number,
        int? floor,
        DateOnly? meterVerificationDate = null)
    {
        if(id == Guid.Empty)
        {
            throw new ArgumentException("Id is required.", nameof(id));
        }

        if(buildingId == Guid.Empty)
        {
            throw new ArgumentException("BuildingId is required.", nameof(buildingId));
        }

        if(string.IsNullOrWhiteSpace(number))
        {
            throw new ArgumentException("Number is required.", nameof(number));
        }

        ValidateMeterVerificationDate(meterVerificationDate);

        return new Apartment
        {
            Id = id,
            BuildingId = buildingId,
            Number = number.Trim(),
            Floor = floor,
            MeterVerificationDate = meterVerificationDate
        };
    }

    public void Update(string number, int? floor, DateOnly? meterVerificationDate)
    {
        if(string.IsNullOrWhiteSpace(number))
        {
            throw new ArgumentException("Number is required.", nameof(number));
        }

        ValidateMeterVerificationDate(meterVerificationDate);

        Number = number.Trim();
        Floor = floor;
        MeterVerificationDate = meterVerificationDate;
    }

    private static void ValidateMeterVerificationDate(DateOnly? meterVerificationDate)
    {
        if(meterVerificationDate is not null
           && meterVerificationDate.Value > DateOnly.FromDateTime(DateTime.UtcNow))
        {
            throw new ArgumentException(
                "Meter verification date cannot be in the future.",
                nameof(meterVerificationDate));
        }
    }
}
