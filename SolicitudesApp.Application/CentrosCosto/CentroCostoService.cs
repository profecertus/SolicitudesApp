using FluentValidation;
using SolicitudesApp.Application.DTOs;
using SolicitudesApp.Application.Interfaces;
using SolicitudesApp.Application.Services;
using SolicitudesApp.Domain.Entidades;
using SolicitudesApp.Domain.Excepciones;

namespace SolicitudesApp.Application.CentrosCosto
{
    public class CentroCostoService : ICentroCostoService
    {
        private readonly IUnitOfWork _uow;
        private readonly IValidator<CrearCentroCostoDto> _crearValidator;
        private readonly IValidator<ActualizarCentroCostoDto> _actualizarValidator;

        public CentroCostoService(IUnitOfWork uow,
            IValidator<CrearCentroCostoDto> crearValidator,
            IValidator<ActualizarCentroCostoDto> actualizarValidator)
        {
            _uow = uow;
            _crearValidator = crearValidator;
            _actualizarValidator = actualizarValidator;
        }

        public async Task<IReadOnlyList<CentroCostoDto>> ListarAsync(bool? activo = null, CancellationToken ct = default)
        {
            var centros = activo.HasValue
                ? await _uow.CentrosCosto.FindAsync(c => c.Activo == activo.Value, ct)
                : await _uow.CentrosCosto.ListAllAsync(ct);
            return centros.OrderBy(c => c.Codigo).Select(MapearADto).ToList();
        }

        public async Task<CentroCostoDto?> ObtenerPorIdAsync(int id, CancellationToken ct = default)
        {
            var centro = await _uow.CentrosCosto.GetByIdAsync(id, ct);
            return centro is null ? null : MapearADto(centro);
        }

        public async Task<CentroCostoDto> CrearAsync(CrearCentroCostoDto dto, CancellationToken ct = default)
        {
            await _crearValidator.ValidateAndThrowAsync(dto, ct);
            await ValidarCodigoUnicoAsync(dto.Codigo, null, ct);

            var centro = new CentroCosto { Codigo = dto.Codigo.Trim(), Nombre = dto.Nombre.Trim(), Activo = true };
            await _uow.CentrosCosto.AddAsync(centro, ct);
            await _uow.SaveChangesAsync(ct);
            return MapearADto(centro);
        }

        public async Task ActualizarAsync(int id, ActualizarCentroCostoDto dto, CancellationToken ct = default)
        {
            await _actualizarValidator.ValidateAndThrowAsync(dto, ct);
            var centro = await _uow.CentrosCosto.GetByIdAsync(id, ct)
                ?? throw new KeyNotFoundException($"No se encontró el centro de costo con Id {id}");
            await ValidarCodigoUnicoAsync(dto.Codigo, id, ct);

            centro.Codigo = dto.Codigo.Trim();
            centro.Nombre = dto.Nombre.Trim();
            centro.Activo = dto.Activo;
            await _uow.CentrosCosto.UpdateAsync(centro, ct);
            await _uow.SaveChangesAsync(ct);
        }

        public async Task EliminarAsync(int id, CancellationToken ct = default)
        {
            var centro = await _uow.CentrosCosto.GetByIdAsync(id, ct)
                ?? throw new KeyNotFoundException($"No se encontró el centro de costo con Id {id}");
            if (await _uow.Solicitudes.AnyAsync(s => s.CentroCostoId == id, ct))
                throw new ReglaDominioException("No se puede eliminar un centro de costo con solicitudes asociadas. Desactívelo en su lugar.");

            await _uow.CentrosCosto.DeleteAsync(centro, ct);
            await _uow.SaveChangesAsync(ct);
        }

        private async Task ValidarCodigoUnicoAsync(string codigo, int? idActual, CancellationToken ct)
        {
            var normalizado = codigo.Trim();
            if (await _uow.CentrosCosto.AnyAsync(c => c.Codigo == normalizado && c.Id != (idActual ?? 0), ct))
                throw new ReglaDominioException($"Ya existe un centro de costo con el código {normalizado}");
        }

        private static CentroCostoDto MapearADto(CentroCosto c) => new(c.Id, c.Codigo, c.Nombre, c.Activo, c.FechaCreacion);
    }
}
