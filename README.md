# Зелёный квартал — Учёт электроэнергии

Система учёта электроэнергии для жилого комплекса **«Зелёный квартал»**.  
Ответственный за систему — **энергетик Василий**.

## Архитектура

| Слой | Технологии | Порт (dev) |
|------|------------|------------|
| **Backend API** | ASP.NET Core 8, Clean Architecture, MediatR, EF Core, PostgreSQL | `5025` |
| **Frontend** | Vue 3, Element Plus, Vite, FSD | `5173` |

```
Браузер → Frontend (Vue SPA) → Backend API → PostgreSQL
```

## Быстрый старт

### Требования

- [.NET SDK 8](https://dotnet.microsoft.com/download)
- [Node.js 22+](https://nodejs.org/)
- PostgreSQL 16+ (опционально на старте)

### Локальный запуск

```powershell
npm install   # OpenSpec CLI

# Backend
dotnet run --project "src/webapi/cnt_gq_webapi/6 WebApp/GQ.WebApi.WebApp.csproj"

# Frontend (в отдельном терминале, прокси /api → backend)
cd src/frontend/cnt_gq_web
npm install
npm run dev
```

- Backend Swagger: http://localhost:5025/swagger  
- Frontend (dev): http://localhost:5173  

### Docker Compose

```powershell
# Скопируйте переменные окружения (опционально)
copy .env.example .env

# Сборка и запуск: postgres + backend + frontend
docker compose up --build

# В фоне
docker compose up --build -d

# Остановка
docker compose down
```

| Сервис | URL |
|--------|-----|
| Frontend (UI) | http://localhost:5173 |
| Backend Swagger | http://localhost:5025/swagger |
| Backend health | http://localhost:5025/api/v1/health |
| PostgreSQL | `localhost:5432` (БД `cnt_gq_db`) |

Переменные портов и пароля — в `.env` (см. `.env.example`). Миграции EF применяются автоматически (`DATABASE_AUTO_MIGRATE=true`).

**Мониторинг (опционально):** `docker compose -f docker-compose.monitoring.yml up -d` или `.\scripts\start-monitoring.ps1`

## Документация

Карта документации: [docs/README.md](docs/README.md) · [manifest.yaml](manifest.yaml) · [AGENTS.md](AGENTS.md)

## Лицензия

Apache License 2.0 — см. [LICENSE](LICENSE).
