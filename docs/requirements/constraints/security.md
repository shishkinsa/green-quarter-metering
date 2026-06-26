---
id: NFR-002
type: constraint
status: draft
---

# Безопасность API

Публичные эндпоинты шаблона не раскрывают внутренние детали ошибок; секреты не хранятся в репозитории.

- Строки подключения и пароли — только в `appsettings.Development.json` / переменных окружения, не в git
- Ошибки API — формат Problem Details (`ApiExceptionHandler`)
- Полная интеграция OIDC — отдельный change; скелет: ADR-0008, `Auth:Enabled`, capability `auth`
