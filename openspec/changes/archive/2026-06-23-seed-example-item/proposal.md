## Why

Начальная загрузка шаблона: эталонная capability `examples` для демонстрации сквозного сценария OpenAPI → backend → frontend → PostgreSQL.

## What Changes

- Добавлена capability `examples` с REST API (create, list, get) и SPA (таблица + форма)
- Эталонная реализация `ExampleItem` во всех слоях

## Impact

- Affected specs: `examples`
- Affected code: `Handlers/Example/`, `ExamplesController`, `entities/example`, `features/example/create-item`, `pages/home`
- OpenAPI: `/examples`, `/examples/{id}`, `/health`
