using Microsoft.AspNetCore.Mvc;
using SolicitudesApp.Application.DTOs;
using SolicitudesApp.Application.Services;

namespace SolicitudesApp.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UsuariosController : ControllerBase
    {
        private readonly IUsuarioService _service;
        public UsuariosController(IUsuarioService service) => _service = service;

        [HttpGet]
        public async Task<ActionResult<IReadOnlyList<UsuarioDto>>> GetAll() => Ok(await _service.ListarAsync());

        [HttpGet("{id:int}")]
        public async Task<ActionResult<UsuarioDto>> GetById(int id)
        {
            var item = await _service.ObtenerPorIdAsync(id);
            if (item == null) return NotFound(new { mensaje = $"No se encontró el usuario con Id {id}" });
            return Ok(item);
        }

        [HttpPost]
        public async Task<ActionResult<UsuarioDto>> Create([FromBody] CrearUsuarioDto dto)
        {
            var creado = await _service.CrearAsync(dto);
            return CreatedAtAction(nameof(GetById), new { id = creado.Id }, creado);
        }

        [HttpPut("{id:int}")]
        public async Task<IActionResult> Update(int id, [FromBody] ActualizarUsuarioDto dto)
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

        // UsuarioRoles: asignación de roles
        [HttpGet("{id:int}/roles")]
        public async Task<ActionResult<IReadOnlyList<RolDto>>> GetRoles(int id) => Ok(await _service.ListarRolesAsync(id));

        [HttpPost("{id:int}/roles/{rolId:int}")]
        public async Task<IActionResult> AsignarRol(int id, int rolId)
        {
            await _service.AsignarRolAsync(id, rolId);
            return NoContent();
        }

        [HttpDelete("{id:int}/roles/{rolId:int}")]
        public async Task<IActionResult> QuitarRol(int id, int rolId)
        {
            await _service.QuitarRolAsync(id, rolId);
            return NoContent();
        }
    }
}
