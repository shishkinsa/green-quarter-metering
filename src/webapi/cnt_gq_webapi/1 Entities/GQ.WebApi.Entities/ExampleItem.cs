namespace GQ.WebApi.Entities;

/// <summary>
/// Пример доменной сущности. Замените на модель вашего домена.
/// </summary>
public sealed class ExampleItem
{
    public Guid Id { get; private set; }

    public string Name { get; private set; } = string.Empty;

    public DateTimeOffset CreatedAt { get; private set; }

    private ExampleItem()
    {
    }

    public static ExampleItem Create(string name)
    {
        if (string.IsNullOrWhiteSpace(name))
        {
            throw new ArgumentException("Name is required.", nameof(name));
        }

        return new ExampleItem
        {
            Id = Guid.NewGuid(),
            Name = name.Trim(),
            CreatedAt = DateTimeOffset.UtcNow,
        };
    }

    /// <summary>
    /// Переименовывает сущность.
    /// </summary>
    public void Rename(string name)
    {
        if (string.IsNullOrWhiteSpace(name))
        {
            throw new ArgumentException("Name is required.", nameof(name));
        }

        Name = name.Trim();
    }
}
