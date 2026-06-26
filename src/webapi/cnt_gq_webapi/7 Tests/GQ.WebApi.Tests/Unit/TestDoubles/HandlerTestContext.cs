using GQ.WebApi.Entities;

namespace GQ.WebApi.Tests.Unit.TestDoubles;

internal sealed class HandlerTestContext
{
    public FakeBuildingRepository Buildings { get; } = new();

    public FakeOwnerRepository Owners { get; } = new();

    public FakeApartmentRepository Apartments { get; private set; } = null!;

    public FakeMeterReadingRepository MeterReadings { get; private set; } = null!;

    public HandlerTestContext()
    {
        Reset();
    }

    public void Reset()
    {
        Buildings.Items.Clear();
        Owners.Items.Clear();
        Apartments = new FakeApartmentRepository(Owners);
        MeterReadings = new FakeMeterReadingRepository(Apartments, Owners);
        Apartments.MeterReadings = MeterReadings;
    }

    public void SeedDirectory()
    {
        Buildings.Items.Add(Building.Create(TestIds.BuildingId, "Корпус 1", "ул. Зелёная, 1"));
        Apartments.Items.Add(Apartment.Create(TestIds.Apartment1Id, TestIds.BuildingId, "12", 3));
        Apartments.Items.Add(Apartment.Create(TestIds.Apartment2Id, TestIds.BuildingId, "15", 3));
        Owners.Items.Add(Owner.Create(TestIds.Apartment1Id, "Иванов Иван Иванович", "+79001234567"));
    }

    public void SeedMeterReadingMay2026()
    {
        MeterReadings.Items.Add(
            MeterReading.Create(TestIds.Reading1Id, TestIds.Apartment1Id, 2026, 5, 1000m));
    }
}
