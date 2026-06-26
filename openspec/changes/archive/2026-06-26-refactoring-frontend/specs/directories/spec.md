---
capability: directories
business: docs/requirements/business/capabilities.md#directories
openapi: docs/architecture/openapi/components/openapi.yaml
adr: []
---

## MODIFIED Requirements

### Requirement: Список квартир дома с владельцами

Система SHALL возвращать квартиры выбранного дома вместе с данными владельца каждой квартиры и сводкой по последнему показанию — для отображения в таблице на странице квартир.

#### Scenario: Успешное получение таблицы квартир и владельцев

- **WHEN** клиент отправляет GET /api/v1/buildings/{buildingId}/apartments
- **THEN** статус ответа 200
- **AND** тело ответа содержит массив `items`
- **AND** каждый элемент содержит поля квартиры: `id`, `buildingId`, `number`, `floor`
- **AND** каждый элемент содержит поля владельца: `ownerId`, `ownerFullName`, `ownerPhone` (допускается `null`, если владелец не назначен)
- **AND** каждый элемент содержит поля сводки показаний: `lastReadingSubmittedAt` (nullable), `lastReadingValue` (nullable), `currentPeriodSubmitted` (boolean — сдано ли показание за текущий календарный месяц)

#### Scenario: Сортировка квартир по номеру

- **WHEN** клиент отправляет GET /api/v1/buildings/{buildingId}/apartments
- **THEN** элементы `items` отсортированы по полю `number` по возрастанию

#### Scenario: Дом не найден

- **WHEN** клиент отправляет GET /api/v1/buildings/{buildingId}/apartments для несуществующего `buildingId`
- **THEN** статус ответа 404

## ADDED Requirements

### Requirement: Удаление дома

Система SHALL удалять дом по идентификатору вместе с каскадным удалением связанных квартир, владельцев и показаний.

#### Scenario: Успешное удаление дома

- **WHEN** клиент отправляет DELETE /api/v1/buildings/{buildingId}
- **AND** дом существует
- **THEN** статус ответа 204
- **AND** дом и все связанные данные удалены из БД

#### Scenario: Дом не найден при удалении

- **WHEN** клиент отправляет DELETE /api/v1/buildings/{buildingId} для несуществующего `buildingId`
- **THEN** статус ответа 404

### Requirement: Удаление квартиры

Система SHALL удалять квартиру по идентификатору вместе с владельцем и показаниями этой квартиры.

#### Scenario: Успешное удаление квартиры

- **WHEN** клиент отправляет DELETE /api/v1/apartments/{apartmentId}
- **AND** квартира существует
- **THEN** статус ответа 204
- **AND** квартира и связанные данные удалены из БД

#### Scenario: Квартира не найдена при удалении

- **WHEN** клиент отправляет DELETE /api/v1/apartments/{apartmentId} для несуществующей квартиры
- **THEN** статус ответа 404
