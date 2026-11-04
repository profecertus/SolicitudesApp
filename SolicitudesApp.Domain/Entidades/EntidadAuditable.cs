using System;
using System.Collections.Generic;
using System.Text;

namespace SolicitudesApp.Domain.Entidades
{
    public abstract class EntidadAuditable
    {
        public DateTime FechaCreacion { get; set; } = DateTime.UtcNow;
        public string CreadoPor { get; set; } = "";
        public DateTime? FechaModificacion { get; set; }
        public string? ModificadoPor { get; set; }
    }
}
