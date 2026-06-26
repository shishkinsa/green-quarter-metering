---
id: BR-002
type: business
status: draft
---

# Capability и пользовательские истории

Краткий бизнес-контекст. **Канон поведения** — `openspec/specs/<capability>/spec.md`.

## examples {#examples}

CRUD примеров сущностей — эталонный сквозной сценарий шаблона.

| ID | Как | Я хочу | Чтобы |
|----|-----|--------|-------|
| US-001 | разработчик | видеть список примеров на главной | убедиться, что frontend и backend связаны |
| US-002 | разработчик | создать пример через форму | проверить POST API и обновление списка |
| US-003 | разработчик | изменить пример через модальное окно | проверить PUT API |
| US-004 | разработчик | удалить пример с подтверждением | проверить DELETE API |

- Spec: [openspec/specs/examples/spec.md](../../../openspec/specs/examples/spec.md)
- Код: `entities/example`, `features/example/*`, `Handlers/Example/`

## categories {#categories}

Read-only справочник категорий — второй capability, без мутаций.

| ID | Как | Я хочу | Чтобы |
|----|-----|--------|-------|
| US-010 | разработчик | видеть справочник категорий на главной | проверить read-only API и второй capability |

- Spec: [openspec/specs/categories/spec.md](../../../openspec/specs/categories/spec.md)
- Код: `entities/category`, `Handlers/Category/`

## auth {#auth}

Скелет аутентификации (JWT Bearer, опционально). Полный OIDC — отдельный change.

- Spec: [openspec/specs/auth/spec.md](../../../openspec/specs/auth/spec.md)
- ADR: [0008](../../architecture/adr/0008-jwt-authentication-skeleton.md)
