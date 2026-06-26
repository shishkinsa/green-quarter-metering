using System;

using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace GQ.WebApi.DataAccess.Postgres.Migrations;

/// <inheritdoc />
public partial class AddBuildingsApartmentsOwners: Migration
{
    /// <inheritdoc />
    protected override void Up(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.CreateTable(
            name: "buildings",
            columns: table => new
            {
                Id = table.Column<Guid>(type: "uuid", nullable: false),
                Name = table.Column<string>(type: "character varying(256)", maxLength: 256, nullable: false),
                Address = table.Column<string>(type: "character varying(512)", maxLength: 512, nullable: true)
            },
            constraints: table => table.PrimaryKey("PK_buildings", x => x.Id));

        migrationBuilder.CreateTable(
            name: "apartments",
            columns: table => new
            {
                Id = table.Column<Guid>(type: "uuid", nullable: false),
                BuildingId = table.Column<Guid>(type: "uuid", nullable: false),
                Number = table.Column<string>(type: "character varying(32)", maxLength: 32, nullable: false),
                Floor = table.Column<int>(type: "integer", nullable: true)
            },
            constraints: table =>
            {
                table.PrimaryKey("PK_apartments", x => x.Id);
                table.ForeignKey(
                    name: "FK_apartments_buildings_BuildingId",
                    column: x => x.BuildingId,
                    principalTable: "buildings",
                    principalColumn: "Id",
                    onDelete: ReferentialAction.Restrict);
            });

        migrationBuilder.CreateTable(
            name: "owners",
            columns: table => new
            {
                Id = table.Column<Guid>(type: "uuid", nullable: false),
                ApartmentId = table.Column<Guid>(type: "uuid", nullable: false),
                FullName = table.Column<string>(type: "character varying(256)", maxLength: 256, nullable: false),
                Phone = table.Column<string>(type: "character varying(32)", maxLength: 32, nullable: true)
            },
            constraints: table =>
            {
                table.PrimaryKey("PK_owners", x => x.Id);
                table.ForeignKey(
                    name: "FK_owners_apartments_ApartmentId",
                    column: x => x.ApartmentId,
                    principalTable: "apartments",
                    principalColumn: "Id",
                    onDelete: ReferentialAction.Cascade);
            });

        migrationBuilder.CreateIndex(
            name: "IX_apartments_BuildingId_Number",
            table: "apartments",
            columns: new[] { "BuildingId", "Number" },
            unique: true);

        migrationBuilder.CreateIndex(
            name: "IX_owners_ApartmentId",
            table: "owners",
            column: "ApartmentId",
            unique: true);
    }

    /// <inheritdoc />
    protected override void Down(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.DropTable(
            name: "owners");

        migrationBuilder.DropTable(
            name: "apartments");

        migrationBuilder.DropTable(
            name: "buildings");
    }
}
