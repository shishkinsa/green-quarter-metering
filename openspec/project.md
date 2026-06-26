# Project Context

## Purpose

**Зелёный квартал — Учёт электроэнергии** — система учёта электроэнергии жилого комплекса. Ответственный: энергетик Василий. Каркас на базе [templates-for-ai-project](https://github.com/shishkinsa/templates-for-ai-project): OpenSpec + Clean Architecture + FSD.

Бизнес-цели: [docs/requirements/business/goals.md](../docs/requirements/business/goals.md)

SPDF: [docs/README.md](../docs/README.md) · [manifest.yaml](../manifest.yaml)

## Tech Stack

| Слой | Технологии | Документ |
|------|------------|----------|
| Frontend | Vue 3, TypeScript, Vite, Element Plus, FSD | [docs/architecture/specs/frontend.md](../docs/architecture/specs/frontend.md) |
| Backend | ASP.NET Core 8, EF Core, MediatR, FluentValidation | [docs/architecture/specs/backend.md](../docs/architecture/specs/backend.md) |
| Data | PostgreSQL | [docs/architecture/adr/0003-use-postgres.md](../docs/architecture/adr/0003-use-postgres.md) |
| Observability | OpenTelemetry, Prometheus, Loki, Tempo, Grafana | [monitoring/README.md](../monitoring/README.md) |

## Project Conventions

### Code Style

- C#: [docs/standards/naming/csharp.md](../docs/standards/naming/csharp.md) + `src/.editorconfig`
- TypeScript / Vue: [docs/standards/naming/typescript.md](../docs/standards/naming/typescript.md)
- SQL: [docs/standards/naming/postgresql.md](../docs/standards/naming/postgresql.md)
- Общие принципы: [docs/standards/coding-standards.md](../docs/standards/coding-standards.md)

### Architecture Patterns

- **Backend**: Clean Architecture, CQRS через MediatR — см. [backend.md](../docs/architecture/specs/backend.md)
- **Data access**: `IDbContext` в handlers (один `SaveChangesAsync` на сценарий); `I*Queries` только для сложных выборок — без CRUD-репозиториев на сущность
- **Frontend**: Feature-Sliced Design — см. [frontend.md](../docs/architecture/specs/frontend.md)
- **C4 / контейнеры**: [docs/process/context/containers.md](../docs/process/context/containers.md), LikeC4 в `docs/architecture/diagram/`
- **Стратегические решения**: ADR в `docs/architecture/adr/` — не дублировать в capability specs

### Testing Strategy

- Unit-тесты для домена и UseCase handlers (in-memory EF + `IDbContext`)
- Integration-тесты для REST endpoints (`WebApplicationFactory`, InMemory БД)
- Проверка: `.\scripts\verify.ps1`

### Git Workflow

- [docs/standards/git-flow.md](../docs/standards/git-flow.md): `feature/*`, `fix/*`, `docs/*`

## Domain Context

- Контейнеры: `CNT_GQ_Web` (Vue SPA), `CNT_GQ_WebAPI` (REST), `CNT_GQ_DB` (PostgreSQL)
- Идентификаторы домена: UUID — см. [containers.md](../docs/process/context/containers.md)
- Основная capability: `directories` — справочники домов, квартир и владельцев

## Important Constraints

- Публичный REST-контракт: [docs/architecture/openapi/components/openapi.yaml](../docs/architecture/openapi/components/openapi.yaml) — синхронно с кодом
- Не противоречить принятым ADR без нового ADR
- Язык коммуникации с пользователем: **русский**

## External Dependencies

- PostgreSQL 16+ (локально или Docker)
- .NET SDK 10.x, Node.js 22+

## Artifact Roles (гибридная модель)

| Артефакт | Назначение |
|----------|------------|
| `openspec/specs/` | Канон поведения системы (WHAT) |
| `docs/requirements/business/` | Бизнес-цели и user stories (WHY) |
| `docs/requirements/constraints/` | Ограничения: perf, security |
| `docs/architecture/adr/` | Стратегические архитектурные решения |
| `docs/architecture/openapi/` | REST-контракт (синхрон с capability API) |
| `openspec/changes/` | Предложения изменений (delta specs) |

## Reference Implementation

| Capability | Spec | Backend | Frontend | Tests |
|------------|------|---------|----------|-------|
| `directories` | [spec.md](specs/directories/spec.md) | `Handlers/Building/` | `entities/building`, `features/directory/*`, `pages/directories` | `BuildingsEndpointTests`, `ApartmentsEndpointTests` |

Старт из шаблона: [docs/process/workflows/bootstrap-project.md](../docs/process/workflows/bootstrap-project.md)
