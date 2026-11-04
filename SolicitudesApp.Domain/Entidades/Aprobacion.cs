using System;
using System.Collections.Generic;
using System.Text;

namespace SolicitudesApp.Domain.Entidades
{
    public class Aprobacion
    {
        public int Id { get; set; }
        public int SolicitudId { get; set; }
        public Solicitud? Solicitud { get; set; }
        public int ResponsableId { get; set; }
        public Usuario? Responsable { get; set; }
        public EstadoSolicitud EstadoResultante { get; set; }
        public string? Comentario { get; set; }
        public DateTime Fecha { get; set; } = DateTime.UtcNow;
    }
    public class MovimientoTesoreria : EntidadAuditable
    {
        public int Id { get; set; }
        public int SolicitudId { get; set; } // unica: relacion uno a uno
        public Solicitud? Solicitud { get; set; }
        public decimal MontoPagado { get; set; }
        public string Medio { get; set; } = ""; // Transferencia, Cheque, Efectivo
        public string Referencia { get; set; } = "";
        public DateTime FechaPago { get; set; } = DateTime.UtcNow;
    }
}
