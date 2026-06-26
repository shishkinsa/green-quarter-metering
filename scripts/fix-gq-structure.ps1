$ErrorActionPreference = 'Stop'
$root = Split-Path -Parent (Split-Path -Parent $MyInvocation.MyCommand.Path)

$spWebapi = Join-Path $root 'src\webapi\cnt_sp_webapi'
if (Test-Path $spWebapi) {
    Remove-Item -Recurse -Force $spWebapi
}

$renames = @(
    @{ Path = 'src\lib\SP.Shared.Observability'; NewName = 'GQ.Shared.Observability' },
    @{ Path = 'src\webapi\cnt_gq_webapi\0 Utils\SP.WebApi.Utils'; NewName = 'GQ.WebApi.Utils' },
    @{ Path = 'src\webapi\cnt_gq_webapi\1 Entities\SP.WebApi.Entities'; NewName = 'GQ.WebApi.Entities' },
    @{ Path = 'src\webapi\cnt_gq_webapi\2 Infrastructure.Interfaces\SP.WebApi.Infrastructure.Interfaces'; NewName = 'GQ.WebApi.Infrastructure.Interfaces' },
    @{ Path = 'src\webapi\cnt_gq_webapi\3 UseCases\SP.WebApi.UseCases'; NewName = 'GQ.WebApi.UseCases' },
    @{ Path = 'src\webapi\cnt_gq_webapi\5 Infrastructure.Implementation\SP.WebApi.DataAccess.Postgres'; NewName = 'GQ.WebApi.DataAccess.Postgres' },
    @{ Path = 'src\webapi\cnt_gq_webapi\6 WebApp\SP.WebApi.WebApp.csproj'; NewName = 'GQ.WebApi.WebApp.csproj' },
    @{ Path = 'src\webapi\cnt_gq_webapi\7 Tests\SP.WebApi.Tests'; NewName = 'GQ.WebApi.Tests' }
)

foreach ($item in $renames) {
    $path = Join-Path $root $item.Path
    if (-not (Test-Path $path)) { continue }
    $parent = Split-Path $path -Parent
    $newPath = Join-Path $parent $item.NewName
    if (Test-Path $newPath) { continue }
    Rename-Item -LiteralPath $path -NewName $item.NewName
}

$csprojRenames = @(
    'src\lib\GQ.Shared.Observability\SP.Shared.Observability.csproj',
    'src\webapi\cnt_gq_webapi\0 Utils\GQ.WebApi.Utils\SP.WebApi.Utils.csproj',
    'src\webapi\cnt_gq_webapi\1 Entities\GQ.WebApi.Entities\SP.WebApi.Entities.csproj',
    'src\webapi\cnt_gq_webapi\2 Infrastructure.Interfaces\GQ.WebApi.Infrastructure.Interfaces\SP.WebApi.Infrastructure.Interfaces.csproj',
    'src\webapi\cnt_gq_webapi\3 UseCases\GQ.WebApi.UseCases\SP.WebApi.UseCases.csproj',
    'src\webapi\cnt_gq_webapi\5 Infrastructure.Implementation\GQ.WebApi.DataAccess.Postgres\SP.WebApi.DataAccess.Postgres.csproj',
    'src\webapi\cnt_gq_webapi\7 Tests\GQ.WebApi.Tests\SP.WebApi.Tests.csproj'
)

foreach ($rel in $csprojRenames) {
    $path = Join-Path $root $rel
    if (-not (Test-Path $path)) { continue }
    $newName = [System.IO.Path]::GetFileName($path).Replace('SP.', 'GQ.')
    Rename-Item -LiteralPath $path -NewName $newName
}

Write-Host 'Structure fixed.' -ForegroundColor Green
