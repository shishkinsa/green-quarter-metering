## 1. Spec & Contract

- [x] 1.1 Delta specs validated (`npx openspec validate add-directory-management --strict --no-interactive`)
- [x] 1.2 Типы id (UUID) согласованы с `docs/process/context/containers.md`

## 2. OpenAPI (`docs/architecture/openapi/components/openapi.yaml`)

- [x] 2.1 Поднять `info.version` (минорная версия API)
- [x] 2.2 Добавить tag `Directories` (справочник домов, квартир, владельцев)
- [x] 2.3 Schemas: `BuildingDto`, `ApartmentDto`, `ApartmentWithOwnerDto`, `OwnerDto`, request/response для create/update/upsert, `ListBuildingsResponse`, `ListApartmentsWithOwnersResponse`
- [x] 2.4 Path `GET /buildings` — список домов (`listBuildings`)
- [x] 2.5 Path `POST /buildings` — создание дома (`createBuilding`)
- [x] 2.6 Path `PUT /buildings/{id}` — обновление дома (`updateBuilding`)
- [x] 2.7 Path `GET /buildings/{buildingId}/apartments` — квартиры дома с владельцами для таблицы (`listApartmentsWithOwners`)
- [x] 2.8 Path `POST /buildings/{buildingId}/apartments` — создание квартиры (`createApartment`)
- [x] 2.9 Path `PUT /apartments/{apartmentId}/owner` — назначение/обновление владельца (`upsertApartmentOwner`)
- [x] 2.10 Коды ответов: 200, 201, 400, 404, 409 — согласно `specs/directories/spec.md`
- [x] 2.11 Обновить `manifest.yaml` → `capabilities.directories.openapi_paths`
- [x] 2.12 Сгенерировать типы frontend: `npm run generate:api` в `src/frontend/cnt_gq_web`

## 3. Backend (Clean Architecture)

- [x] 3.1 `1 Entities` — `Building`, `Apartment`, `Owner` с фабриками `Create()`
- [x] 3.2 `2 Infrastructure.Interfaces` — `IBuildingRepository`, `IApartmentRepository`, `IOwnerRepository`
- [x] 3.3 `5 Infrastructure.Implementation` — репозитории, EF-конфигурации, FK и UNIQUE (`BuildingId`+`Number`, `ApartmentId` на owners)
- [x] 3.4 `3 UseCases` — ListBuildings, ListApartmentsWithOwners, Create/Update Building, Create/Update Apartment, Upsert Owner + Validators + Dto
- [x] 3.5 `6 WebApp` — `BuildingsController`, `ApartmentsController` (тонкие, MediatR)
- [x] 3.6 Миграция EF: `.\scripts\ef-migrate.ps1 -Action add -MigrationName AddBuildingsApartmentsOwners`
- [x] 3.7 Seed: дома, квартиры, владельцы в `DatabaseSeeder`
- [x] 3.8 Integration `BuildingsEndpointTests`, `ApartmentsEndpointTests` + `scenario-coverage.txt`

## 4. Frontend (FSD)

- [x] 4.1 `entities/building`, `entities/apartment`, `entities/owner` — типы из OpenAPI (`generate:api`)
- [x] 4.2 API-функции: listBuildings, listApartmentsWithOwners, create/update building, create apartment, upsert owner
- [x] 4.3 `pages/directories/` — выбор дома + таблица (номер, этаж, ФИО, телефон)
- [x] 4.4 Формы создания/редактирования дома, квартиры и владельца (features)
- [x] 4.5 Маршрут `/directories` в Vue Router + пункт навигации
- [x] 4.6 TSDoc для экспортируемых API и компонентов

## 5. Docs & Architecture

- [x] 5.1 `docs/architecture/diagram/data/cnt_gq_db/database.md` — таблицы `buildings`, `apartments`, `owners`
- [x] 5.2 `docs/requirements/business/capabilities.md` — секция `directories`
- [x] 5.3 `manifest.yaml` — capability `directories` (spec, design, backend/frontend paths)

## 6. Verification

- [x] 6.1 `.\scripts\verify.ps1`
- [x] 6.2 `npx openspec validate add-directory-management --strict --no-interactive`
