using Microsoft.AspNetCore.Mvc;
using SolicitudesApp.Application.DTOs;
using SolicitudesApp.Application.Services;

namespace SolicitudesApp.Api.Controllers
{
    [ApiController]
    [Route("api/solicitudes/{solicitudId:int}/detalles")]
    public class SolicitudDetallesController : ControllerBase
    {
        private readonly ISolicitudDetalleService _service;
        public SolicitudDetallesController(ISolicitudDetalleService service) => _service = service;

        [HttpGet]
        public async Task<ActionResult<IReadOnlyList<SolicitudDetalleDto>>> GetAll(int solicitudId) =>
            Ok(await _service.ListarAsync(solicitudId));

        [HttpGet("{id:int}")]
        public async Task<ActionResult<SolicitudDetalleDto>> GetById(int solicitudId, int id)
        {
            var item = await _service.ObtenerPorIdAsync(solicitudId, id);
            if (item == null) return NotFound(new { mensaje = $"No se encontró el detalle {id} en la solicitud {solicitudId}" });
            return Ok(item);
        }

        [HttpPost]
        public async Task<ActionResult<SolicitudDetalleDto>> Create(int solicitudId, [FromBody] GuardarDetalleDto dto)
        {
            var creado = await _service.CrearAsync(solicitudId, dto);
            return CreatedAtAction(nameof(GetById), new { solicitudId, id = creado.Id }, creado);
        }

        [HttpPut("{id:int}")]
        public async Task<IActionResult> Update(int solicitudId, int id, [FromBody] GuardarDetalleDto dto)
        {
            await _service.ActualizarAsync(solicitudId, id, dto);
            return NoContent();
        }

        [HttpDelete("{id:int}")]
        public async Task<IActionResult> Delete(int solicitudId, int id)
        {
            await _service.EliminarAsync(solicitudId, id);
            return NoContent();
        }
    }
}
