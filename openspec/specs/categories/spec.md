---
capability: categories
business: docs/requirements/business/capabilities.md#categories
openapi: docs/architecture/openapi/components/openapi.yaml
adr: []
---

## Purpose

Read-only справочник категорий — второй эталон capability для демонстрации add-entity workflow без мутаций.

REST-контракт: [docs/architecture/openapi/components/openapi.yaml](../../../docs/architecture/openapi/components/openapi.yaml)

## Requirements

### Requirement: List Categories

The system SHALL expose a read-only list of reference categories.

#### Scenario: List categories

- **WHEN** a client sends GET /api/v1/categories
- **THEN** the response status is 200
- **AND** the response body contains an `items` array with at least one category

#### Scenario: Categories are ordered by code

- **WHEN** a client sends GET /api/v1/categories
- **THEN** items are sorted by `code` ascending

### Requirement: Display Categories In SPA

The SPA SHALL display the category list on the home page.

#### Scenario: Categories visible on home

- **WHEN** a user opens the home page
- **THEN** categories from GET /api/v1/categories are shown in a table
