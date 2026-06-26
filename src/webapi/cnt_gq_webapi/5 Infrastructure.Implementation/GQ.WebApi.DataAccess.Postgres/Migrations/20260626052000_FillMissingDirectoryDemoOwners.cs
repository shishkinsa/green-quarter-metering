using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace GQ.WebApi.DataAccess.Postgres.Migrations;

/// <inheritdoc />
public partial class FillMissingDirectoryDemoOwners: Migration
{
    /// <inheritdoc />
    protected override void Up(MigrationBuilder migrationBuilder)
    {
        DirectoryDemoData.FillMissingOwners(migrationBuilder);
    }

    /// <inheritdoc />
    protected override void Down(MigrationBuilder migrationBuilder)
    {
        DirectoryDemoData.DeleteFilledOwners(migrationBuilder);
    }
}
