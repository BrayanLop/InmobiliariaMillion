using InmobiliariaMillion.Aplicacion.DTOs.Modelos;
using InmobiliariaMillion.Aplicacion.DTOs.Modelos.TrazabilidadPropiedad;
using InmobiliariaMillion.Aplicacion.Servicios.Interfaces;
using Microsoft.AspNetCore.Mvc;

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

        [HttpPost("Crear")]
        [ProducesResponseType(typeof(TrazabilidadPropiedadOutputDto), 201)]
        [ProducesResponseType(400)]
        [ProducesResponseType(500)]
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

        [HttpGet("{id}")]
        [ProducesResponseType(typeof(TrazabilidadPropiedadOutputDto), 200)]
        [ProducesResponseType(404)]
        [ProducesResponseType(500)]
        public async Task<ActionResult<TrazabilidadPropiedadOutputDto>> ObtenerPorId(string id)
        {
            var result = await _trazabilidadPropiedadServicio.ObtenerTrazabilidadPropiedadPorIdAsync(id);
            if (result == null)
                return NotFound();
            return Ok(result);
        }

        [HttpPut("{id}")]
        [ProducesResponseType(typeof(TrazabilidadPropiedadOutputDto), 200)]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        [ProducesResponseType(500)]
        public async Task<ActionResult<TrazabilidadPropiedadOutputDto>> Actualizar(string id, [FromBody] TrazabilidadPropiedadInputDto trazabilidadPropiedadDto)
        {
            if (string.IsNullOrWhiteSpace(id) || trazabilidadPropiedadDto == null)
            {
                return BadRequest("Los datos del registro son requeridos.");
            }

            trazabilidadPropiedadDto.IdTrazabilidadPropiedad = id;
            var result = await _trazabilidadPropiedadServicio.ActualizarTrazabilidadPropiedadAsync(trazabilidadPropiedadDto);
            if (result == null) return NotFound();

            return Ok(result);
        }

        [HttpDelete("{id}")]
        [ProducesResponseType(204)]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        [ProducesResponseType(500)]
        public async Task<ActionResult> Eliminar(string id)
        {
            var eliminado = await _trazabilidadPropiedadServicio.EliminarTrazabilidadPropiedadAsync(id);
            if (!eliminado) return NotFound();

            return NoContent();
        }

        [HttpGet("ventas-recientes")]
        [ProducesResponseType(400)]
        [ProducesResponseType(500)]
        public async Task<ActionResult<List<TrazabilidadPropiedadOutputDto>>> ObtenerVentasRecientes([FromQuery] DateTime desde)
        {
            var result = await _trazabilidadPropiedadServicio.ObtenerVentasRecientesAsync(desde);
            return Ok(result);
        }
    }
}