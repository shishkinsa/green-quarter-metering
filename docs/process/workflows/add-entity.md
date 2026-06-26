# Workflow: добавление новой сущности

Чеклист для AI и разработчика.

**Эталоны capability:**

| Тип | Capability | Код |
|-----|------------|-----|
| CRUD (write) | `examples` / `ExampleItem` | [design.md](../../../openspec/specs/examples/design.md) |
| Read-only справочник | `categories` / `Category` | [design.md](../../../openspec/specs/categories/design.md) |

Для read-only сущности пропустите шаги Command/Validator/features и оставьте Query + List endpoint.

## 0. OpenSpec change (обязательно для новых сущностей)

- [ ] `/opsx-propose add-{entity}` или `npx openspec new change "add-{entity}" --schema full-stack`
- [ ] Delta specs в `openspec/changes/<id>/specs/{capability}/spec.md`
- [ ] `npx openspec validate <id> --strict --no-interactive` — до реализации
- [ ] Approve proposal перед кодом
- [ ] После деплоя: `/opsx-archive` — merge в `openspec/specs/`

Шаблон tasks для schema `full-stack`: [openspec/schemas/full-stack/templates/tasks.md](../../../openspec/schemas/full-stack/templates/tasks.md)

## 1. Требования

- [ ] Delta spec в OpenSpec change (не отдельный FR для поведения)
- [ ] Бизнес-контекст в [capabilities.md](../../requirements/business/capabilities.md) при необходимости
- [ ] Обновить [manifest.yaml](../../../manifest.yaml) при новой capability

## 2. Контракт

- [ ] Добавить paths/schemas в [docs/architecture/openapi/components/openapi.yaml](../../architecture/openapi/components/openapi.yaml)
- [ ] Согласовать типы id (UUID / int / code) с [context/containers.md](../context/containers.md)

## 3. Backend

- [ ] `1 Entities` — доменная сущность с фабрикой `Create()`
- [ ] `2 Infrastructure.Interfaces` — `I{Name}Repository`
- [ ] `5 Infrastructure.Implementation` — репозиторий + EF-конфигурация
- [ ] `3 UseCases` — Command/Query + Handler + Validator + Dto + Mappings
- [ ] `6 WebApp` — Controller (тонкий, Requestum)
- [ ] Миграция EF: `.\scripts\ef-migrate.ps1 -Action add -MigrationName Add{Name}`
- [ ] Integration test endpoint + `openspec/specs/{capability}/scenario-coverage.txt`

## 4. Frontend (FSD)

- [ ] `entities/{name}/model/types.ts` — типы из OpenAPI (или `npm run generate:api`)
- [ ] `entities/{name}/api/` — функции API
- [ ] `entities/{name}/ui/` — таблица / карточка
- [ ] `features/{name}/` — сценарии (create, edit, …)
- [ ] `pages/` — подключить на страницу
- [ ] TSDoc для экспортируемых API и компонентов

## 5. Документация

- [ ] `docs/architecture/diagram/data/` — схема БД при новых таблицах
- [ ] ADR — при новой технологии или контейнере
- [ ] `docs/process/snippets/good-examples.md` — добавить эталонный фрагмент

## 6. Проверка

```powershell
.\scripts\verify.ps1
npx openspec validate --specs --strict --no-interactive
```
