using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SolicitudesApp.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class MigracionSemilla01 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "CreadoPor",
                table: "CentroCosto",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<DateTime>(
                name: "FechaCreacion",
                table: "CentroCosto",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<DateTime>(
                name: "FechaModificacion",
                table: "CentroCosto",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ModificadoPor",
                table: "CentroCosto",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.UpdateData(
                table: "CentroCosto",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "CreadoPor", "FechaCreacion", "FechaModificacion", "ModificadoPor" },
                values: new object[] { "seed", new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null });

            migrationBuilder.UpdateData(
                table: "CentroCosto",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "CreadoPor", "FechaCreacion", "FechaModificacion", "ModificadoPor" },
                values: new object[] { "seed", new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null });

            migrationBuilder.UpdateData(
                table: "CentroCosto",
                keyColumn: "Id",
                keyValue: 3,
                columns: new[] { "CreadoPor", "FechaCreacion", "FechaModificacion", "ModificadoPor" },
                values: new object[] { "seed", new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CreadoPor",
                table: "CentroCosto");

            migrationBuilder.DropColumn(
                name: "FechaCreacion",
                table: "CentroCosto");

            migrationBuilder.DropColumn(
                name: "FechaModificacion",
                table: "CentroCosto");

            migrationBuilder.DropColumn(
                name: "ModificadoPor",
                table: "CentroCosto");
        }
    }
}
