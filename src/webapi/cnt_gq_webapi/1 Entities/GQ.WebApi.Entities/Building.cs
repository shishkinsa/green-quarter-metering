namespace GQ.WebApi.Entities;

/// <summary>
/// Дом (корпус) жилого комплекса.
/// </summary>
public sealed class Building
{
    public Guid Id { get; private set; }

    public string Name { get; private set; } = string.Empty;

    public string? Address { get; private set; }

    private Building()
    {
    }

    public static Building Create(string name, string? address)
    {
        return Create(Guid.NewGuid(), name, address);
    }

    public static Building Create(Guid id, string name, string? address)
    {
        if (id == Guid.Empty)
        {
            throw new ArgumentException("Id is required.", nameof(id));
        }

        if (string.IsNullOrWhiteSpace(name))
        {
            throw new ArgumentException("Name is required.", nameof(name));
        }

        return new Building
        {
            Id = id,
            Name = name.Trim(),
            Address = string.IsNullOrWhiteSpace(address) ? null : address.Trim(),
        };
    }

    public void Update(string name, string? address)
    {
        if (string.IsNullOrWhiteSpace(name))
        {
            throw new ArgumentException("Name is required.", nameof(name));
        }

        Name = name.Trim();
        Address = string.IsNullOrWhiteSpace(address) ? null : address.Trim();
    }
}
