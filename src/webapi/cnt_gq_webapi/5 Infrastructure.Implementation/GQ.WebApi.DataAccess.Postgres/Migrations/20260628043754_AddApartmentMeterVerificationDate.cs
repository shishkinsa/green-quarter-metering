using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace GQ.WebApi.DataAccess.Postgres.Migrations;

/// <inheritdoc />
public partial class AddApartmentMeterVerificationDate : Migration
{
    /// <inheritdoc />
    protected override void Up(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.AddColumn<DateOnly>(
            name: "MeterVerificationDate",
            table: "apartments",
            type: "date",
            nullable: true);
    }

    /// <inheritdoc />
    protected override void Down(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.DropColumn(
            name: "MeterVerificationDate",
            table: "apartments");
    }
}
