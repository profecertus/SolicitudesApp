using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SolicitudesApp.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class UsuariosCredencialesYJwt : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Username",
                table: "Usuarios",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: false,
                defaultValue: "");

            migrationBuilder.UpdateData(
                table: "Usuarios",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "Username", "PasswordHash" },
                values: new object[] { "ana.torres", "0f80f19b3ab7eb5c11371e4727ec69e325443dc2d0d0918e44739e36a3b08377" });

            migrationBuilder.UpdateData(
                table: "Usuarios",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "Username", "PasswordHash" },
                values: new object[] { "bruno.diaz", "0f80f19b3ab7eb5c11371e4727ec69e325443dc2d0d0918e44739e36a3b08377" });

            migrationBuilder.UpdateData(
                table: "Usuarios",
                keyColumn: "Id",
                keyValue: 3,
                columns: new[] { "Username", "PasswordHash" },
                values: new object[] { "carla.ruiz", "0f80f19b3ab7eb5c11371e4727ec69e325443dc2d0d0918e44739e36a3b08377" });

            migrationBuilder.UpdateData(
                table: "Usuarios",
                keyColumn: "Id",
                keyValue: 4,
                columns: new[] { "Username", "PasswordHash" },
                values: new object[] { "diego.paz", "0f80f19b3ab7eb5c11371e4727ec69e325443dc2d0d0918e44739e36a3b08377" });

            migrationBuilder.UpdateData(
                table: "Usuarios",
                keyColumn: "Id",
                keyValue: 5,
                columns: new[] { "Username", "PasswordHash" },
                values: new object[] { "elena.soto", "0f80f19b3ab7eb5c11371e4727ec69e325443dc2d0d0918e44739e36a3b08377" });

            migrationBuilder.InsertData(
                table: "Usuarios",
                columns: new[] { "Id", "Activo", "Correo", "CreadoPor", "FechaCreacion", "FechaModificacion", "ModificadoPor", "Nombre", "PasswordHash", "Username" },
                values: new object[] { 6, true, "admin@solicitudes.com", "seed", new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, "Administrador Sistema", "1802a3950a346fe794f67d6584832b7066519637e7b40508c0cd6daf00277b33", "admin" });

            migrationBuilder.InsertData(
                table: "UsuarioRoles",
                columns: new[] { "RolId", "UsuarioId", "FechaAsignacion" },
                values: new object[] { 3, 6, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc) });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "UsuarioRoles",
                keyColumns: new[] { "RolId", "UsuarioId" },
                keyValues: new object[] { 3, 6 });

            migrationBuilder.DeleteData(
                table: "Usuarios",
                keyColumn: "Id",
                keyValue: 6);

            migrationBuilder.UpdateData(
                table: "Usuarios",
                keyColumn: "Id",
                keyValue: 1,
                column: "PasswordHash",
                value: "seed-sin-autenticacion");

            migrationBuilder.UpdateData(
                table: "Usuarios",
                keyColumn: "Id",
                keyValue: 2,
                column: "PasswordHash",
                value: "seed-sin-autenticacion");

            migrationBuilder.UpdateData(
                table: "Usuarios",
                keyColumn: "Id",
                keyValue: 3,
                column: "PasswordHash",
                value: "seed-sin-autenticacion");

            migrationBuilder.UpdateData(
                table: "Usuarios",
                keyColumn: "Id",
                keyValue: 4,
                column: "PasswordHash",
                value: "seed-sin-autenticacion");

            migrationBuilder.UpdateData(
                table: "Usuarios",
                keyColumn: "Id",
                keyValue: 5,
                column: "PasswordHash",
                value: "seed-sin-autenticacion");

            migrationBuilder.DropColumn(
                name: "Username",
                table: "Usuarios");
        }
    }
}
