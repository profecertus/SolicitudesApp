using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace SolicitudesApp.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class MigracionSemilla : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "CentroCosto",
                columns: new[] { "Id", "Activo", "Codigo", "Nombre" },
                values: new object[,]
                {
                    { 1, true, "CC-TI-01", "Tecnologías de la Información" },
                    { 2, true, "CC-OPS-02", "Operaciones y Logística" },
                    { 3, true, "CC-VEN-03", "Ventas y Marketing" }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "CentroCosto",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "CentroCosto",
                keyColumn: "Id",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "CentroCosto",
                keyColumn: "Id",
                keyValue: 3);
        }
    }
}
