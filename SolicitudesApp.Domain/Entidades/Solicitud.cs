using SolicitudesApp.Domain.Excepciones;
using static SolicitudesApp.Domain.Entidades.EstadoSolicitud;

namespace SolicitudesApp.Domain.Entidades
{
    public partial class Solicitud : EntidadAuditable
    {       
        public int Id { get; set; }
        public string Motivo { get; set; } = "";
        public decimal MontoTotal { get; set; }
        public int? CentroCostoId { get; set; }
        public CentroCosto? CentroCosto { get; set; }
        public EstadoSolicitud Estado { get; set; } = EstadoSolicitud.Borrador;
        public DateOnly FechaRequerida { get; set; }
        // Clave foranea + propiedad de navegacion
        public int SolicitanteId { get; set; }
        public Usuario? Solicitante { get; set; }
        // Uno a muchos
        public ICollection<SolicitudDetalle> Detalles { get; set; }
        = new List<SolicitudDetalle>();
        public ICollection<Aprobacion> Aprobaciones { get; set; } = new List<Aprobacion>();
        // Uno a uno
        public MovimientoTesoreria? Movimiento { get; set; }
        // El total se recalcula a partir del detalle
        public void RecalcularTotal() => MontoTotal = Detalles.Where(d => d.Activo == 'A').Sum(d => d.Importe);        
    }

    public class SolicitudDetalle:EntidadAuditable
    {
        public int Id { get; set; }
        public int SolicitudId { get; set; }
        public Solicitud? Solicitud { get; set; }
        public string Concepto { get; set; } = "";
        public int Cantidad { get; set; } = 1;
        public decimal PrecioUnitario { get; set; }
        public char Activo {  get; set; } = 'A';
        public decimal Importe => Cantidad * PrecioUnitario;
    }
}

