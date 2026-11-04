using System;
using System.Collections.Generic;
using System.Text;

namespace SolicitudesApp.Domain.Entidades
{
    public class CentroCosto : EntidadAuditable
    {
        public int Id { get; set; }
        public string Codigo { get; set; } = String.Empty;
        public string Nombre { get; set;  } = String.Empty;
        public bool Activo { get; set; } = true;
        public ICollection<Solicitud> Solicitudes { get; set; } = new List<Solicitud>();
    }
}
