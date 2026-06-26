using GQ.WebApi.DataAccess.Postgres.Data;
using GQ.WebApi.DataAccess.Postgres.Queries;
using GQ.WebApi.Entities;
using GQ.WebApi.Infrastructure.Interfaces.DataAccess;
using GQ.WebApi.Infrastructure.Interfaces.Queries;

using Microsoft.EntityFrameworkCore;

namespace GQ.WebApi.Tests.Unit.TestDoubles;

internal sealed class HandlerTestContext: IAsyncDisposable
{
    private readonly string databaseName = Guid.NewGuid().ToString();

    public AppDbContext Db { get; private set; } = null!;

    public IDbContext Context => Db;

    public IApartmentQueries ApartmentQueries { get; private set; } = null!;

    public IMeterReadingQueries MeterReadingQueries { get; private set; } = null!;

    public HandlerTestContext()
    {
        Reset();
    }

    public void Reset()
    {
        Db?.Dispose();
        DbContextOptions<AppDbContext> options = new DbContextOptionsBuilder<AppDbContext>()
            .UseInMemoryDatabase(databaseName)
            .Options;
        Db = new AppDbContext(options);
        ApartmentQueries = new ApartmentQueries(Db);
        MeterReadingQueries = new MeterReadingQueries(Db);
    }

    public async Task SeedDirectoryAsync()
    {
        Db.Buildings.Add(Building.Create(TestIds.BuildingId, "Корпус 1", "ул. Зелёная, 1"));
        Db.Apartments.Add(Apartment.Create(TestIds.Apartment1Id, TestIds.BuildingId, "12", 3));
        Db.Apartments.Add(Apartment.Create(TestIds.Apartment2Id, TestIds.BuildingId, "15", 3));
        Db.Owners.Add(Owner.Create(TestIds.Apartment1Id, "Иванов Иван Иванович", "+79001234567"));
        await Db.SaveChangesAsync();
    }

    public async Task SeedMeterReadingMay2026Async()
    {
        Db.MeterReadings.Add(
            MeterReading.Create(TestIds.Reading1Id, TestIds.Apartment1Id, 2026, 5, 1000m));
        await Db.SaveChangesAsync();
    }

    public async ValueTask DisposeAsync()
    {
        await Db.DisposeAsync();
    }
}
