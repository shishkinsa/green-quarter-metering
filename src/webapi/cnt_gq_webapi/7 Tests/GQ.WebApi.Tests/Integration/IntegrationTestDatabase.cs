using GQ.WebApi.DataAccess.Postgres.Data;
using GQ.WebApi.Entities;
using Microsoft.EntityFrameworkCore;

namespace GQ.WebApi.Tests.Integration;

/// <summary>
/// Минимальные данные для интеграционных тестов (InMemory).
/// </summary>
internal static class IntegrationTestDatabase
{
    internal static void Seed(AppDbContext dbContext)
    {
        if (dbContext.Buildings.Any())
        {
            return;
        }

        var buildingId = Guid.Parse("b0000001-0000-0000-0000-000000000001");
        dbContext.Buildings.Add(Building.Create(buildingId, "Корпус 1", "ул. Зелёная, 1"));

        var apartment1Id = Guid.Parse("c0000001-0000-0000-0000-000000000001");
        dbContext.Apartments.Add(Apartment.Create(apartment1Id, buildingId, "12", 3));

        var apartment2Id = Guid.Parse("c0000001-0000-0000-0000-000000000002");
        dbContext.Apartments.Add(Apartment.Create(apartment2Id, buildingId, "15", 3));

        dbContext.Owners.Add(Owner.Create(apartment1Id, "Иванов Иван Иванович", "+79001234567"));

        dbContext.SaveChanges();
    }
}
