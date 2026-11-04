using Microsoft.AspNetCore.Mvc;
using SolicitudesApp.Application.DTOs;
using SolicitudesApp.Application.Services;

namespace SolicitudesApp.Api.Controllers
{
    // Solo lectura: las aprobaciones se crean con las transiciones de /api/solicitudes/{id}/...
    [ApiController]
    [Route("api/[controller]")]
    public class AprobacionesController : ControllerBase
    {
        private readonly IAprobacionService _service;
        public AprobacionesController(IAprobacionService service) => _service = service;

        [HttpGet]
        public async Task<ActionResult<IReadOnlyList<AprobacionDto>>> GetAll([FromQuery] int? solicitudId = null) =>
            Ok(await _service.ListarAsync(solicitudId));

        [HttpGet("{id:int}")]
        public async Task<ActionResult<AprobacionDto>> GetById(int id)
        {
            var item = await _service.ObtenerPorIdAsync(id);
            if (item == null) return NotFound(new { mensaje = $"No se encontró la aprobación con Id {id}" });
            return Ok(item);
        }
    }
}
