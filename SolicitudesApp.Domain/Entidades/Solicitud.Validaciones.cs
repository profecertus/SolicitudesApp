using SolicitudesApp.Domain.Excepciones;
using static SolicitudesApp.Domain.Entidades.EstadoSolicitud;

namespace SolicitudesApp.Domain.Entidades
{
    public partial class Solicitud:EntidadAuditable
    {
        private bool PuedePasarA(EstadoSolicitud destino) => (Estado, destino) switch
        {
            (Borrador, Enviada) => true, // el usuario la envia
            (Enviada, EnRevision) => true, // el revisor la toma
            (EnRevision, Borrador) => true, // se devuelve observada
            (EnRevision, Aprobada) => true,
            (EnRevision, Rechazada) => true,
            (Aprobada, Pagada) => true, // tesoreria paga
            _ => false
        };
        public void CambiarEstado(EstadoSolicitud destino, Usuario responsable,
        string? comentario = null)
        {
            if (!PuedePasarA(destino))
                throw new ReglaDominioException(
                $"No se puede pasar de {Estado} a {destino}");
            Estado = destino;
            FechaModificacion = DateTime.UtcNow;
            ModificadoPor = responsable.Correo;
            Aprobaciones.Add(new Aprobacion
            {
                SolicitudId = Id,
                ResponsableId = responsable.Id,
                Responsable = responsable,
                EstadoResultante = destino,
                Comentario = comentario
            });
        }

        public void RegistrarPago(MovimientoTesoreria movimiento, Usuario tesorero)
        {
            ArgumentNullException.ThrowIfNull(movimiento);
            ArgumentNullException.ThrowIfNull(tesorero);
            try
            {
                CambiarEstado(Pagada, tesorero, $"Pago procesado: {movimiento.Medio} - Ref: {movimiento.Referencia}");
            }catch(ReglaDominioException rde)
            {
                throw;
            }
            

            movimiento.SolicitudId = Id;
            movimiento.Solicitud = this;                
            movimiento.CreadoPor = tesorero.Correo;
            movimiento.FechaCreacion = DateTime.UtcNow;
            movimiento.FechaPago = DateTime.UtcNow;
            Movimiento = movimiento;
        }

    }
}
