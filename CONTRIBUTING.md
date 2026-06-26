# Участие в разработке

## Порядок изменений (OpenSpec + docs-first)

1. OpenSpec change → delta specs → validate
2. Архитектура → ADR, LikeC4, OpenAPI
3. Код → `src/`
4. Тесты + `.\scripts\verify.ps1`
5. Archive → merge в `openspec/specs/`

Для новых сущностей: schema `full-stack` — [openspec/schemas/full-stack/](openspec/schemas/full-stack/).

## Перед Pull Request

```powershell
npm install
.\scripts\verify.ps1
```

Чеклист:

- [ ] OpenSpec specs/changes валидны (`npx openspec validate --all --strict --no-interactive`)
- [ ] OpenAPI синхронизирован с кодом и delta specs
- [ ] `dotnet test` проходит
- [ ] `npm run lint` и `npm run build` проходят
- [ ] ADR добавлен при архитектурных решениях

## Ветки

См. [docs/standards/git-flow.md](docs/standards/git-flow.md): `feature/*`, `fix/*`, `docs/*`.

## Инициализация нового проекта из шаблона

```powershell
.\scripts\init-project.ps1 -ProjectName "My Project" -ProjectPrefix "MP" -ProjectSlug "my-project"
npm install
npx openspec list --specs
```

Dry-run:

```powershell
.\scripts\init-project.ps1 -ProjectName "My Project" -ProjectPrefix "MP" -ProjectSlug "my-project" -WhatIf
```
