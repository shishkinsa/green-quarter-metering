# Examples — Technical Design

## Backend

- **Entity**: `ExampleItem` в `1 Entities/` с фабрикой `Create()`
- **Repository**: `IExampleItemRepository` → `ExampleItemRepository` (EF Core)
- **UseCases**: `Handlers/Example/` — Create, List, GetById, Update, Delete
- **API**: `ExamplesController` — GET/POST/PUT/DELETE
- **Persistence**: таблица `example_items`, миграция EF
- **Auth**: POST защищён `[Authorize]` при `Auth:Enabled=true` (см. capability `auth`)

## Frontend (FSD)

- `entities/example/model/types.ts` — типы из OpenAPI
- `entities/example/api/` — вызовы REST
- `entities/example/ui/ExampleTable.tsx` — таблица
- `features/example/create-item/` — форма создания
- `features/example/edit-item/` — модальное редактирование
- `features/example/delete-item/` — удаление с подтверждением
- `pages/home/` — подключение на главной

## Tests

- Unit: `ExampleItemTests` (домен)
- Unit: `CreateExampleCommandValidatorTests`, `UpdateExampleCommandValidatorTests`
- Integration: `ExamplesEndpointTests` (health, CRUD, 400/404)
- Frontend: `ExampleTable.test.tsx` (отображение таблицы)

Проверка покрытия API-сценариев: `scripts/check-spec-coverage.ps1` (читает [scenario-coverage.txt](scenario-coverage.txt)).

## Scenario coverage (OpenSpec → tests)

| Spec scenario | Requirement | Test(s) | Уровень |
|---------------|-------------|---------|---------|
| Health endpoint available | Health Check | `GetHealth_ReturnsOk` | integration |
| Successful creation | Create | `CreateAndListExample_Works` | integration |
| Validation error | Create | `CreateExample_WithInvalidName_ReturnsBadRequest`, `CreateExampleCommandValidatorTests` | integration + unit |
| List after creation | List | `CreateAndListExample_Works` | integration |
| Get existing item | Get By Id | `GetExampleById_WhenExists_ReturnsOk` | integration |
| Item not found | Get By Id | `GetExampleById_WhenNotFound_ReturnsNotFound` | integration |
| Successful update | Update | `UpdateAndDeleteExample_Works` | integration |
| Update validation error | Update | `UpdateExample_WithInvalidName_ReturnsBadRequest`, `UpdateExampleCommandValidatorTests` | integration + unit |
| Update not found | Update | `UpdateExample_WhenNotFound_ReturnsNotFound` | integration |
| Successful delete | Delete | `UpdateAndDeleteExample_Works` | integration |
| Delete not found | Delete | `DeleteExample_WhenNotFound_ReturnsNotFound` | integration |
| View list on home page | Display In SPA | `ExampleTable.test.tsx` | frontend unit |
| Create via form | Display In SPA | — | manual / E2E (в форке) |
| Edit via modal | Edit And Delete In SPA | — | manual / E2E (в форке) |
| Delete via confirmation | Edit And Delete In SPA | — | manual / E2E (в форке) |

## Traceability

| Requirement | OpenAPI | Backend | Frontend | Tests |
|-------------|---------|---------|----------|-------|
| Health Check | GET `/health` | `HealthController` | `HomePage` | `GetHealth_ReturnsOk` |
| Create | POST `/examples` | `CreateExampleCommandHandler` | `CreateExampleForm` | `CreateAndListExample_Works`, `CreateExample_WithInvalidName_ReturnsBadRequest` |
| Create validation | POST `/examples` 400 | `CreateExampleCommandValidator` | — | `CreateExample_WithInvalidName_ReturnsBadRequest`, `CreateExampleCommandValidatorTests` |
| List | GET `/examples` | `ListExamplesQueryHandler` | `ExampleTable` | `CreateAndListExample_Works` |
| Get By Id | GET `/examples/{id}` | `GetExampleByIdQueryHandler` | — | `GetExampleById_WhenExists_ReturnsOk`, `GetExampleById_WhenNotFound_ReturnsNotFound` |
| Update | PUT `/examples/{id}` | `UpdateExampleCommandHandler` | `EditExampleModal` | `UpdateAndDeleteExample_Works`, `UpdateExample_WithInvalidName_ReturnsBadRequest`, `UpdateExample_WhenNotFound_ReturnsNotFound` |
| Update validation | PUT `/examples/{id}` 400 | `UpdateExampleCommandValidator` | — | `UpdateExample_WithInvalidName_ReturnsBadRequest`, `UpdateExampleCommandValidatorTests` |
| Delete | DELETE `/examples/{id}` | `DeleteExampleCommandHandler` | `DeleteExampleButton` | `UpdateAndDeleteExample_Works`, `DeleteExample_WhenNotFound_ReturnsNotFound` |
| Display In SPA | — | — | `HomePage`, `ExampleTable` | `ExampleTable.test.tsx` |
