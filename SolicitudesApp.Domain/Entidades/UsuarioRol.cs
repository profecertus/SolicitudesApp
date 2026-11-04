 using System;
using System.Collections.Generic;
using System.Text;

namespace SolicitudesApp.Domain.Entidades
{
    public class UsuarioRol
    {
        public int UsuarioId { get; set; }
        public Usuario? Usuario { get; set; }
        public int RolId { get; set; }
        public Rol? Rol { get; set; }
        public DateTime FechaAsignacion { get; set; } = DateTime.UtcNow;
    }
}
