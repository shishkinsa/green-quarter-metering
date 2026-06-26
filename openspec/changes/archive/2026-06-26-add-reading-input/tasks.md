## 1. Spec & Contract

- [x] 1.1 Delta specs validated (`npx openspec validate add-reading-input --strict --no-interactive`)
- [x] 1.2 Типы id (UUID) согласованы с `docs/process/context/containers.md`

## 2. OpenAPI (`docs/architecture/openapi/components/openapi.yaml`)

- [x] 2.1 Поднять `info.version` (минорная версия API)
- [x] 2.2 Добавить tag `MeterReadings`
- [x] 2.3 Schemas: `MeterReadingDto`, `SubmitMeterReadingRequest`, `SubmitMeterReadingResponse`, `BuildingMeterReadingStatusDto`, `ListBuildingMeterReadingsResponse`
- [x] 2.4 Path `POST /apartments/{apartmentId}/meter-readings` — передача/обновление показания (`submitMeterReading`)
- [x] 2.5 Path `GET /buildings/{buildingId}/meter-readings` — сводка сдачи за период (`listBuildingMeterReadings`), query: `periodYear`, `periodMonth`
- [x] 2.6 Коды ответов: 200, 201, 400, 404 — согласно `specs/meter-readings/spec.md`
- [x] 2.7 Обновить `manifest.yaml` → `capabilities.meter-readings.openapi_paths`
- [x] 2.8 Сгенерировать типы frontend: `npm run generate:api` в `src/frontend/cnt_gq_web`

## 3. Backend (Clean Architecture)

- [x] 3.1 `1 Entities` — `MeterReading` с фабрикой `Create()`
- [x] 3.2 `2 Infrastructure.Interfaces` — `IMeterReadingRepository` (GetByApartmentAndPeriod, GetLatestBeforePeriod, Upsert, ListByBuildingAndPeriod)
- [x] 3.3 `5 Infrastructure.Implementation` — репозиторий, EF-конфигурация, FK → `apartments`, UNIQUE (`ApartmentId`, `PeriodYear`, `PeriodMonth`)
- [x] 3.4 `3 UseCases` — SubmitMeterReading (Command + Validator + Handler), ListBuildingMeterReadings (Query + Handler) + Dto + Mappings
- [x] 3.5 `6 WebApp` — `MeterReadingsController` или методы в существующих контроллерах (тонкие, MediatR)
- [x] 3.6 Миграция EF: `.\scripts\ef-migrate.ps1 -Action add -MigrationName AddMeterReadings`
- [x] 3.7 Seed: несколько демо-показаний в `DatabaseSeeder` (опционально)
- [x] 3.8 Unit-тесты хэндлеров + `scenario-coverage.txt`

## 4. Frontend (FSD)

- [x] 4.1 `entities/meter-reading` — типы из OpenAPI (`generate:api`)
- [x] 4.2 API-функции: `submitMeterReading`, `listBuildingMeterReadings`
- [x] 4.3 `features/submit-meter-reading/` — форма: дом, квартира, год, месяц, значение
- [x] 4.4 `pages/meter-readings/` — форма передачи + таблица статуса (дом, период)
- [x] 4.5 Маршрут `/meter-readings` в Vue Router + пункт навигации
- [x] 4.6 TSDoc для экспортируемых API и компонентов

## 5. Docs & Architecture

- [x] 5.1 `docs/architecture/diagram/data/cnt_gq_db/database.md` — таблица `meter_readings`
- [x] 5.2 `docs/requirements/business/capabilities.md` — секция `meter-readings`
- [x] 5.3 `manifest.yaml` — capability `meter-readings` (spec, design, backend/frontend paths)

## 6. Verification

- [x] 6.1 `.\scripts\verify.ps1`
- [x] 6.2 `npx openspec validate add-reading-input --strict --no-interactive`
