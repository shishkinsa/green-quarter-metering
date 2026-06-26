## 1. Spec & Contract

- [ ] 1.1 Delta specs validated (`npx openspec validate <change-id> --strict --no-interactive`)
- [ ] 1.2 OpenAPI: docs/architecture/openapi/components/openapi.yaml
- [ ] 1.3 Типы id согласованы с docs/process/context/containers.md

## 2. Backend (Clean Architecture)

- [ ] 2.1 `1 Entities` — доменная сущность с фабрикой `Create()`
- [ ] 2.2 `2 Infrastructure.Interfaces` — `IDbContext`; при сложных выборках — `I{Name}Queries`
- [ ] 2.3 `5 Infrastructure.Implementation` — EF-конфигурация + query-сервисы (`Queries/`)
- [ ] 2.4 `3 UseCases` — Command/Query + Handler + Validator + Dto + Mappings
- [ ] 2.5 `6 WebApp` — Controller (тонкий, Requestum)
- [ ] 2.6 Миграция EF: `.\scripts\ef-migrate.ps1 -Action add -MigrationName Add{Name}`
- [ ] 2.7 Unit-тест handler + integration test endpoint

## 3. Frontend (FSD)

- [ ] 3.1 `entities/{name}/model/types.ts` — типы из OpenAPI
- [ ] 3.2 `entities/{name}/api/` — функции API
- [ ] 3.3 `entities/{name}/ui/` — таблица / карточка
- [ ] 3.4 `features/{name}/` — сценарии (create, edit, …)
- [ ] 3.5 `pages/` — подключить на страницу
- [ ] 3.6 TSDoc для экспортируемых API и компонентов

## 4. Docs & Architecture

- [ ] 4.1 `docs/architecture/diagram/data/` — схема БД при новых таблицах
- [ ] 4.2 ADR — при новой технологии или контейнере
- [ ] 4.3 Матрица трассировки в docs/requirements/README.md
- [ ] 4.4 docs/process/snippets/good-examples.md — при новом паттерне

## 5. Verification

- [ ] 5.1 `.\scripts\verify.ps1`
- [ ] 5.2 `npx openspec validate --specs --strict --no-interactive`
