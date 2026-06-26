---
id: BR-002
type: business
status: draft
---

# Capability и пользовательские истории

Краткий бизнес-контекст. **Канон поведения** — `openspec/specs/<capability>/spec.md`.

## directories {#directories}

Справочник домов, квартир и владельцев ЖК — главная страница приложения.

| ID | Как | Я хочу | Чтобы |
|----|-----|--------|-------|
| US-020 | энергетик | видеть список домов | выбрать объект для просмотра квартир |
| US-021 | энергетик | видеть таблицу квартир и владельцев по дому | знать, кому принадлежит помещение |
| US-022 | энергетик | добавлять дома, квартиры и владельцев | вести справочник без правки БД |

- Spec: [openspec/specs/directories/spec.md](../../../openspec/specs/directories/spec.md)
- Код: `entities/building`, `features/directory/*`, `Handlers/Building/`, `pages/directories`
