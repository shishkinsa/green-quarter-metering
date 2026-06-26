# Directories — Technical Design

Справочник домов, квартир и владельцев ЖК «Зелёный квартал».

## Backend

- **Entities**: `Building`, `Apartment`, `Owner` в `1 Entities/`
- **Data access**: `IDbContext` в handlers (CRUD, один `SaveChangesAsync` на сценарий); `IApartmentQueries` — `ListByBuildingWithOwnersAsync`
- **UseCases**: `Handlers/Building/`, `Handlers/Apartment/`, `Handlers/Owner/`
- **API**: `BuildingsController`, `ApartmentsController`
- **Persistence**: таблицы `buildings`, `apartments`, `owners`; миграции `AddBuildingsApartmentsOwners`, `SeedDirectoryDemoData`

CRUD (`Create`/`Update`/`Delete` домов и квартир, upsert владельца) — через `IDbContext` в handlers. Сложная выборка квартир с владельцами и статусом показаний — `IApartmentQueries.ListByBuildingWithOwnersAsync`.

## Frontend (FSD)

- `entities/building/model/types.ts`, `entities/building/api/buildingApi.ts`
- `entities/building/ui/ApartmentsTable.vue`
- `features/directory/create-building/` — форма добавления дома
- `pages/directories/` — выбор дома и таблица квартир/владельцев

## Tests

- Integration: `BuildingsEndpointTests`, `ApartmentsEndpointTests`
- Покрытие API-сценариев: [scenario-coverage.txt](scenario-coverage.txt)

## Scenario coverage

| Spec scenario | Test | Уровень |
|---------------|------|---------|
| Успешное получение списка домов | `ListBuildings_ReturnsSeededBuilding` | integration |
| Успешное получение таблицы квартир и владельцев | `ListApartmentsWithOwners_ReturnsSeededData` | integration |
| Успешное создание дома / квартиры | `CreateBuilding_AndApartment_Works` | integration |
| Дом не найден | `ListApartments_WhenBuildingNotFound_Returns404` | integration |
| Успешное назначение владельца | `UpsertOwner_Works` | integration |
| Квартира не найдена | `UpsertOwner_WhenApartmentNotFound_Returns404` | integration |
| Ошибка валидации владельца | `UpsertOwner_WithInvalidName_Returns400` | integration |
| Таблица квартир и владельцев | — | manual (UI на `/directories`) |

## Traceability

| Requirement | OpenAPI | Backend | Frontend | Tests |
|-------------|---------|---------|----------|-------|
| Список домов | GET `/buildings` | `ListBuildingsQueryHandler` | `DirectoriesPage` | `ListBuildings_ReturnsSeededBuilding` |
| Список квартир с владельцами | GET `/buildings/{id}/apartments` | `ListApartmentsWithOwnersQueryHandler` | `ApartmentsTable` | `ListApartmentsWithOwners_ReturnsSeededData` |
| Создание/изменение дома | POST/PUT `/buildings` | `Create/UpdateBuildingCommandHandler` | `CreateBuildingForm` | `CreateBuilding_AndApartment_Works` |
| Создание квартиры | POST `/buildings/{id}/apartments` | `CreateApartmentCommandHandler` | `DirectoriesPage` | `CreateBuilding_AndApartment_Works` |
| Назначение владельца | PUT `/apartments/{id}/owner` | `UpsertApartmentOwnerCommandHandler` | диалог владельца | `UpsertOwner_Works` |
| Таблица в SPA | — | — | `pages/directories` | manual |
