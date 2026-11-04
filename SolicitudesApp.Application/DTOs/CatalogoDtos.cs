using SolicitudesApp.Domain.Entidades;

namespace SolicitudesApp.Application.DTOs
{
    // Centros de costo
    public record CentroCostoDto(int Id, string Codigo, string Nombre, bool Activo, DateTime FechaCreacion);
    public record CrearCentroCostoDto(string Codigo, string Nombre);
    public record ActualizarCentroCostoDto(string Codigo, string Nombre, bool Activo);

    // Roles
    public record RolDto(int Id, string Nombre, decimal LimiteAprobacion);
    public record GuardarRolDto(string Nombre, decimal LimiteAprobacion);

    // Usuarios (el PasswordHash nunca se expone)
    public record UsuarioDto(int Id, string Nombre, string Correo, bool Activo, DateTime FechaCreacion, List<RolDto> Roles);
    public record CrearUsuarioDto(string Nombre, string Correo, List<int>? RolIds);
    public record ActualizarUsuarioDto(string Nombre, string Correo, bool Activo);

    // Detalles de una solicitud (el DTO de lectura es SolicitudDetalleDto)
    public record GuardarDetalleDto(string Concepto, int Cantidad, decimal PrecioUnitario);

    // Solo lectura: se generan desde el flujo de estados de la solicitud
    public record AprobacionDto(
        int Id,
        int SolicitudId,
        string CodigoSolicitud,
        EstadoSolicitud EstadoResultante,
        string EstadoNombre,
        int ResponsableId,
        string ResponsableNombre,
        string? Comentario,
        DateTime Fecha);

    public record MovimientoTesoreriaDto(
        int Id,
        int SolicitudId,
        string CodigoSolicitud,
        decimal MontoPagado,
        string Medio,
        string Referencia,
        DateTime FechaPago,
        string CreadoPor);
}
