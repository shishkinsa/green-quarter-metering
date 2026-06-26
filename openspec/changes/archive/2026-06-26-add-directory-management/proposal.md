## Why

Для учёта электроэнергии в ЖК «Зелёный квартал» энергетику Василию нужен справочник объектов недвижимости: дома, квартиры и владельцы. Без этих данных невозможно привязать показания приборов учёта к конкретным помещениям и ответственным лицам. Сейчас в системе нет доменных сущностей «дом / квартира / владелец».

## What Changes

- Новая capability `directories`: справочник домов, квартир и владельцев
- Сущности БД: `buildings`, `apartments`, `owners` (связи: дом → квартиры, квартира → владелец 1:1)
- REST API: список домов, список квартир дома с владельцами, CRUD домов и квартир, назначение владельца
- SPA: страница «Справочники» — выбор дома и таблица «квартира + владелец»
- OpenAPI, EF-миграция, seed, тесты, обновление `manifest.yaml` и схемы БД

## Capabilities

### New Capabilities

- `directories`: дома, квартиры, владельцы; получение таблицы квартир и владельцев по выбранному дому

### Modified Capabilities

- _(нет)_

## Impact

- Affected specs: `openspec/specs/directories/spec.md` (после archive)
- Affected code: `Handlers/Building/`, `Handlers/Apartment/`, `Handlers/Owner/`, контроллеры, `entities/building`, `entities/apartment`, `entities/owner`, `pages/directories`
- OpenAPI: `/buildings`, `/buildings/{id}/apartments`, `/apartments/{id}/owner`
- БД: таблицы `buildings`, `apartments`, `owners`
- Docs: `manifest.yaml`, `docs/architecture/diagram/data/cnt_gq_db/database.md`, `docs/requirements/business/capabilities.md`
