# Документация «Зелёный квартал — Учёт электроэнергии» (SPDF)

Модель слоёв документации проекта. **Машиночитаемый индекс:** [manifest.yaml](../manifest.yaml).

Система учёта электроэнергии для ЖК **«Зелёный квартал»**. Ответственный: **энергетик Василий**.

## Быстрый старт

1. [AGENTS.md](../AGENTS.md) — правила и OpenSpec
2. [manifest.yaml](../manifest.yaml) — карта артефактов и capability
3. `openspec/specs/<capability>/spec.md` — канон поведения

**При расхождении приоритет:** `openspec/specs` → OpenAPI → ADR → `requirements/constraints`.

## Слои

| Вопрос | Канон | Путь |
|--------|-------|------|
| **Что** делает система? | OpenSpec Requirement + Scenario | [../openspec/specs/](../openspec/specs/) |
| **Зачем**? | Бизнес-контекст | [requirements/business/](requirements/business/) |
| **Сколько / насколько безопасно?** | Ограничения | [requirements/constraints/](requirements/constraints/) |
| **Как устроен код?** | Layer specs | [architecture/specs/](architecture/specs/) |
| **Какой REST-контракт?** | OpenAPI | [architecture/openapi/](architecture/openapi/) |
| **Стратегические решения** | ADR | [architecture/adr/](architecture/adr/) |
| **Топология C4** | LikeC4 | [architecture/diagram/](architecture/diagram/) |
| **Контейнеры (кратко)** | Таблица для AI | [process/context/containers.md](process/context/containers.md) |
| **Где в коде** | Design + manifest | `openspec/specs/<cap>/design.md` |
| **Как работать** | Workflows | [process/workflows/](process/workflows/) |
| **Стиль кода** | Standards | [standards/](standards/) |

## Не дублировать

| Не создавать | Вместо этого |
|--------------|--------------|
| `requirements/functional/*.md` с Gherkin | `openspec/specs/` |
| Отдельный `tech-stack.md` | ADR (§ Стек ниже) |
| Полная копия C4 в markdown | `process/context/containers.md` + LikeC4 |
| Матрица трассировки в 3 местах | `manifest.yaml` + `design.md` |

## Lifecycle фичи

```text
Идея → /opsx-propose → delta spec → OpenAPI → ADR/LikeC4 (если нужно)
     → код + тесты → verify.ps1 → /opsx-archive → manifest.yaml
```

Подробнее: [process/workflows/change-lifecycle.md](process/workflows/change-lifecycle.md).

## Стек (индекс ADR) {#стек-индекс-adr}

| Область | ADR / спека |
|---------|-------------|
| Backend (.NET 8) | [0001](architecture/adr/0001-dotnet-aspnet-core-backend.md) |
| Frontend (Vue 3) | [frontend.md](architecture/specs/frontend.md) |
| PostgreSQL | [0003](architecture/adr/0003-use-postgres.md) |
| CQRS (MediatR) | [0005](architecture/adr/0005-requestum-cqrs.md) * |
| OpenTelemetry | [0006](architecture/adr/0006-opentelemetry-observability.md) |
| FluentValidation | [0007](architecture/adr/0007-fluentvalidation.md) |

\* ADR-0005 описывает Requestum; в проекте используется **MediatR** — см. [backend.md](architecture/specs/backend.md) и [openspec/project.md](../openspec/project.md).

## Capability

| ID | Тип | Spec |
|----|-----|------|
| `directories` | справочники ЖК | [spec.md](../openspec/specs/directories/spec.md) |

## Проверка

```powershell
.\scripts\verify.ps1
npx likec4 validate docs/architecture/diagram
```

## `public/`

Зарезервировано под сгенерированный doc-site (VitePress/MkDocs). Пока не используется.
