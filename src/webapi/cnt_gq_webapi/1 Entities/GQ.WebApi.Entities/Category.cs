namespace GQ.WebApi.Entities;

/// <summary>
/// Справочная категория (read-only эталон второй сущности).
/// </summary>
public sealed class Category
{
    public Guid Id { get; private set; }

    public string Code { get; private set; } = string.Empty;

    public string Name { get; private set; } = string.Empty;

    private Category()
    {
    }

    public static Category Create(Guid id, string code, string name)
    {
        if (string.IsNullOrWhiteSpace(code))
        {
            throw new ArgumentException("Code is required.", nameof(code));
        }

        if (string.IsNullOrWhiteSpace(name))
        {
            throw new ArgumentException("Name is required.", nameof(name));
        }

        return new Category
        {
            Id = id,
            Code = code.Trim(),
            Name = name.Trim(),
        };
    }
}
