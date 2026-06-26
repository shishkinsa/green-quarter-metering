$ErrorActionPreference = 'Stop'
$root = Split-Path -Parent (Split-Path -Parent $MyInvocation.MyCommand.Path)

Write-Host "Starting monitoring stack..." -ForegroundColor Cyan
docker compose -f (Join-Path $root 'docker-compose.monitoring.yml') up -d

Write-Host ""
Write-Host "Grafana:    http://localhost:3000  (admin / admin)" -ForegroundColor Green
Write-Host "Prometheus: http://localhost:9090" -ForegroundColor Green
Write-Host "OTLP:       localhost:4317 (gRPC), localhost:4318 (HTTP)" -ForegroundColor Green
