using SolicitudesApp.Domain.Entidades;
using System;
using System.Collections.Generic;
using System.Text;

namespace SolicitudesApp.Application.DTOs
{
    public record CrearDetalleDto(string Concepto, int Cantidad, decimal PrecioUnitario);
    public record CrearSolicitudDto(
        string Motivo,
        int SolicitanteId,
        int? CentroCostoId,
        List<CrearDetalleDto> Detalles
    );
    public record ActualizarSolicitudDto(
        string Motivo,
        int? CentroCostoId
    );
    public record SolicitudDetalleDto(
        int Id,
        string Concepto,
        int Cantidad,
        decimal PrecioUnitario,
        decimal Importe
    );
    public record SolicitudDto(
        int Id,
        string Codigo,
        string Motivo,
        decimal MontoTotal,
        EstadoSolicitud Estado,
        string EstadoNombre,
        int SolicitanteId,
        string SolicitanteNombre,
        int? CentroCostoId,
        string CentroCostoNombre,
        DateTime FechaCreacion,
        List<SolicitudDetalleDto> Detalles
    );
    public record ResumenEstadosDto(
        string Estado,
        int Cantidad,
        decimal MontoTotal
    );
}
