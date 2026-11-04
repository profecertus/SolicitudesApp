using Microsoft.AspNetCore.Mvc;
using SolicitudesApp.Application.DTOs;
using SolicitudesApp.Application.Services;

namespace SolicitudesApp.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CentrosCostoController : ControllerBase
    {
        private readonly ICentroCostoService _service;
        public CentrosCostoController(ICentroCostoService service) => _service = service;

        [HttpGet]
        public async Task<ActionResult<IReadOnlyList<CentroCostoDto>>> GetAll([FromQuery] bool? activo = null) =>
            Ok(await _service.ListarAsync(activo));

        [HttpGet("{id:int}")]
        public async Task<ActionResult<CentroCostoDto>> GetById(int id)
        {
            var item = await _service.ObtenerPorIdAsync(id);
            if (item == null) return NotFound(new { mensaje = $"No se encontró el centro de costo con Id {id}" });
            return Ok(item);
        }

        [HttpPost]
        public async Task<ActionResult<CentroCostoDto>> Create([FromBody] CrearCentroCostoDto dto)
        {
            var creado = await _service.CrearAsync(dto);
            return CreatedAtAction(nameof(GetById), new { id = creado.Id }, creado);
        }

        [HttpPut("{id:int}")]
        public async Task<IActionResult> Update(int id, [FromBody] ActualizarCentroCostoDto dto)
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
    }
}
