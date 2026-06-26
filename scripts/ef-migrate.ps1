param(
    [ValidateSet('update', 'add', 'remove')]
    [string]$Action = 'update',

    [string]$MigrationName = 'NewMigration'
)

$ErrorActionPreference = 'Stop'
$root = Split-Path -Parent (Split-Path -Parent $MyInvocation.MyCommand.Path)
$dataProject = Join-Path $root 'src\webapi\cnt_gq_webapi\5 Infrastructure.Implementation\GQ.WebApi.DataAccess.Postgres\GQ.WebApi.DataAccess.Postgres.csproj'
$webProject = Join-Path $root 'src\webapi\cnt_gq_webapi\6 WebApp\GQ.WebApi.WebApp.csproj'

switch ($Action) {
    'update' {
        dotnet ef database update --project $dataProject --startup-project $webProject
    }
    'add' {
        dotnet ef migrations add $MigrationName --project $dataProject --startup-project $webProject --output-dir Migrations
    }
    'remove' {
        dotnet ef migrations remove --project $dataProject --startup-project $webProject
    }
}
