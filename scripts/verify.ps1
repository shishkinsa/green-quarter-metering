$ErrorActionPreference = 'Stop'
$root = Split-Path -Parent (Split-Path -Parent $MyInvocation.MyCommand.Path)

Write-Host "=== OpenSpec: validate specs ===" -ForegroundColor Cyan
Push-Location $root
try {
    if (Test-Path 'package.json') {
        if (-not (Test-Path 'node_modules')) {
            npm install --silent 2>$null
        }
        npx openspec validate --specs --strict --no-interactive
        if ($LASTEXITCODE -ne 0) { throw "OpenSpec specs validation failed" }
        npx openspec validate --changes --strict --no-interactive
        if ($LASTEXITCODE -ne 0) { throw "OpenSpec changes validation failed" }
    }
} finally {
    Pop-Location
}

Write-Host "=== Backend: build & test ===" -ForegroundColor Cyan
$webProject = Join-Path $root 'src\webapi\cnt_gq_webapi\6 WebApp\GQ.WebApi.WebApp.csproj'
$testProject = Join-Path $root 'src\webapi\cnt_gq_webapi\7 Tests\GQ.WebApi.Tests\GQ.WebApi.Tests.csproj'

dotnet test $testProject --verbosity minimal
if ($LASTEXITCODE -ne 0) { throw "Backend tests failed" }

Write-Host "=== Manifest (SPDF index) ===" -ForegroundColor Cyan
& (Join-Path $root 'scripts\check-manifest.ps1') -Root $root
if ($LASTEXITCODE -ne 0) { throw "Manifest check failed" }

Write-Host "=== Spec coverage (API scenarios) ===" -ForegroundColor Cyan
& (Join-Path $root 'scripts\check-spec-coverage.ps1')

Write-Host "=== Frontend: lint & build ===" -ForegroundColor Cyan
$frontendDir = Join-Path $root 'src\frontend\cnt_gq_web'
Push-Location $frontendDir
try {
    if (-not (Test-Path 'node_modules')) {
        npm install
    }
    npm run lint
    if ($LASTEXITCODE -ne 0) { throw "Frontend lint failed" }
    npm run test
    if ($LASTEXITCODE -ne 0) { throw "Frontend tests failed" }
    npm run build
    if ($LASTEXITCODE -ne 0) { throw "Frontend build failed" }
} finally {
    Pop-Location
}

Write-Host "=== Verify OK ===" -ForegroundColor Green
