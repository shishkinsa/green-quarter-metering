# Lifecycle изменения (OpenSpec + SPDF)

## Когда нужен change

| Тип | Change proposal |
|-----|-----------------|
| Новая capability / breaking API | Да |
| Новое поведение существующей capability | Да (delta spec) |
| Bugfix (восстанавливает spec) | Нет |
| Документация / typo | Нет |

## Порядок

```text
1. /opsx-propose или openspec new change "<id>"
2. Delta specs в openspec/changes/<id>/specs/
3. OpenAPI (если REST меняется) — docs/architecture/openapi/
4. ADR + LikeC4 (если архитектура/контейнеры меняются)
5. Код + тесты + scenario-coverage.txt
6. npx openspec validate <id> --strict --no-interactive
7. .\scripts\verify.ps1
8. /opsx-archive — merge в openspec/specs/
9. Обновить manifest.yaml (новая capability)
```

## Слои SPDF при change

| Слой | Что обновить |
|------|--------------|
| behavior | `openspec/changes/.../specs/` → archive → `openspec/specs/` |
| intent | `requirements/business/capabilities.md` (user stories) |
| structure | OpenAPI, ADR, LikeC4, `design.md` |
| process | `manifest.yaml` |

См. [README.md](../../README.md), [add-entity.md](add-entity.md), [AGENTS.md](../../../AGENTS.md).
