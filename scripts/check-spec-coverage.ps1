$ErrorActionPreference = 'Stop'
$root = Split-Path -Parent (Split-Path -Parent $MyInvocation.MyCommand.Path)
$testsRoot = Join-Path $root 'src\webapi\cnt_gq_webapi\7 Tests\GQ.WebApi.Tests'
$specsRoot = Join-Path $root 'openspec\specs'

$coverageFiles = Get-ChildItem -Path $specsRoot -Recurse -Filter 'scenario-coverage.txt'
if ($coverageFiles.Count -eq 0) {
    throw "No scenario-coverage.txt files under openspec/specs"
}

$testSource = Get-ChildItem -Path $testsRoot -Recurse -Filter '*.cs' |
    ForEach-Object { Get-Content $_.FullName -Raw } |
    Out-String

$missing = @()
$mapped = 0

foreach ($coverageFile in $coverageFiles) {
    $capability = Split-Path (Split-Path $coverageFile.FullName -Parent) -Leaf
    Write-Host "  [$capability]" -ForegroundColor DarkGray

    Get-Content $coverageFile.FullName | ForEach-Object {
        $line = $_.Trim()
        if ($line -eq '' -or $line.StartsWith('#')) { return }

        $parts = $line -split '\|', 2
        if ($parts.Count -ne 2) {
            throw "Invalid coverage line in $($coverageFile.Name): $line"
        }

        $scenario = $parts[0].Trim()
        $testMethod = $parts[1].Trim()
        $mapped++

        $pattern = "(Task|void)\s+$([regex]::Escape($testMethod))\s*\("
        if ($testSource -notmatch $pattern) {
            $missing += "[$capability] $scenario -> $testMethod"
        }
    }
}

if ($missing.Count -gt 0) {
    Write-Host "Missing tests for OpenSpec scenarios:" -ForegroundColor Red
    $missing | ForEach-Object { Write-Host "  $_" }
    throw "Spec coverage check failed ($($missing.Count) missing)"
}

Write-Host "Spec coverage OK: $mapped scenarios across $($coverageFiles.Count) capabilities" -ForegroundColor Green
