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
                    values: $"'{apartmentId}', '{buildingId}', '{Escape(FormatApartmentNumber(floor, apartmentIndex))}', {floor}");

                AppendOwnerInsert(sql, buildingIndex, apartmentIndex, apartmentId);
            }
        }

        migrationBuilder.Sql(sql.ToString());
    }

    internal static void FillMissingOwners(MigrationBuilder migrationBuilder)
    {
        var sql = new StringBuilder();

        for (var buildingIndex = 1; buildingIndex <= BuildingCount; buildingIndex++)
        {
            for (var apartmentIndex = 1; apartmentIndex <= ApartmentsPerBuilding; apartmentIndex++)
            {
                AppendOwnerInsert(
                    sql,
                    buildingIndex,
                    apartmentIndex,
                    CreateApartmentId(buildingIndex, apartmentIndex));
            }
        }

        migrationBuilder.Sql(sql.ToString());
    }

    internal static void DeleteFilledOwners(MigrationBuilder migrationBuilder)
    {
        for (var buildingIndex = BuildingCount; buildingIndex >= 1; buildingIndex--)
        {
            for (var apartmentIndex = ApartmentsPerBuilding; apartmentIndex >= 1; apartmentIndex--)
            {
                if (apartmentIndex % 3 == 1)
                {
                    continue;
                }

                migrationBuilder.DeleteData(
                    table: "owners",
                    keyColumn: "Id",
                    keyValue: CreateOwnerId(buildingIndex, apartmentIndex));
            }
        }
    }

    internal static void Delete(MigrationBuilder migrationBuilder)
    {
        for (var buildingIndex = BuildingCount; buildingIndex >= 1; buildingIndex--)
        {
            for (var apartmentIndex = ApartmentsPerBuilding; apartmentIndex >= 1; apartmentIndex--)
            {
                migrationBuilder.DeleteData(
                    table: "owners",
                    keyColumn: "Id",
                    keyValue: CreateOwnerId(buildingIndex, apartmentIndex));

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

    private static void AppendOwnerInsert(
        StringBuilder sql,
        int buildingIndex,
        int apartmentIndex,
        Guid apartmentId)
    {
        var ownerId = CreateOwnerId(buildingIndex, apartmentIndex);
        var phone = FormatOwnerPhone(buildingIndex, apartmentIndex);
        AppendInsert(
            sql,
            table: "owners",
            conflictColumn: "\"ApartmentId\"",
            columns: "\"Id\", \"ApartmentId\", \"FullName\", \"Phone\"",
            values: $"'{ownerId}', '{apartmentId}', '{Escape(FormatOwnerName(buildingIndex, apartmentIndex))}', '{Escape(phone)}'");
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

    private static string FormatApartmentNumber(int floor, int apartmentIndex)
    {
        return $"{floor}{apartmentIndex % 4 + 1:D2}";
    }

    private static string FormatOwnerName(int buildingIndex, int apartmentIndex)
    {
        return $"Житель {buildingIndex}-{apartmentIndex}";
    }

    private static string FormatOwnerPhone(int buildingIndex, int apartmentIndex)
    {
        return $"+7900{buildingIndex:D2}{apartmentIndex:D6}"[..12];
    }

    private static string Escape(string value)
    {
        return value.Replace("'", "''", StringComparison.Ordinal);
    }

    private static Guid CreateBuildingId(int buildingIndex)
    {
        return Guid.Parse($"b0000001-0000-0000-0000-{buildingIndex:D12}", CultureInfo.InvariantCulture);
    }

    private static Guid CreateApartmentId(int buildingIndex, int apartmentIndex)
    {
        return Guid.Parse($"c0000001-0000-0000-0000-{(buildingIndex * 1000L + apartmentIndex):D12}", CultureInfo.InvariantCulture);
    }

    private static Guid CreateOwnerId(int buildingIndex, int apartmentIndex)
    {
        return Guid.Parse($"d0000001-0000-0000-0000-{(buildingIndex * 1000L + apartmentIndex):D12}", CultureInfo.InvariantCulture);
    }
}
