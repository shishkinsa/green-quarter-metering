using System.Globalization;
using System.Text;
using Microsoft.EntityFrameworkCore.Migrations;

namespace GQ.WebApi.DataAccess.Postgres.Migrations;

/// <summary>
/// Демо-данные справочников (10 домов × 50 квартир) для data-миграции.
/// </summary>
internal static class DirectoryDemoData
{
    internal const int BuildingCount = 10;

    internal const int ApartmentsPerBuilding = 50;

    internal static void Insert(MigrationBuilder migrationBuilder)
    {
        var sql = new StringBuilder();

        for (var buildingIndex = 1; buildingIndex <= BuildingCount; buildingIndex++)
        {
            var buildingId = CreateBuildingId(buildingIndex);
            AppendInsert(
                sql,
                table: "buildings",
                conflictColumn: "\"Id\"",
                columns: "\"Id\", \"Name\", \"Address\"",
                values: $"'{buildingId}', '{Escape($"Корпус {buildingIndex}")}', '{Escape($"ул. Зелёная, {buildingIndex}")}'");

            for (var apartmentIndex = 1; apartmentIndex <= ApartmentsPerBuilding; apartmentIndex++)
            {
                var apartmentId = CreateApartmentId(buildingIndex, apartmentIndex);
                var floor = (apartmentIndex - 1) / 4 + 1;
                AppendInsert(
                    sql,
                    table: "apartments",
                    conflictColumn: "\"Id\"",
                    columns: "\"Id\", \"BuildingId\", \"Number\", \"Floor\"",
                    values: $"'{apartmentId}', '{buildingId}', '{Escape(apartmentIndex.ToString(CultureInfo.InvariantCulture))}', {floor}");

                if (apartmentIndex % 3 == 1)
                {
                    var ownerId = CreateOwnerId(buildingIndex, apartmentIndex);
                    var phone = $"+7900{buildingIndex:D2}{apartmentIndex:D6}"[..12];
                    AppendInsert(
                        sql,
                        table: "owners",
                        conflictColumn: "\"ApartmentId\"",
                        columns: "\"Id\", \"ApartmentId\", \"FullName\", \"Phone\"",
                        values: $"'{ownerId}', '{apartmentId}', '{Escape($"Житель {buildingIndex}-{apartmentIndex}")}', '{Escape(phone)}'");
                }
            }
        }

        migrationBuilder.Sql(sql.ToString());
    }

    internal static void Delete(MigrationBuilder migrationBuilder)
    {
        for (var buildingIndex = BuildingCount; buildingIndex >= 1; buildingIndex--)
        {
            for (var apartmentIndex = ApartmentsPerBuilding; apartmentIndex >= 1; apartmentIndex--)
            {
                if (apartmentIndex % 3 == 1)
                {
                    migrationBuilder.DeleteData(
                        table: "owners",
                        keyColumn: "Id",
                        keyValue: CreateOwnerId(buildingIndex, apartmentIndex));
                }

                migrationBuilder.DeleteData(
                    table: "apartments",
                    keyColumn: "Id",
                    keyValue: CreateApartmentId(buildingIndex, apartmentIndex));
            }

            migrationBuilder.DeleteData(
                table: "buildings",
                keyColumn: "Id",
                keyValue: CreateBuildingId(buildingIndex));
        }
    }

    private static void AppendInsert(
        StringBuilder sql,
        string table,
        string conflictColumn,
        string columns,
        string values)
    {
        sql.Append("INSERT INTO ")
            .Append(table)
            .Append(" (")
            .Append(columns)
            .Append(") VALUES (")
            .Append(values)
            .Append(") ON CONFLICT (")
            .Append(conflictColumn)
            .Append(") DO NOTHING;")
            .AppendLine();
    }

    private static string Escape(string value) => value.Replace("'", "''", StringComparison.Ordinal);

    private static Guid CreateBuildingId(int buildingIndex) =>
        Guid.Parse($"b0000001-0000-0000-0000-{buildingIndex:D12}", CultureInfo.InvariantCulture);

    private static Guid CreateApartmentId(int buildingIndex, int apartmentIndex) =>
        Guid.Parse($"c0000001-0000-0000-0000-{(buildingIndex * 1000L + apartmentIndex):D12}", CultureInfo.InvariantCulture);

    private static Guid CreateOwnerId(int buildingIndex, int apartmentIndex) =>
        Guid.Parse($"d0000001-0000-0000-0000-{(buildingIndex * 1000L + apartmentIndex):D12}", CultureInfo.InvariantCulture);
}
