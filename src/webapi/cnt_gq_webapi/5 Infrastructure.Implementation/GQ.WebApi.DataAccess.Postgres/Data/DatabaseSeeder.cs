using GQ.WebApi.Entities;

namespace GQ.WebApi.DataAccess.Postgres.Data;

/// <summary>
/// Начальное наполнение справочников для InMemory и после миграций.
/// </summary>
public static class DatabaseSeeder
{
    public static void SeedDirectories(AppDbContext dbContext)
    {
        if (dbContext.Buildings.Any())
        {
            return;
        }

        var buildingId = Guid.Parse("b0000001-0000-0000-0000-000000000001");
        var building = Building.Create(buildingId, "Корпус 1", "ул. Зелёная, 1");
        dbContext.Buildings.Add(building);

        var apartment1Id = Guid.Parse("c0000001-0000-0000-0000-000000000001");
        dbContext.Apartments.Add(Apartment.Create(apartment1Id, buildingId, "12", 3));

        var apartment2Id = Guid.Parse("c0000001-0000-0000-0000-000000000002");
        dbContext.Apartments.Add(Apartment.Create(apartment2Id, buildingId, "15", 3));

        dbContext.Owners.Add(Owner.Create(apartment1Id, "Иванов Иван Иванович", "+79001234567"));

        dbContext.SaveChanges();
    }
}
