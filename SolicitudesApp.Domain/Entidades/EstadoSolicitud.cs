using System;
using System.Collections.Generic;
using System.Text;

namespace SolicitudesApp.Domain.Entidades
{
    public enum EstadoSolicitud
    {
        Borrador = 1,
        Enviada = 2,
        EnRevision = 3,
        Aprobada = 4,
        Rechazada = 5,
        Pagada = 6
    }
}
