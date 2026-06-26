# ADR-0008: JWT authentication skeleton

## 📋 Метаданные

| Атрибут | Значение |
|---------|----------|
| **Статус** | Принято |
| **Дата** | 2026-06-23 |
| **Автор** | Зеленый квартал — Учёт электроэнергии Team |
| **Версия** | 1.0 |

## 🎯 Контекст

Enterprise API требует аутентификации мутаций, но шаблон должен работать без IdP на старте.

## ✅ Решение

- Флаг `Auth:Enabled` (по умолчанию `false`)
- Политика `AuthenticatedWhenEnabled` на защищённых эндпоинтах (POST `/examples`)
- **Development**: схема `DevBearer` — любой `Authorization: Bearer {token}`
- **Production**: JWT Bearer с `Auth:Authority` и `Auth:Audience`
- Frontend: `VITE_AUTH_ENABLED`, токен в `shared/auth/token.ts`

## 📎 Последствия

- OpenSpec capability: `openspec/specs/auth/spec.md`
- Полная интеграция OIDC — отдельный change при выборе IdP
- См. [NFR-002](../../requirements/constraints/security.md)
