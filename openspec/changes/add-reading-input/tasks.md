## 1. Spec & Contract

- [ ] 1.1 Delta specs validated (`npx openspec validate add-reading-input --strict --no-interactive`)
- [ ] 1.2 Типы id (UUID) согласованы с `docs/process/context/containers.md`

## 2. OpenAPI (`docs/architecture/openapi/components/openapi.yaml`)

- [ ] 2.1 Поднять `info.version` (минорная версия API)
- [ ] 2.2 Добавить tag `MeterReadings`
- [ ] 2.3 Schemas: `MeterReadingDto`, `SubmitMeterReadingRequest`, `SubmitMeterReadingResponse`, `BuildingMeterReadingStatusDto`, `ListBuildingMeterReadingsResponse`
- [ ] 2.4 Path `POST /apartments/{apartmentId}/meter-readings` — передача/обновление показания (`submitMeterReading`)
- [ ] 2.5 Path `GET /buildings/{buildingId}/meter-readings` — сводка сдачи за период (`listBuildingMeterReadings`), query: `periodYear`, `periodMonth`
- [ ] 2.6 Коды ответов: 200, 201, 400, 404 — согласно `specs/meter-readings/spec.md`
- [ ] 2.7 Обновить `manifest.yaml` → `capabilities.meter-readings.openapi_paths`
- [ ] 2.8 Сгенерировать типы frontend: `npm run generate:api` в `src/frontend/cnt_gq_web`

## 3. Backend (Clean Architecture)

- [ ] 3.1 `1 Entities` — `MeterReading` с фабрикой `Create()`
- [ ] 3.2 `2 Infrastructure.Interfaces` — `IMeterReadingRepository` (GetByApartmentAndPeriod, GetLatestBeforePeriod, Upsert, ListByBuildingAndPeriod)
- [ ] 3.3 `5 Infrastructure.Implementation` — репозиторий, EF-конфигурация, FK → `apartments`, UNIQUE (`ApartmentId`, `PeriodYear`, `PeriodMonth`)
- [ ] 3.4 `3 UseCases` — SubmitMeterReading (Command + Validator + Handler), ListBuildingMeterReadings (Query + Handler) + Dto + Mappings
- [ ] 3.5 `6 WebApp` — `MeterReadingsController` или методы в существующих контроллерах (тонкие, MediatR)
- [ ] 3.6 Миграция EF: `.\scripts\ef-migrate.ps1 -Action add -MigrationName AddMeterReadings`
- [ ] 3.7 Seed: несколько демо-показаний в `DatabaseSeeder` (опционально)
- [ ] 3.8 Unit-тесты хэндлеров + `scenario-coverage.txt`

## 4. Frontend (FSD)

- [ ] 4.1 `entities/meter-reading` — типы из OpenAPI (`generate:api`)
- [ ] 4.2 API-функции: `submitMeterReading`, `listBuildingMeterReadings`
- [ ] 4.3 `features/submit-meter-reading/` — форма: дом, квартира, год, месяц, значение
- [ ] 4.4 `pages/meter-readings/` — форма передачи + таблица статуса (дом, период)
- [ ] 4.5 Маршрут `/meter-readings` в Vue Router + пункт навигации
- [ ] 4.6 TSDoc для экспортируемых API и компонентов

## 5. Docs & Architecture

- [ ] 5.1 `docs/architecture/diagram/data/cnt_gq_db/database.md` — таблица `meter_readings`
- [ ] 5.2 `docs/requirements/business/capabilities.md` — секция `meter-readings`
- [ ] 5.3 `manifest.yaml` — capability `meter-readings` (spec, design, backend/frontend paths)

## 6. Verification

- [ ] 6.1 `.\scripts\verify.ps1`
- [ ] 6.2 `npx openspec validate add-reading-input --strict --no-interactive`
