---
capability: meter-readings
business: docs/requirements/business/capabilities.md#meter-readings
openapi: docs/architecture/openapi/components/openapi.yaml
adr: []
---

## MODIFIED Requirements

### Requirement: Передача показания по квартире

Система SHALL принимать текущее показание электросчётчика для квартиры и отчётного периода (календарный год и месяц) и фиксировать дату и время передачи.

#### Scenario: Успешная передача нового показания

- **WHEN** клиент отправляет POST /api/v1/apartments/{apartmentId}/meter-readings с телом `{"periodYear":2026,"periodMonth":6,"value":12345.5}`
- **AND** квартира существует
- **AND** для квартиры нет показания за указанный период
- **AND** значение `value` не меньше последнего сохранённого показания по этой квартире (если предыдущее показание есть)
- **THEN** статус ответа 201
- **AND** тело ответа `item` содержит `id`, `apartmentId`, `periodYear`, `periodMonth`, `value`, `submittedAt` (UTC, ISO 8601)

#### Scenario: Обновление показания за тот же месяц

- **WHEN** клиент отправляет POST /api/v1/apartments/{apartmentId}/meter-readings с телом `{"periodYear":2026,"periodMonth":6,"value":12350}`
- **AND** для квартиры уже есть показание за период 2026-06
- **AND** новое `value` не меньше последнего показания за предыдущие периоды
- **THEN** статус ответа 200
- **AND** тело ответа `item` содержит обновлённое `value` и обновлённый `submittedAt`

#### Scenario: Показание меньше предыдущего

- **WHEN** клиент отправляет POST /api/v1/apartments/{apartmentId}/meter-readings
- **AND** переданное `value` меньше последнего сохранённого показания по квартире (за любой предшествующий период)
- **THEN** статус ответа 400
- **AND** тело ответа содержит описание ошибки валидации

#### Scenario: Ошибка валидации периода

- **WHEN** клиент отправляет POST /api/v1/apartments/{apartmentId}/meter-readings с `periodMonth` вне диапазона 1–12 или с отрицательным `value`
- **THEN** статус ответа 400

#### Scenario: Квартира не найдена

- **WHEN** клиент отправляет POST /api/v1/apartments/{apartmentId}/meter-readings для несуществующей квартиры
- **THEN** статус ответа 404

## ADDED Requirements

### Requirement: История показаний по квартире

Система SHALL возвращать все сохранённые показания выбранной квартиры для отображения в таблице на frontend.

#### Scenario: Успешное получение истории

- **WHEN** клиент отправляет GET /api/v1/apartments/{apartmentId}/meter-readings
- **AND** квартира существует
- **THEN** статус ответа 200
- **AND** тело ответа содержит массив `items`
- **AND** каждый элемент содержит `id`, `apartmentId`, `periodYear`, `periodMonth`, `value`, `submittedAt`
- **AND** элементы отсортированы по `periodYear` и `periodMonth` по убыванию

#### Scenario: Пустая история

- **WHEN** клиент отправляет GET /api/v1/apartments/{apartmentId}/meter-readings
- **AND** у квартиры нет показаний
- **THEN** статус ответа 200
- **AND** `items` — пустой массив

#### Scenario: Квартира не найдена

- **WHEN** клиент отправляет GET /api/v1/apartments/{apartmentId}/meter-readings для несуществующей квартиры
- **THEN** статус ответа 404
