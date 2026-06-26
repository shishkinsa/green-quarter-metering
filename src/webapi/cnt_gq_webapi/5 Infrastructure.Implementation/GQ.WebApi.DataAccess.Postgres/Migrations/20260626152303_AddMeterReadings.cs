using System;

using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace GQ.WebApi.DataAccess.Postgres.Migrations;

/// <inheritdoc />
public partial class AddMeterReadings: Migration
{
    /// <inheritdoc />
    protected override void Up(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.CreateTable(
            name: "meter_readings",
            columns: table => new
            {
                Id = table.Column<Guid>(type: "uuid", nullable: false),
                ApartmentId = table.Column<Guid>(type: "uuid", nullable: false),
                PeriodYear = table.Column<int>(type: "integer", nullable: false),
                PeriodMonth = table.Column<int>(type: "integer", nullable: false),
                Value = table.Column<decimal>(type: "numeric(18,3)", nullable: false, precision: 18, scale: 3)
            },
            constraints: table =>
            {
                table.PrimaryKey("PK_meter_readings", x => x.Id);
                table.ForeignKey(
                    name: "FK_meter_readings_apartments_ApartmentId",
                    column: x => x.ApartmentId,
                    principalTable: "apartments",
                    principalColumn: "Id",
                    onDelete: ReferentialAction.Restrict);
            });

        migrationBuilder.CreateIndex(
            name: "IX_meter_readings_ApartmentId_PeriodYear_PeriodMonth",
            table: "meter_readings",
            columns: new[] { "ApartmentId", "PeriodYear", "PeriodMonth" },
            unique: true);
    }

    /// <inheritdoc />
    protected override void Down(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.DropTable(
            name: "meter_readings");
    }
}
