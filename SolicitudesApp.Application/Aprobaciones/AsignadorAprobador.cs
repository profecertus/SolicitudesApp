using SolicitudesApp.Domain.Entidades;

namespace SolicitudesApp.Application.Aprobaciones;

public class AsignadorAprobador
{
    // Los niveles se recorren de menor a mayor limite
    private static readonly (string Nivel, decimal Limite)[] _niveles =
    [
        ("Jefatura", 1000m),
        ("Gerencia", 5000m),
        ("Directorio", decimal.MaxValue)
    ];

    public static string Determinar(decimal monto)
    {
        if (monto <= 0)
            throw new ArgumentException("El monto debe ser mayor a cero");

        return _niveles.First(n => monto <= n.Limite).Nivel;
    }

    public bool RequiereDobleAprobacion(decimal monto) => monto > 5000m;
}
