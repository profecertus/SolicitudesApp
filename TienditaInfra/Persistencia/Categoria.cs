using System;
using System.Collections.Generic;

namespace TienditaInfra.Persistencia;

public partial class Categoria
{
    public int CategoriaId { get; set; }

    public string Nombre { get; set; } = null!;

    public string? Descripcion { get; set; }

    public bool Activo { get; set; }

    public virtual ICollection<Producto> Productos { get; set; } = new List<Producto>();
}
