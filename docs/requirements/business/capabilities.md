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

## meter-readings {#meter-readings}

Передача показаний электросчётчиков по квартирам и сводка сдачи по дому за отчётный месяц.

| ID | Как | Я хочу | Чтобы |
|----|-----|--------|-------|
| US-030 | владелец квартиры | передать текущее показание счётчика | не звонить энергетику и не вести Excel |
| US-031 | энергетик | видеть, кто сдал показания по дому за месяц | контролировать полноту сдачи |
| US-032 | энергетик | получать отказ при показании меньше предыдущего | отсекать ошибки и аномалии ввода |

- Spec: [openspec/changes/add-reading-input/specs/meter-readings/spec.md](../../../openspec/changes/add-reading-input/specs/meter-readings/spec.md)
- Код: `entities/meter-reading`, `features/submit-meter-reading`, `Handlers/MeterReading/`, `pages/meter-readings`
