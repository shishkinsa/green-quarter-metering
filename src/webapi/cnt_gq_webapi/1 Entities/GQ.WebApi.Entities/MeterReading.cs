namespace GQ.WebApi.Entities;

/// <summary>
/// Показание электросчётчика по квартире за отчётный период.
/// </summary>
public sealed class MeterReading
{
    public Guid Id { get; private set; }

    public Guid ApartmentId { get; private set; }

    public int PeriodYear { get; private set; }

    public int PeriodMonth { get; private set; }

    public decimal Value { get; private set; }

    private MeterReading()
    {
    }

    public static MeterReading Create(Guid apartmentId, int periodYear, int periodMonth, decimal value)
    {
        return Create(Guid.NewGuid(), apartmentId, periodYear, periodMonth, value);
    }

    public static MeterReading Create(Guid id, Guid apartmentId, int periodYear, int periodMonth, decimal value)
    {
        if(id == Guid.Empty)
        {
            throw new ArgumentException("Id is required.", nameof(id));
        }

        if(apartmentId == Guid.Empty)
        {
            throw new ArgumentException("ApartmentId is required.", nameof(apartmentId));
        }

        ValidatePeriod(periodYear, periodMonth);
        ValidateValue(value);

        return new MeterReading
        {
            Id = id,
            ApartmentId = apartmentId,
            PeriodYear = periodYear,
            PeriodMonth = periodMonth,
            Value = value
        };
    }

    public void UpdateValue(decimal value)
    {
        ValidateValue(value);
        Value = value;
    }

    private static void ValidatePeriod(int periodYear, int periodMonth)
    {
        if(periodYear is < 2000 or > 2100)
        {
            throw new ArgumentException("PeriodYear is out of range.", nameof(periodYear));
        }

        if(periodMonth is < 1 or > 12)
        {
            throw new ArgumentException("PeriodMonth must be between 1 and 12.", nameof(periodMonth));
        }
    }

    private static void ValidateValue(decimal value)
    {
        if(value < 0)
        {
            throw new ArgumentException("Value cannot be negative.", nameof(value));
        }
    }
}
