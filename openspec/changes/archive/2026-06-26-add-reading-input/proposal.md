## Why

Справочник домов, квартир и владельцев уже реализован, но показания электроэнергии по-прежнему собираются вручную (Excel, звонки). Энергетику Василию нужна цифровая передача показаний с привязкой к отчётному месяцу и контролем аномалий; без этого невозможно перейти к учёту потребления в системе.

## What Changes

- Новая capability `meter-readings`: передача и хранение показаний электросчётчика по квартирам
- Сущность БД `meter_readings` с привязкой к `apartments` и отчётному периоду (год + месяц)
- REST API: передача показания по квартире; сводка сдачи показаний по дому за месяц
- Валидации: показание не меньше предыдущего; одно значение на квартиру за отчётный месяц (повтор — обновление)
- SPA: форма передачи показания; таблица статуса сдачи по выбранному дому и месяцу
- OpenAPI, EF-миграция, тесты, обновление `manifest.yaml` и схемы БД

## Capabilities

### New Capabilities

- `meter-readings`: передача показания владельцем квартиры, просмотр статуса сдачи по дому за отчётный месяц, бизнес-правила монотонности и уникальности периода

### Modified Capabilities

- _(нет)_

## Impact

- Affected specs: `openspec/specs/meter-readings/spec.md` (после archive)
- Affected code: `Handlers/MeterReading/`, `MeterReadingsController`, `entities/meter-reading`, `features/meter-reading/`, `pages/meter-readings`
- OpenAPI: `POST /apartments/{apartmentId}/meter-readings`, `GET /buildings/{buildingId}/meter-readings`
- БД: таблица `meter_readings`
- Docs: `manifest.yaml`, `docs/architecture/diagram/data/cnt_gq_db/database.md`, `docs/requirements/business/capabilities.md`
