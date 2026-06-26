---
capability: auth
business: docs/requirements/business/capabilities.md#auth
openapi: docs/architecture/openapi/components/openapi.yaml
adr: ["0008"]
---

## Purpose

Скелет аутентификации API: JWT Bearer (production) и Dev Bearer (локальная разработка). Защита мутаций включается флагом `Auth:Enabled`.

ADR: [docs/architecture/adr/0008-jwt-authentication-skeleton.md](../../../docs/architecture/adr/0008-jwt-authentication-skeleton.md)

## Requirements

> Интеграционные тесты auth в шаблоне не включены — проверяйте вручную при `Auth:Enabled=true` или добавьте тесты в форке после выбора IdP.

### Requirement: Optional API Authentication

The system SHALL support optional JWT authentication controlled by configuration `Auth:Enabled`.

#### Scenario: Auth disabled

- **WHEN** `Auth:Enabled` is false
- **THEN** protected endpoints accept requests without authentication

#### Scenario: Auth enabled without token

- **WHEN** `Auth:Enabled` is true
- **AND** a client sends POST /api/v1/examples without Authorization header
- **THEN** the response status is 401

#### Scenario: Auth enabled with dev bearer token

- **WHEN** `Auth:Enabled` is true in Development
- **AND** a client sends POST /api/v1/examples with header `Authorization: Bearer {token}`
- **THEN** the response status is 201
