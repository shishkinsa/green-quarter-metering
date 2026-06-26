# cnt_gq_web — карта FSD-слоёв

SPA «Зелёный квартал — Учёт электроэнергии». Стек: Vue 3, TypeScript, Element Plus, Vue Router.

## Навигация

| Маршрут | Page | Описание |
|---------|------|----------|
| `/` | `pages/buildings` | Таблица домов |
| `/buildings/:buildingId/apartments` | `pages/apartments` | Квартиры дома |
| `/apartments/:apartmentId/meter-readings` | `pages/apartment-meter-readings` | История показаний |

## Слои

```
app/          — main.ts, router.ts, App.vue, глобальные стили
pages/        — композиция экранов (тонкие, без прямых HTTP)
widgets/      — app-layout, app-breadcrumbs
features/     — модальные формы и сценарные действия (create/update/delete/submit)
entities/     — building, meter-reading: API, composables, presentation tables
shared/       — api (http, errors), lib (notifyApiError)
```

## Правила импорта

- Импорт только **вниз** по слоям FSD.
- Слайсы импортируются через **public API** (`index.ts`), не через deep paths.
- Типы DTO — из `@/shared/api/generated/openapi` (после `npm run generate:api`).

## Команды

```bash
npm run dev          # dev-сервер
npm run build        # production-сборка
npm run lint         # ESLint
npm run generate:api # типы из OpenAPI
```
