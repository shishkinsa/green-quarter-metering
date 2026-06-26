param(
    [string]$Root
)

$ErrorActionPreference = 'Stop'

if ([string]::IsNullOrWhiteSpace($Root)) {
    $Root = Split-Path -Parent (Split-Path -Parent $MyInvocation.MyCommand.Path)
}

$Root = (Resolve-Path -LiteralPath $Root).Path

$requiredPaths = @(
    'docs\README.md',
    'manifest.yaml',
    'docs\requirements\index.yaml',
    'docs\requirements\business\goals.md',
    'docs\requirements\business\capabilities.md',
    'docs\requirements\constraints\performance.md',
    'docs\requirements\constraints\security.md',
    'docs\process\context\containers.md',
    'docs\architecture\planning\capacity.md',
    'docs\architecture\specs\assemblies.md',
    'docs\architecture\specs\backend.md',
    'docs\architecture\specs\frontend.md',
    'docs\standards\coding-standards.md',
    'docs\standards\git-flow.md',
    'openspec\specs\examples\spec.md',
    'openspec\specs\categories\spec.md',
    'openspec\specs\auth\spec.md',
    'docs\architecture\openapi\components\openapi.yaml'
)

$missing = @()
foreach ($rel in $requiredPaths) {
    $full = [System.IO.Path]::GetFullPath((Join-Path $Root ($rel -replace '/', [IO.Path]::DirectorySeparatorChar)))
    if (-not (Test-Path -LiteralPath $full)) {
        $missing += $rel
    }
}

if ($missing.Count -gt 0) {
    Write-Host 'Manifest check FAILED - missing paths:' -ForegroundColor Red
    $missing | ForEach-Object { Write-Host "  $_" }
    exit 1
}

$manifestPath = Join-Path $Root 'manifest.yaml'
$manifestContent = Get-Content -LiteralPath $manifestPath -Raw
$specRoot = Join-Path $Root 'openspec\specs'
$specDirs = Get-ChildItem -LiteralPath $specRoot -Directory | ForEach-Object { $_.Name }

foreach ($cap in $specDirs) {
    $pattern = '(?m)^\s+' + [regex]::Escape($cap) + '\s*:'
    if ($manifestContent -notmatch $pattern) {
        Write-Host "Manifest check FAILED - capability '$cap' in openspec/specs but not in manifest.yaml" -ForegroundColor Red
        exit 1
    }
}

Write-Host "Manifest check OK ($($requiredPaths.Count) paths, $($specDirs.Count) capabilities)" -ForegroundColor Green
exit 0
