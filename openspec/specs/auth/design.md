# Auth — Technical Design

Скелет аутентификации: опциональная защита мутаций без привязки к конкретному IdP.

ADR: [docs/architecture/adr/0008-jwt-authentication-skeleton.md](../../../docs/architecture/adr/0008-jwt-authentication-skeleton.md)

## Backend

- **Config**: `Auth:Enabled`, `Auth:Authority`, `Auth:Audience` в `appsettings.json`
- **Policy**: `AuthenticatedWhenEnabled` — пропускает запросы при `Enabled=false`
- **Development**: схема `DevBearer` — любой `Authorization: Bearer {token}`
- **Production**: JWT Bearer с `Authority` / `Audience`
- **Защищённый endpoint**: POST `/api/v1/examples` (`[Authorize]`)

## Frontend

- `shared/auth/token.ts` — `VITE_AUTH_ENABLED`, dev-токен в sessionStorage
- `shared/auth/ui/AuthStubBanner.tsx` — напоминание в UI
- `shared/api/http.ts` — подстановка Bearer в заголовок

## Tests

Интеграционные тесты auth в шаблоне не включены — добавьте в форке после выбора IdP.

## Ручная проверка

```powershell
# Backend
$env:Auth__Enabled = "true"
dotnet run --project "src/webapi/cnt_gq_webapi/6 WebApp/GQ.WebApi.WebApp.csproj"

# Frontend (.env)
VITE_AUTH_ENABLED=true
npm run dev
```

POST без токена → 401; с `Bearer dev-token` → 201.

## Traceability

| Requirement | OpenAPI | Backend | Frontend |
|-------------|---------|---------|----------|
| Auth disabled | — | `Auth:Enabled=false` | `VITE_AUTH_ENABLED` unset |
| Auth enabled 401 | POST 401 | `AuthWhenEnabledHandler` | — |
| Dev bearer | — | `DevBearerAuthenticationHandler` | `getAuthToken()` |
