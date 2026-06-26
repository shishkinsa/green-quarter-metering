---
capability: directories
business: docs/requirements/business/capabilities.md#directories
openapi: docs/architecture/openapi/components/openapi.yaml
adr: []
---

## Purpose

Справочник объектов ЖК «Зелёный квартал»: дома, квартиры и владельцы. Система SHALL хранить сущности в PostgreSQL и предоставлять REST API и табличное отображение квартир выбранного дома вместе с данными владельцев.

REST-контракт: [docs/architecture/openapi/components/openapi.yaml](../../../docs/architecture/openapi/components/openapi.yaml)

## Requirements

### Requirement: Список домов

Система SHALL возвращать список домов (корпусов) ЖК.

#### Scenario: Успешное получение списка домов

- **WHEN** клиент отправляет GET /api/v1/buildings
- **THEN** статус ответа 200
- **AND** тело ответа содержит массив `items` с объектами дома (`id`, `name`, `address`)

#### Scenario: Сортировка домов по наименованию

- **WHEN** клиент отправляет GET /api/v1/buildings
- **THEN** элементы `items` отсортированы по полю `name` по возрастанию

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

### Requirement: Создание и изменение дома

Система SHALL позволять создавать и обновлять записи домов.

#### Scenario: Успешное создание дома

- **WHEN** клиент отправляет POST /api/v1/buildings с телом `{"name":"Корпус 1","address":"ул. Зелёная, 1"}`
- **THEN** статус ответа 201
- **AND** тело ответа `item` содержит `id`, `name`, `address`

#### Scenario: Ошибка валидации при создании дома

- **WHEN** клиент отправляет POST /api/v1/buildings с пустым `name`
- **THEN** статус ответа 400

#### Scenario: Успешное обновление дома

- **WHEN** клиент отправляет PUT /api/v1/buildings/{id} с валидными `name` и `address`
- **THEN** статус ответа 200
- **AND** тело ответа `item` отражает обновлённые значения

### Requirement: Создание и изменение квартиры

Система SHALL позволять создавать и обновлять квартиры в рамках существующего дома.

#### Scenario: Успешное создание квартиры

- **WHEN** клиент отправляет POST /api/v1/buildings/{buildingId}/apartments с телом `{"number":"12","floor":3}`
- **THEN** статус ответа 201
- **AND** тело ответа `item` содержит `id`, `buildingId`, `number`, `floor`

#### Scenario: Дубликат номера квартиры в доме

- **WHEN** клиент отправляет POST /api/v1/buildings/{buildingId}/apartments с `number`, уже существующим в этом доме
- **THEN** статус ответа 409

#### Scenario: Дом не найден при создании квартиры

- **WHEN** клиент отправляет POST /api/v1/buildings/{buildingId}/apartments для несуществующего дома
- **THEN** статус ответа 404

### Requirement: Назначение и изменение владельца квартиры

Система SHALL хранить владельца как отдельную сущность, связанную с квартирой (связь один-к-одному: одна квартира — один владелец).

#### Scenario: Успешное назначение владельца

- **WHEN** клиент отправляет PUT /api/v1/apartments/{apartmentId}/owner с телом `{"fullName":"Иванов Иван Иванович","phone":"+79001234567"}`
- **THEN** статус ответа 200
- **AND** тело ответа `item` содержит `id`, `apartmentId`, `fullName`, `phone`

#### Scenario: Ошибка валидации владельца

- **WHEN** клиент отправляет PUT /api/v1/apartments/{apartmentId}/owner с пустым `fullName`
- **THEN** статус ответа 400

#### Scenario: Квартира не найдена

- **WHEN** клиент отправляет PUT /api/v1/apartments/{apartmentId}/owner для несуществующей квартиры
- **THEN** статус ответа 404

### Requirement: Хранение сущностей в базе данных

Система SHALL персистировать дома, квартиры и владельцев в PostgreSQL.

#### Scenario: Структура таблицы домов

- **WHEN** применена миграция capability `directories`
- **THEN** в БД существует таблица `buildings` с колонками `Id` (uuid, PK), `Name` (varchar, NOT NULL), `Address` (varchar, nullable)

#### Scenario: Структура таблицы квартир

- **WHEN** применена миграция capability `directories`
- **THEN** в БД существует таблица `apartments` с колонками `Id` (uuid, PK), `BuildingId` (uuid, FK → `buildings`), `Number` (varchar, NOT NULL), `Floor` (int, nullable)
- **AND** уникальное ограничение на пару (`BuildingId`, `Number`)

#### Scenario: Структура таблицы владельцев

- **WHEN** применена миграция capability `directories`
- **THEN** в БД существует таблица `owners` с колонками `Id` (uuid, PK), `ApartmentId` (uuid, FK → `apartments`, UNIQUE), `FullName` (varchar, NOT NULL), `Phone` (varchar, nullable)

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
