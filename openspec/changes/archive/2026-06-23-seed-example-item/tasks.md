## 1. Spec & Contract

- [x] 1.1 Создать capability spec `examples`
- [x] 1.2 Описать REST в OpenAPI (`/examples`, `/examples/{id}`, `/health`)

## 2. Backend (Clean Architecture)

- [x] 2.1 Entity `ExampleItem` с фабрикой `Create()`
- [x] 2.2 `IExampleItemRepository` + EF-реализация
- [x] 2.3 UseCases: Create, List, GetById + Validators
- [x] 2.4 `ExamplesController`, `HealthController`
- [x] 2.5 EF-миграция `InitialCreate`

## 3. Frontend (FSD)

- [x] 3.1 `entities/example` — types, api, ExampleTable
- [x] 3.2 `features/example/create-item` — CreateExampleForm
- [x] 3.3 `pages/home` — HomePage с таблицей и формой

## 4. Docs & Architecture

- [x] 4.1 FR-001, user stories, матрица трассировки
- [x] 4.2 Схема БД `example_items`

## 5. Verification

- [x] 5.1 Unit: `ExampleItemTests`
- [x] 5.2 Integration: `ExamplesEndpointTests`
- [x] 5.3 `verify.ps1` проходит
