## Context

Справочник (`directories`) хранит дома, квартиры и владельцев. Дата поверки — атрибут квартиры. Сейчас `Apartment` содержит только `Number` и `Floor`; create API — POST без update endpoint (хотя spec уже декларирует «создание и изменение»). UI: модалка «Добавить квартиру» без редактирования.

## Goals / Non-Goals

**Goals:**

- Персистентное поле `MeterVerificationDate` на квартире.
- Задание даты при **POST create** и **PUT update** квартиры.
- Реализация PUT `/apartments/{id}` (закрывает gap spec vs код).
- Валидация: дата не в будущем (UTC).
- UI: date picker в формах добавления и редактирования квартиры; колонка в таблице.

**Non-Goals:**

- Дата поверки в upsert владельца — не входит в scope.
- Автоматический расчёт следующей поверки, уведомления, блокировка показаний.
- Отдельный REST-ресурс «поверка».

## Decisions

### 1. Хранение на `Apartment`

**Решение:** колонка `MeterVerificationDate` (`date`, nullable) в `apartments`.

**Причина:** поверка относится к прибору учёта в помещении; не зависит от владельца.

### 2. Create и Update квартиры — единая точка ввода даты

**Решение:**

- `meterVerificationDate` в `CreateApartmentRequest` и `UpdateApartmentRequest` (optional, nullable).
- PUT `/api/v1/apartments/{apartmentId}` — body: `number`, `floor`, `meterVerificationDate`.
- Handler create/update валидирует и сохраняет дату вместе с остальными полями квартиры.

**Причина:** соответствует UX «дата при добавлении и редактировании квартиры»; владелец редактируется отдельно.

### 3. PUT update квартиры (новый endpoint)

**Решение:** добавить `UpdateApartmentCommand` + controller action; 409 при конфликте номера в доме; 404 если квартира не найдена.

**Причина:** в коде отсутствует update, но требуется для редактирования; spec `directories` уже подразумевает изменение квартиры.

### 4. Тип и валидация даты

**Решение:** `DateOnly?` в entity; JSON `format: date`; если задана — `<= UtcNow.Date`.

### 5. Frontend

**Решение:** расширить `CreateApartmentDialog` или выделить `UpsertApartmentDialog` (create + edit); кнопка «Редактировать» в `ApartmentsTable`; date picker с `disabledDate` для будущих дней.

## Risks / Trade-offs

- [Новый PUT endpoint — расширение scope change] → оправдано требованием «редактирование квартиры»; без него дату нельзя обновить после создания.
- [UTC vs локаль браузера] → backend — источник истины.

## Migration Plan

1. EF-миграция `AddApartmentMeterVerificationDate`.
2. Backend (create + update), затем frontend.
3. Rollback: откат миграции.

## Open Questions

- _(нет)_
