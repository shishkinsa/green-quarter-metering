# Инструкции для AI-агентов

**Зеленый квартал — Учёт электроэнергии** — шаблон full stack: React/TypeScript (FSD) + ASP.NET Core / .NET 10 + PostgreSQL.

**Карта документации:** [docs/README.md](docs/README.md) · [manifest.yaml](manifest.yaml) · [openspec/project.md](openspec/project.md)

Язык коммуникации с пользователем: **русский**.

<!-- OPENSPEC:START -->
## OpenSpec (изменения поведения)

1. Прочитай [openspec/project.md](openspec/project.md) и этот файл (`AGENTS.md`)
2. Создай change: `/opsx-propose <описание>` или `openspec new change "<id>"`
3. Full stack: schema `full-stack` — [openspec/schemas/full-stack/](openspec/schemas/full-stack/)
4. Валидируй: `npx openspec validate <change-id> --strict --no-interactive`
5. Реализуй: `/opsx-apply` после согласования proposal
6. Архивируй: `/opsx-archive` → merge в `openspec/specs/`

**Канон поведения:** `openspec/specs/<capability>/spec.md`

Keep this managed block so `openspec update` can refresh the instructions.
<!-- OPENSPEC:END -->

## Порядок работы

1. Слой: `frontend` / `backend` / `architecture` / `infra`
2. [manifest.yaml](manifest.yaml) → артефакты нужной capability
3. [docs/process/context/containers.md](docs/process/context/containers.md) + [openspec/project.md](openspec/project.md)
4. ADR: [docs/architecture/adr/](docs/architecture/adr/)
5. **Новая фича:** change → OpenAPI/ADR → код → тесты → archive → manifest
6. **Bugfix:** без change, если восстанавливает spec

## OpenSpec: краткий чеклист

- Изучи существующее: `npx openspec list --specs`, `npx openspec list`
- Определи scope: новая capability или изменение существующей
- Выбери уникальный `change-id`: kebab-case, с глаголом (`add-`, `update-`, `remove-`)
- Full stack сущность: `openspec new change "<id>" --schema full-stack`
- Создай: `proposal.md`, `tasks.md`, `design.md` (при необходимости), delta specs по затронутым capability
- Delta: `## ADDED|MODIFIED|REMOVED|RENAMED Requirements`; минимум один `#### Scenario:` на requirement
- Валидируй: `npx openspec validate <change-id> --strict --no-interactive`
- **Не начинай реализацию** до согласования proposal

## OpenSpec: три этапа

### Этап 1 — Создание change

**Нужен proposal при:** новой фиче, breaking change, смене архитектуры/паттернов, обновлении security.

**Proposal не нужен при:** bugfix (восстанавливает spec), typo/форматирование, non-breaking обновлении зависимостей.

1. `openspec/project.md`, `npx openspec list`, `npx openspec list --specs`
2. `change-id` с глаголом → `openspec/changes/<id>/`
3. Delta specs: `## ADDED|MODIFIED|REMOVED Requirements`
4. `npx openspec validate <id> --strict --no-interactive`

### Этап 2 — Реализация

1. `proposal.md`, `design.md` (если есть), `tasks.md`
2. OpenAPI: [docs/architecture/openapi/components/openapi.yaml](docs/architecture/openapi/components/openapi.yaml)
3. Код по [docs/process/workflows/add-entity.md](docs/process/workflows/add-entity.md)
4. Все задачи в `tasks.md` → `- [x]`
5. `.\scripts\verify.ps1`

### Этап 3 — Архивация

- `/opsx-archive` или `npx openspec archive <change-id> --yes`
- Delta → `openspec/specs/`
- Change → `openspec/changes/archive/YYYY-MM-DD-<name>/`

## Роли артефактов

| Слой | Путь | Назначение |
|------|------|------------|
| Поведение (WHAT) | `openspec/specs/` | Канон требований |
| Изменения | `openspec/changes/` | Delta specs + tasks |
| Бизнес (WHY) | `docs/requirements/business/` | Цели, user stories |
| Ограничения | `docs/requirements/constraints/` | Perf, security |
| Стратегия | `docs/architecture/adr/` | ADR |
| REST | `docs/architecture/openapi/` | Контракт API |
| C4 | `docs/architecture/diagram/` | Контейнеры |

## Формат spec-файла

Каждый spec: `## Purpose` и `## Requirements`.

```markdown
## Purpose
[краткое назначение capability]

## Requirements

### Requirement: Понятная формулировка
The system SHALL ...

#### Scenario: Описательное имя
- **WHEN** ...
- **THEN** ...
```

Операции delta: `## ADDED Requirements`, `## MODIFIED Requirements`, `## REMOVED Requirements`, `## RENAMED Requirements`.

## Команды CLI

```bash
npx openspec list                  # активные changes
npx openspec list --specs          # capabilities
npx openspec show <item>           # детали
npx openspec validate <item> --strict --no-interactive
npx openspec archive <change> --yes
npx openspec new change "<name>" --schema full-stack
```

## Slash-команды Cursor

| Команда | Назначение |
|---------|------------|
| `/opsx-propose` | Создать change и артефакты планирования |
| `/opsx-apply` | Реализовать по tasks.md |
| `/opsx-archive` | Архивировать и слить specs |
| `/opsx-explore` | Исследовать до commit в change |
| `/opsx-sync` | Слить delta specs в main specs |

## Дерево решений

```text
Новый запрос?
├─ Bugfix (восстанавливает spec)? → правка без change
├─ Typo / формат / комментарий? → правка без change
├─ Новая фича / capability? → proposal (full-stack для сущностей)
├─ Breaking change? → proposal
└─ Неясно? → proposal (безопаснее)
```

**Specs — истина. Changes — предложения. Держи их синхронно.**

## Ключевые документы

| Документ | Назначение |
|----------|------------|
| [docs/README.md](docs/README.md) | Модель SPDF, слои, lifecycle |
| [manifest.yaml](manifest.yaml) | Индекс capability и путей |
| [openspec/project.md](openspec/project.md) | Контекст стека и соглашения |
| [docs/process/workflows/add-entity.md](docs/process/workflows/add-entity.md) | Новая сущность |
| [docs/process/workflows/change-lifecycle.md](docs/process/workflows/change-lifecycle.md) | Lifecycle change |
| [docs/architecture/openapi/components/openapi.yaml](docs/architecture/openapi/components/openapi.yaml) | REST |

## Эталонная capability

| ID | Spec | Код |
|----|------|-----|
| `directories` | [spec.md](openspec/specs/directories/spec.md) | `Handlers/Building/`, `entities/building`, `pages/directories` |

## Правила

- API ↔ OpenAPI ↔ spec синхронно
- Не добавляй зависимости без необходимости
- UseCases и критичная логика — с тестами

## Проверка

```powershell
.\scripts\verify.ps1
```
