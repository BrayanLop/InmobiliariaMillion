using Microsoft.AspNetCore.Mvc;
using InmobiliariaMillion.Aplicacion.Servicios.Interfaces;
using InmobiliariaMillion.Aplicacion.DTOs.Modelos.TrazabilidadPropiedad;

namespace InmobiliariaMillion.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Produces("application/json")]
    public class TrazabilidadPropiedadController : ControllerBase
    {
        private readonly ITrazabilidadPropiedadServicio _trazabilidadPropiedadServicio;

        public TrazabilidadPropiedadController(ITrazabilidadPropiedadServicio trazabilidadPropiedadServicio)
        {
            _trazabilidadPropiedadServicio = trazabilidadPropiedadServicio;
        }

        [HttpPost]
        public async Task<ActionResult<TrazabilidadPropiedadOutputDto>> Crear([FromBody] TrazabilidadPropiedadInputDto dto)
        {
            var result = await _trazabilidadPropiedadServicio.CrearTrazabilidadPropiedadAsync(dto);
            return CreatedAtAction(nameof(ObtenerPorId), new { idTrazabilidadPropiedad = result.IdTrazabilidadPropiedad }, result);
        }

        [HttpGet("propiedad/{idPropiedad}")]
        public async Task<ActionResult<List<TrazabilidadPropiedadOutputDto>>> ObtenerPorPropiedad(string idPropiedad)
        {
            var result = await _trazabilidadPropiedadServicio.ObtenerPorPropiedadAsync(idPropiedad);
            return Ok(result);
        }

        [HttpGet("{idTrazabilidadPropiedad}")]
        public async Task<ActionResult<TrazabilidadPropiedadOutputDto>> ObtenerPorId(string idTrazabilidadPropiedad)
        {
            var result = await _trazabilidadPropiedadServicio.ObtenerTrazabilidadPropiedadPorIdAsync(idTrazabilidadPropiedad);
            if (result == null)
                return NotFound();
            return Ok(result);
        }

        [HttpPut("{idTrazabilidadPropiedad}")]
        public async Task<ActionResult<TrazabilidadPropiedadOutputDto>> Actualizar(string idTrazabilidadPropiedad, [FromBody] TrazabilidadPropiedadInputDto dto)
        {
            if (idTrazabilidadPropiedad != dto.IdTrazabilidadPropiedad)
                return BadRequest("El id de la ruta no coincide con el del cuerpo.");
            var result = await _trazabilidadPropiedadServicio.ActualizarTrazabilidadPropiedadAsync(dto);
            if (result == null)
                return NotFound();
            return Ok(result);
        }

        [HttpDelete("{idTrazabilidadPropiedad}")]
        public async Task<ActionResult> Eliminar(string idTrazabilidadPropiedad)
        {
            var eliminado = await _trazabilidadPropiedadServicio.EliminarTrazabilidadPropiedadAsync(idTrazabilidadPropiedad);
            if (!eliminado)
                return NotFound();
            return NoContent();
        }

        [HttpGet("ventas-recientes")]
        public async Task<ActionResult<List<TrazabilidadPropiedadOutputDto>>> ObtenerVentasRecientes([FromQuery] DateTime desde)
        {
            var result = await _trazabilidadPropiedadServicio.ObtenerVentasRecientesAsync(desde);
            return Ok(result);
        }
    }
}