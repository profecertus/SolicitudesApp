using System;
using System.Collections.Generic;

namespace TienditaInfra.Persistencia;

public partial class Producto
{
    public int ProductoId { get; set; }

    public string CodigoBarras { get; set; } = null!;

    public string Nombre { get; set; } = null!;

    public int CategoriaId { get; set; }

    public int ProveedorId { get; set; }

    public decimal PrecioCompra { get; set; }

    public decimal PrecioVenta { get; set; }

    public int Stock { get; set; }

    public DateTime FechaRegistro { get; set; }

    public bool Activo { get; set; }

    public virtual Categoria Categoria { get; set; } = null!;

    public virtual ICollection<DetalleVenta> DetalleVenta { get; set; } = new List<DetalleVenta>();

    public virtual Proveedore Proveedor { get; set; } = null!;
}
