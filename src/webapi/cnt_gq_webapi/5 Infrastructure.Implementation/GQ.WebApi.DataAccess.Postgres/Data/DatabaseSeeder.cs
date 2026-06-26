using GQ.WebApi.Entities;

namespace GQ.WebApi.DataAccess.Postgres.Data;

/// <summary>
/// Начальное наполнение справочников для InMemory и после миграций.
/// </summary>
public static class DatabaseSeeder
{
    public static void SeedCategories(AppDbContext dbContext)
    {
        if (dbContext.Categories.Any())
        {
            return;
        }

        dbContext.Categories.AddRange(
            Category.Create(
                Guid.Parse("a0000001-0000-0000-0000-000000000001"),
                "general",
                "Общее"),
            Category.Create(
                Guid.Parse("a0000001-0000-0000-0000-000000000002"),
                "demo",
                "Демо"),
            Category.Create(
                Guid.Parse("a0000001-0000-0000-0000-000000000003"),
                "template",
                "Шаблон"));

        dbContext.SaveChanges();
    }
}
