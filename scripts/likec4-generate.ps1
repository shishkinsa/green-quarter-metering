$ErrorActionPreference = 'Stop'
$root = Split-Path -Parent (Split-Path -Parent $MyInvocation.MyCommand.Path)
$diagram = Join-Path $root 'docs\architecture\diagram'
$generated = Join-Path $diagram 'generated'

Push-Location $root
try {
    if (-not (Test-Path 'node_modules')) {
        npm install --silent
    }

    Write-Host '=== LikeC4: validate ===' -ForegroundColor Cyan
    npx likec4 validate $diagram
    if ($LASTEXITCODE -ne 0) { throw 'LikeC4 validation failed' }

    Write-Host '=== LikeC4: mermaid + dot + json ===' -ForegroundColor Cyan
    npx likec4 gen mermaid -o (Join-Path $generated 'mermaid') $diagram
    if ($LASTEXITCODE -ne 0) { throw 'LikeC4 mermaid export failed' }

    npx likec4 gen dot -o (Join-Path $generated 'dot') $diagram
    if ($LASTEXITCODE -ne 0) { throw 'LikeC4 dot export failed' }

    npx likec4 export json -o $generated $diagram
    if ($LASTEXITCODE -ne 0) { throw 'LikeC4 json export failed' }

    Write-Host '=== LikeC4: static site ===' -ForegroundColor Cyan
    npx likec4 build -o (Join-Path $generated 'site') -t 'Зелёный квартал — C4' $diagram
    if ($LASTEXITCODE -ne 0) { throw 'LikeC4 build failed' }

    Write-Host '=== LikeC4: PNG (optional) ===' -ForegroundColor Cyan
    npx likec4 export png --flat -o (Join-Path $generated 'png') $diagram
    if ($LASTEXITCODE -ne 0) {
        Write-Host 'PNG export skipped (install Playwright: npx playwright install chromium)' -ForegroundColor Yellow
    }

    Write-Host '=== LikeC4 generate OK ===' -ForegroundColor Green
} finally {
    Pop-Location
}
