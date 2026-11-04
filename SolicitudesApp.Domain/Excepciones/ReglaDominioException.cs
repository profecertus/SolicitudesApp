namespace SolicitudesApp.Domain.Excepciones;

public class ReglaDominioException : Exception
{
    public ReglaDominioException(string mensaje) : base(mensaje)
    {
    }
}
