namespace GQ.WebApi.Entities;

/// <summary>
/// Владелец квартиры.
/// </summary>
public sealed class Owner
{
    public Guid Id { get; private set; }

    public Guid ApartmentId { get; private set; }

    public string FullName { get; private set; } = string.Empty;

    public string? Phone { get; private set; }

    private Owner()
    {
    }

    public static Owner Create(Guid apartmentId, string fullName, string? phone)
    {
        if (apartmentId == Guid.Empty)
        {
            throw new ArgumentException("ApartmentId is required.", nameof(apartmentId));
        }

        if (string.IsNullOrWhiteSpace(fullName))
        {
            throw new ArgumentException("FullName is required.", nameof(fullName));
        }

        return new Owner
        {
            Id = Guid.NewGuid(),
            ApartmentId = apartmentId,
            FullName = fullName.Trim(),
            Phone = string.IsNullOrWhiteSpace(phone) ? null : phone.Trim(),
        };
    }

    public void Update(string fullName, string? phone)
    {
        if (string.IsNullOrWhiteSpace(fullName))
        {
            throw new ArgumentException("FullName is required.", nameof(fullName));
        }

        FullName = fullName.Trim();
        Phone = string.IsNullOrWhiteSpace(phone) ? null : phone.Trim();
    }
}
