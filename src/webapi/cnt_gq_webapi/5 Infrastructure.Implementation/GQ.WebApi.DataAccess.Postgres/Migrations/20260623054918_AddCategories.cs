using System;

using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace GQ.WebApi.DataAccess.Postgres.Migrations;

/// <inheritdoc />
public partial class AddCategories: Migration
{
    /// <inheritdoc />
    protected override void Up(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.CreateTable(
            name: "categories",
            columns: table => new
            {
                Id = table.Column<Guid>(type: "uuid", nullable: false),
                Code = table.Column<string>(type: "character varying(64)", maxLength: 64, nullable: false),
                Name = table.Column<string>(type: "character varying(256)", maxLength: 256, nullable: false)
            },
            constraints: table => table.PrimaryKey("PK_categories", x => x.Id));
    }

    /// <inheritdoc />
    protected override void Down(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.DropTable(
            name: "categories");
    }
}
