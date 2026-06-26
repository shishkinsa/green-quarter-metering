## Why

Capability `examples` поддерживает только create/list/get. Для полного CRUD-примера и демонстрации OpenSpec pilot нужны update и delete — типичный сценарий «1 → n» при развитии эталонной фичи.

## What Changes

- REST: PUT `/api/v1/examples/{id}` — обновление name
- REST: DELETE `/api/v1/examples/{id}` — удаление (204)
- SPA: редактирование и удаление строк в таблице на главной странице
- Unit + integration тесты

## Capabilities

### Modified Capabilities

- `examples` — ADDED requirements для update/delete API и UI

## Impact

- Affected specs: `openspec/specs/examples/spec.md`
- Affected code: `Handlers/Example/`, `ExamplesController`, `entities/example`, `features/example/*`, `pages/home`
- OpenAPI: PUT/DELETE на `/examples/{id}`
- БД: без миграции (схема не меняется)
