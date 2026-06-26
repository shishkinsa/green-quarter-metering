# Categories — Technical Design

Второй эталон capability: read-only справочник без мутаций (контраст с CRUD `examples`).

## Backend

- **Entity**: `Category` в `1 Entities/`
- **Repository**: `ICategoryRepository` → `CategoryRepository` (только List)
- **UseCases**: `Handlers/Category/Queries/ListCategories`
- **API**: `CategoriesController` — GET only
- **Persistence**: таблица `categories`, seed через `DatabaseSeeder` (+ миграция `AddCategories`)

## Frontend (FSD)

- `entities/category/model/types.ts`
- `entities/category/api/categoryApi.ts`
- `entities/category/ui/CategoryTable.tsx`
- `pages/home/` — блок «Категории»

## Tests

- Integration: `CategoriesEndpointTests.ListCategories_ReturnsSeededItems`
- Покрытие API-сценариев: [scenario-coverage.txt](scenario-coverage.txt)

## Scenario coverage

| Spec scenario | Test | Уровень |
|---------------|------|---------|
| List categories | `ListCategories_ReturnsSeededItems` | integration |
| Categories visible on home | — | manual (UI на HomePage) |

## Traceability

| Requirement | OpenAPI | Backend | Frontend | Tests |
|-------------|---------|---------|----------|-------|
| List Categories | GET `/categories` | `ListCategoriesQueryHandler` | `CategoryTable` | `ListCategories_ReturnsSeededItems` |
| Display In SPA | — | — | `HomePage` | — |

## Когда использовать как эталон

- Справочники без write API
- Seed data + read-only repository
- Минимальный frontend: entity + table без features/*

См. [docs/process/workflows/add-entity.md](../../../docs/process/workflows/add-entity.md) — полный чеклист для write-capability.
