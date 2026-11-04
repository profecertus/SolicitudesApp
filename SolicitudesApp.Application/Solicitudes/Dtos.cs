using SolicitudesApp.Domain.Entidades;

namespace SolicitudesApp.Application.Solicitudes
{
    /*
    public record SolicitudLecturaDto(
        int Id,
        string Motivo,
        decimal MontoTotal,
        string Estado,
        string Solicitante,
        DateTime FechaCreacion,
        int CantidadDetalles
        );

    public record SolicitudResumenDto(
        int Id,
        string Motivo, 
        decimal MontoTotal,
        string Solicitante,
        int TotalItems,
        string Estado
        );

    public record CambiarEstadoDto(
        EstadoSolicitud Destino,
        int ResponsableId,
        string? Comentario
    );

    public record SolicitudRespuestaDto(
        int Id,
        string Motivo,
        decimal MontoTotal,
        string Estado,
        string AprobadorRequerido,
        string nombre,
        string correo,
        IEnumerable<AprobacionHistorialDto> Historial,
        DateTime? FechaCreacion = null,
        DateTime? FechaModificacion = null
    );

    public record AprobacionHistorialDto(
        string Estado,
        string? Responsable,
        string? Comentario,
        DateTime Fecha
        );
    public record CrearSolicitudItemDto(string Concepto, int Cantidad, decimal PrecioUnitario);
    
    public record CrearSolicitudDto(string Motivo, int SolicitanteId, int? CentroCostoId, List<CrearSolicitudItemDto> Items);

    public record SolicitudDto(int Id, string Motivo, decimal Monto, string Estado);
  */
}
