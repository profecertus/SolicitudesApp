using Microsoft.AspNetCore.Mvc;
using SolicitudesApp.Application.DTOs;
using SolicitudesApp.Application.Services;

namespace SolicitudesApp.Api.Controllers
{
    // Solo lectura: los movimientos se crean con POST /api/solicitudes/{id}/pagar
    [ApiController]
    [Route("api/[controller]")]
    public class MovimientosTesoreriaController : ControllerBase
    {
        private readonly IMovimientoTesoreriaService _service;
        public MovimientosTesoreriaController(IMovimientoTesoreriaService service) => _service = service;

        [HttpGet]
        public async Task<ActionResult<IReadOnlyList<MovimientoTesoreriaDto>>> GetAll() => Ok(await _service.ListarAsync());

        [HttpGet("{id:int}")]
        public async Task<ActionResult<MovimientoTesoreriaDto>> GetById(int id)
        {
            var item = await _service.ObtenerPorIdAsync(id);
            if (item == null) return NotFound(new { mensaje = $"No se encontró el movimiento con Id {id}" });
            return Ok(item);
        }
    }
}
