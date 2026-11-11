namespace SolicitudesApp.Domain.Entidades;

public class Usuario : EntidadAuditable
{
    public int Id { get; set; }
    public string Username { get; set; } = "";
    public string Nombre { get; set; } = "";
    public string Correo { get; set; } = "";
    public string PasswordHash { get; set; } = "";
    public bool Activo { get; set; } = true;
    // Muchos a muchos
    public ICollection<Rol> Roles { get; set; } = new List<Rol>();
    // Uno a muchos: las solicitudes que este usuario presento
    public ICollection<Solicitud> Solicitudes { get; set; } = new List<Solicitud>();
}
public class Rol
{
    public int Id { get; set; }
    public string Nombre { get; set; } = ""; // Usuario, Revisor, Aprobador, Tesoreria
    public decimal LimiteAprobacion { get; set; }
    public ICollection<Usuario> Usuarios { get; set; } = new List<Usuario>();
}
