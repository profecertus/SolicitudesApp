using System;
using System.Collections.Generic;

namespace TienditaInfra.Persistencia;

public partial class Venta
{
    public int VentaId { get; set; }

    public string NumeroBoleta { get; set; } = null!;

    public DateTime FechaVenta { get; set; }

    public string ClienteNombre { get; set; } = null!;

    public string MetodoPago { get; set; } = null!;

    public decimal Total { get; set; }

    public string Estado { get; set; } = null!;

    public virtual ICollection<DetalleVenta> DetalleVenta { get; set; } = new List<DetalleVenta>();
}
