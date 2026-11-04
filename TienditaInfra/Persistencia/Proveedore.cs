using System;
using System.Collections.Generic;

namespace TienditaInfra.Persistencia;

public partial class Proveedore
{
    public int ProveedorId { get; set; }

    public string Ruc { get; set; } = null!;

    public string RazonSocial { get; set; } = null!;

    public string? Telefono { get; set; }

    public string? Email { get; set; }

    public string? Direccion { get; set; }

    public bool Activo { get; set; }

    public virtual ICollection<Producto> Productos { get; set; } = new List<Producto>();
}
