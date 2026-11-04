using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace SolicitudesApp.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class SemillasDatosPrueba : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "CreadoPor",
                table: "MovimientoTesoreria",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<DateTime>(
                name: "FechaCreacion",
                table: "MovimientoTesoreria",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<DateTime>(
                name: "FechaModificacion",
                table: "MovimientoTesoreria",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ModificadoPor",
                table: "MovimientoTesoreria",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.InsertData(
                table: "Usuarios",
                columns: new[] { "Id", "Activo", "Correo", "CreadoPor", "FechaCreacion", "FechaModificacion", "ModificadoPor", "Nombre", "PasswordHash" },
                values: new object[,]
                {
                    { 1, true, "ana.torres@solicitudes.com", "seed", new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, "Ana Torres", "seed-sin-autenticacion" },
                    { 2, true, "bruno.diaz@solicitudes.com", "seed", new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, "Bruno Diaz", "seed-sin-autenticacion" },
                    { 3, true, "carla.ruiz@solicitudes.com", "seed", new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, "Carla Ruiz", "seed-sin-autenticacion" },
                    { 4, true, "diego.paz@solicitudes.com", "seed", new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, "Diego Paz", "seed-sin-autenticacion" },
                    { 5, true, "elena.soto@solicitudes.com", "seed", new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, "Elena Soto", "seed-sin-autenticacion" }
                });

            migrationBuilder.InsertData(
                table: "Solicitudes",
                columns: new[] { "Id", "CentroCostoId", "CreadoPor", "Estado", "FechaCreacion", "FechaModificacion", "FechaRequerida", "ModificadoPor", "MontoTotal", "Motivo", "SolicitanteId" },
                values: new object[,]
                {
                    { 1, 1, "seed", 1, new DateTime(2026, 10, 1, 9, 0, 0, 0, DateTimeKind.Utc), null, new DateOnly(2026, 12, 15), null, 7000m, "Compra de laptops para el equipo de desarrollo", 1 },
                    { 2, 3, "seed", 2, new DateTime(2026, 10, 2, 9, 0, 0, 0, DateTimeKind.Utc), null, new DateOnly(2026, 11, 30), null, 1500m, "Material publicitario para campaña de fin de año", 5 },
                    { 3, 2, "seed", 3, new DateTime(2026, 10, 3, 9, 0, 0, 0, DateTimeKind.Utc), null, new DateOnly(2026, 11, 20), null, 4800m, "Mantenimiento preventivo de montacargas", 1 },
                    { 4, 1, "seed", 4, new DateTime(2026, 10, 4, 9, 0, 0, 0, DateTimeKind.Utc), null, new DateOnly(2026, 11, 25), null, 1200m, "Renovación de licencias de software", 5 },
                    { 5, 3, "seed", 5, new DateTime(2026, 10, 5, 9, 0, 0, 0, DateTimeKind.Utc), null, new DateOnly(2026, 12, 5), null, 9000m, "Viaje a feria comercial internacional", 1 },
                    { 6, 2, "seed", 6, new DateTime(2026, 10, 6, 9, 0, 0, 0, DateTimeKind.Utc), null, new DateOnly(2026, 11, 10), null, 2250m, "Compra de pallets para almacén", 5 }
                });

            migrationBuilder.InsertData(
                table: "UsuarioRoles",
                columns: new[] { "RolId", "UsuarioId", "FechaAsignacion" },
                values: new object[,]
                {
                    { 1, 1, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc) },
                    { 2, 2, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc) },
                    { 3, 3, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc) },
                    { 4, 4, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc) },
                    { 1, 5, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc) }
                });

            migrationBuilder.InsertData(
                table: "Aprobaciones",
                columns: new[] { "Id", "Comentario", "EstadoResultante", "Fecha", "ResponsableId", "SolicitudId" },
                values: new object[,]
                {
                    { 1, "Envio de solicitud", 2, new DateTime(2026, 10, 2, 10, 0, 0, 0, DateTimeKind.Utc), 5, 2 },
                    { 2, "Envio de solicitud", 2, new DateTime(2026, 10, 3, 10, 0, 0, 0, DateTimeKind.Utc), 1, 3 },
                    { 3, "Revision iniciada", 3, new DateTime(2026, 10, 4, 11, 0, 0, 0, DateTimeKind.Utc), 2, 3 },
                    { 4, "Envio de solicitud", 2, new DateTime(2026, 10, 4, 10, 0, 0, 0, DateTimeKind.Utc), 5, 4 },
                    { 5, "Revision iniciada", 3, new DateTime(2026, 10, 5, 11, 0, 0, 0, DateTimeKind.Utc), 2, 4 },
                    { 6, "Aprobada por nivel Gerencia", 4, new DateTime(2026, 10, 6, 15, 0, 0, 0, DateTimeKind.Utc), 3, 4 },
                    { 7, "Envio de solicitud", 2, new DateTime(2026, 10, 5, 10, 0, 0, 0, DateTimeKind.Utc), 1, 5 },
                    { 8, "Revision iniciada", 3, new DateTime(2026, 10, 6, 11, 0, 0, 0, DateTimeKind.Utc), 2, 5 },
                    { 9, "Presupuesto de viajes agotado para este periodo", 5, new DateTime(2026, 10, 7, 16, 0, 0, 0, DateTimeKind.Utc), 3, 5 },
                    { 10, "Envio de solicitud", 2, new DateTime(2026, 10, 6, 10, 0, 0, 0, DateTimeKind.Utc), 5, 6 },
                    { 11, "Revision iniciada", 3, new DateTime(2026, 10, 7, 11, 0, 0, 0, DateTimeKind.Utc), 2, 6 },
                    { 12, "Aprobada por nivel Gerencia", 4, new DateTime(2026, 10, 8, 15, 0, 0, 0, DateTimeKind.Utc), 3, 6 },
                    { 13, "Pago procesado: Transferencia - Ref: TRX-2026-0001", 6, new DateTime(2026, 10, 9, 10, 0, 0, 0, DateTimeKind.Utc), 4, 6 }
                });

            migrationBuilder.InsertData(
                table: "MovimientoTesoreria",
                columns: new[] { "Id", "CreadoPor", "FechaCreacion", "FechaModificacion", "FechaPago", "Medio", "ModificadoPor", "MontoPagado", "Referencia", "SolicitudId" },
                values: new object[] { 1, "diego.paz@solicitudes.com", new DateTime(2026, 10, 9, 10, 0, 0, 0, DateTimeKind.Utc), null, new DateTime(2026, 10, 9, 10, 0, 0, 0, DateTimeKind.Utc), "Transferencia", null, 2250m, "TRX-2026-0001", 6 });

            migrationBuilder.InsertData(
                table: "SolicitudDetalles",
                columns: new[] { "Id", "Activo", "Cantidad", "Concepto", "CreadoPor", "FechaCreacion", "FechaModificacion", "ModificadoPor", "PrecioUnitario", "SolicitudId" },
                values: new object[,]
                {
                    { 1, "A", 2, "Laptop 16GB RAM / 512GB SSD", "seed", new DateTime(2026, 10, 1, 9, 0, 0, 0, DateTimeKind.Utc), null, null, 3500m, 1 },
                    { 2, "A", 2, "Volantes impresos (paquete x100)", "seed", new DateTime(2026, 10, 2, 9, 0, 0, 0, DateTimeKind.Utc), null, null, 500m, 2 },
                    { 3, "A", 2, "Banner roll-up", "seed", new DateTime(2026, 10, 2, 9, 0, 0, 0, DateTimeKind.Utc), null, null, 250m, 2 },
                    { 4, "A", 1, "Servicio de mantenimiento preventivo", "seed", new DateTime(2026, 10, 3, 9, 0, 0, 0, DateTimeKind.Utc), null, null, 4800m, 3 },
                    { 5, "A", 10, "Licencia anual de software de oficina", "seed", new DateTime(2026, 10, 4, 9, 0, 0, 0, DateTimeKind.Utc), null, null, 120m, 4 },
                    { 6, "A", 1, "Pasajes y viáticos para feria", "seed", new DateTime(2026, 10, 5, 9, 0, 0, 0, DateTimeKind.Utc), null, null, 9000m, 5 },
                    { 7, "A", 50, "Pallet de madera estándar", "seed", new DateTime(2026, 10, 6, 9, 0, 0, 0, DateTimeKind.Utc), null, null, 45m, 6 }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Aprobaciones",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "Aprobaciones",
                keyColumn: "Id",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "Aprobaciones",
                keyColumn: "Id",
                keyValue: 3);

            migrationBuilder.DeleteData(
                table: "Aprobaciones",
                keyColumn: "Id",
                keyValue: 4);

            migrationBuilder.DeleteData(
                table: "Aprobaciones",
                keyColumn: "Id",
                keyValue: 5);

            migrationBuilder.DeleteData(
                table: "Aprobaciones",
                keyColumn: "Id",
                keyValue: 6);

            migrationBuilder.DeleteData(
                table: "Aprobaciones",
                keyColumn: "Id",
                keyValue: 7);

            migrationBuilder.DeleteData(
                table: "Aprobaciones",
                keyColumn: "Id",
                keyValue: 8);

            migrationBuilder.DeleteData(
                table: "Aprobaciones",
                keyColumn: "Id",
                keyValue: 9);

            migrationBuilder.DeleteData(
                table: "Aprobaciones",
                keyColumn: "Id",
                keyValue: 10);

            migrationBuilder.DeleteData(
                table: "Aprobaciones",
                keyColumn: "Id",
                keyValue: 11);

            migrationBuilder.DeleteData(
                table: "Aprobaciones",
                keyColumn: "Id",
                keyValue: 12);

            migrationBuilder.DeleteData(
                table: "Aprobaciones",
                keyColumn: "Id",
                keyValue: 13);

            migrationBuilder.DeleteData(
                table: "MovimientoTesoreria",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "SolicitudDetalles",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "SolicitudDetalles",
                keyColumn: "Id",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "SolicitudDetalles",
                keyColumn: "Id",
                keyValue: 3);

            migrationBuilder.DeleteData(
                table: "SolicitudDetalles",
                keyColumn: "Id",
                keyValue: 4);

            migrationBuilder.DeleteData(
                table: "SolicitudDetalles",
                keyColumn: "Id",
                keyValue: 5);

            migrationBuilder.DeleteData(
                table: "SolicitudDetalles",
                keyColumn: "Id",
                keyValue: 6);

            migrationBuilder.DeleteData(
                table: "SolicitudDetalles",
                keyColumn: "Id",
                keyValue: 7);

            migrationBuilder.DeleteData(
                table: "UsuarioRoles",
                keyColumns: new[] { "RolId", "UsuarioId" },
                keyValues: new object[] { 1, 1 });

            migrationBuilder.DeleteData(
                table: "UsuarioRoles",
                keyColumns: new[] { "RolId", "UsuarioId" },
                keyValues: new object[] { 2, 2 });

            migrationBuilder.DeleteData(
                table: "UsuarioRoles",
                keyColumns: new[] { "RolId", "UsuarioId" },
                keyValues: new object[] { 3, 3 });

            migrationBuilder.DeleteData(
                table: "UsuarioRoles",
                keyColumns: new[] { "RolId", "UsuarioId" },
                keyValues: new object[] { 4, 4 });

            migrationBuilder.DeleteData(
                table: "UsuarioRoles",
                keyColumns: new[] { "RolId", "UsuarioId" },
                keyValues: new object[] { 1, 5 });

            migrationBuilder.DeleteData(
                table: "Solicitudes",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "Solicitudes",
                keyColumn: "Id",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "Solicitudes",
                keyColumn: "Id",
                keyValue: 3);

            migrationBuilder.DeleteData(
                table: "Solicitudes",
                keyColumn: "Id",
                keyValue: 4);

            migrationBuilder.DeleteData(
                table: "Solicitudes",
                keyColumn: "Id",
                keyValue: 5);

            migrationBuilder.DeleteData(
                table: "Solicitudes",
                keyColumn: "Id",
                keyValue: 6);

            migrationBuilder.DeleteData(
                table: "Usuarios",
                keyColumn: "Id",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "Usuarios",
                keyColumn: "Id",
                keyValue: 3);

            migrationBuilder.DeleteData(
                table: "Usuarios",
                keyColumn: "Id",
                keyValue: 4);

            migrationBuilder.DeleteData(
                table: "Usuarios",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "Usuarios",
                keyColumn: "Id",
                keyValue: 5);

            migrationBuilder.DropColumn(
                name: "CreadoPor",
                table: "MovimientoTesoreria");

            migrationBuilder.DropColumn(
                name: "FechaCreacion",
                table: "MovimientoTesoreria");

            migrationBuilder.DropColumn(
                name: "FechaModificacion",
                table: "MovimientoTesoreria");

            migrationBuilder.DropColumn(
                name: "ModificadoPor",
                table: "MovimientoTesoreria");
        }
    }
}
