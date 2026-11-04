using Microsoft.AspNetCore.Mvc;
using Microsoft.JSInterop.Infrastructure;
using SolicitudesApp.Application.DTOs;
using SolicitudesApp.Application.Services;
using SolicitudesApp.Application.Solicitudes;
using SolicitudesApp.Domain.Entidades;
using SolicitudesApp.Domain.Excepciones;

namespace SolicitudesApp.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class SolicitudesController : ControllerBase
    {
        private readonly ISolicitudService _service;
        public SolicitudesController(ISolicitudService service) => _service = service;
        [HttpGet]
        public async Task<ActionResult<PagedResult<SolicitudDto>>> GetAll(
        [FromQuery] int pagina = 1,
        [FromQuery] int tamanoPagina = 10,
        [FromQuery] EstadoSolicitud? estado = null)
        {
            var result = await _service.ListarPaginadoAsync(pagina, tamanoPagina, estado);
            return Ok(result);
        }

        [HttpGet("{id:int}")]
        public async Task<ActionResult<SolicitudDto>> GetById(int id)
        {
            var item = await _service.ObtenerPorIdAsync(id);
            if (item == null) return NotFound(new { mensaje = $"No se encontró la solicitud con Id {id}" });
            return Ok(item);
        }

        [HttpPost]
        public async Task<ActionResult<SolicitudDto>> Create([FromBody] CrearSolicitudDto dto)
        {
            var creada = await _service.CrearAsync(dto);
            return CreatedAtAction(nameof(GetById), new { id = creada.Id }, creada);
        }

        [HttpPut("{id:int}")]
        public async Task<IActionResult> Update(int id, [FromBody] ActualizarSolicitudDto dto)
        {
            await _service.ActualizarAsync(id, dto);
            return NoContent();
        }

        [HttpDelete("{id:int}")]
        public async Task<IActionResult> Delete(int id)
        {
            await _service.EliminarAsync(id);
            return NoContent();
        }

        [HttpGet("resumen-estados")]
        public async Task<ActionResult<IReadOnlyList<ResumenEstadosDto>>> GetResumenEstados()
        {
            var resumen = await _service.ObtenerResumenPorEstadosAsync();
            return Ok(resumen);
        }

        [HttpPost("{id:int}/enviar")]
        public async Task<ActionResult<SolicitudDto>> Enviar(int id)
        {
            var result = await _service.EnviarAsync(id);
            return Ok(result);
        }

        [HttpPost("{id:int}/revisar")]
        public async Task<ActionResult<SolicitudDto>> Revisar(int id, [FromQuery] int revisorId = 4)
        {
            var result = await _service.RevisarAsync(id, revisorId);
            return Ok(result);
        }

        [HttpPost("{id:int}/observar")]
        public async Task<ActionResult<Solicitud>> Observar(int id, [FromBody] ObservarSolicitudDto dto)
        {
            var result = await _service.ObservarAsync(id, dto);
            return Ok(result);
        }

        [HttpPost("{id:int}/aprobar")]
        public async Task<ActionResult<SolicitudDto>> Aprobar(int id, [FromBody] AprobarSolicitudDto dto)
        {
            var result = await _service.AprobarAsync(id, dto);
            return Ok(result);
        }

        [HttpPost("{id:int}/rechazar")]
        public async Task<ActionResult<SolicitudDto>> Rechazar(int id, [FromBody] RechazarSolicitudDto dto)
        {
            var result = await _service.RechazarAsync(id, dto);
            return Ok(result);
        }

        [HttpPost("{id:int}/pagar")]
        public async Task<ActionResult<SolicitudDto>> Pagar(int id, [FromBody] PagarSolicitudDto dto)
        {
            var result = await _service.PagarAsync(id, dto);
            return Ok(result);
        }

        [HttpGet("{id:int}/timeline")]
        public async Task<ActionResult<List<TimelineItemDto>>> ObtenerTimeline(int id)
        {
            var timeline = await _service.ObtenerTimelineAsync(id);
            return Ok(timeline);
        }

    } 
}