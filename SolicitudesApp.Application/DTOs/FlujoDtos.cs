using SolicitudesApp.Domain.Entidades;

namespace SolicitudesApp.Application.DTOs
{
    public record RevisarSolicitudDto(int RevisorId, bool Conforme, string? Observaciones);
    public record ResultadoTransicionDto(int SolicitudId, string EstadoResultante, string Mensaje);

    public record ObservarSolicitudDto(int ResponsableId, string Observaciones);
    public record AprobarSolicitudDto(int ResponsableId, string? Observaciones);
    public record RechazarSolicitudDto(int ResponsableId, string MotivoRechazo);
    public record PagarSolicitudDto(int ResponsableId, string Medio, string Referencia);

    public record TimelineItemDto(
        int Id,
        int SolicitudId,
        EstadoSolicitud EstadoResultante,
        string EstadoNombre,
        int ResponsableId,
        string ResponsableNombre,
        string? Comentario,
        DateTime FechaAccion
        );
}
