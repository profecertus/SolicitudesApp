using Microsoft.AspNetCore.Mvc;
using SolicitudesApp.Application.DTOs;
using SolicitudesApp.Application.Services;

namespace SolicitudesApp.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class RolesController : ControllerBase
    {
        private readonly IRolService _service;
        public RolesController(IRolService service) => _service = service;

        [HttpGet]
        public async Task<ActionResult<IReadOnlyList<RolDto>>> GetAll() => Ok(await _service.ListarAsync());

        [HttpGet("{id:int}")]
        public async Task<ActionResult<RolDto>> GetById(int id)
        {
            var item = await _service.ObtenerPorIdAsync(id);
            if (item == null) return NotFound(new { mensaje = $"No se encontró el rol con Id {id}" });
            return Ok(item);
        }

        [HttpPost]
        public async Task<ActionResult<RolDto>> Create([FromBody] GuardarRolDto dto)
        {
            var creado = await _service.CrearAsync(dto);
            return CreatedAtAction(nameof(GetById), new { id = creado.Id }, creado);
        }

        [HttpPut("{id:int}")]
        public async Task<IActionResult> Update(int id, [FromBody] GuardarRolDto dto)
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
