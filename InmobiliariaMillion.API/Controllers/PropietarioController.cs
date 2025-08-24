using InmobiliariaMillion.Aplicacion.DTOs.Modelos.Propietario;
using InmobiliariaMillion.Aplicacion.DTOs.Utilidades;
using InmobiliariaMillion.Aplicacion.Servicios.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace InmobiliariaMillion.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Produces("application/json")]
    public class PropietarioController : ControllerBase
    {
        private readonly IPropietarioServicio _propietarioServicio;

        public PropietarioController(IPropietarioServicio propietarioServicio)
        {
            _propietarioServicio = propietarioServicio;
        }

        /// <summary>
        /// Obtiene propietarios con filtros
         /// <param name="nombre">Filtro por nombre</param>
        /// </summary>
        [HttpGet("Obtener")]
        [ProducesResponseType(typeof(List<PropietarioOutputDto>), 200)]
        [ProducesResponseType(400)]
        [ProducesResponseType(500)]
        public async Task<ActionResult<List<PropietarioOutputDto>>> Obtener(
            [FromQuery] FiltrosPropietarioDto filtrosPropietarioDto)
        {
            try
            {
                var propietarios = await _propietarioServicio.ObtenerPropietariosAsync(filtrosPropietarioDto);
                return Ok(propietarios);
            }
            catch (Exception ex)
            {
                return StatusCode(500, "Error interno del servidor");
            }
        }

        /// <summary>
        /// Crea un nuevo propietario
        /// </summary>
        [HttpPost("Crear")]
        [ProducesResponseType(typeof(PropietarioOutputDto), 201)]
        [ProducesResponseType(400)]
        [ProducesResponseType(500)]
        public async Task<ActionResult<PropietarioOutputDto>> Crear([FromBody] PropietarioInputDto propietarioDto)
        {
            try
            {
                if (propietarioDto == null)
                {
                    return BadRequest("Los datos del propietario son requeridos.");
                }

                var propietarioCreado = await _propietarioServicio.CrearPropietarioAsync(propietarioDto);

                return CreatedAtAction(nameof(ObtenerPropietarioPorId), new { id = propietarioCreado.IdPropietario }, propietarioCreado);
            }
            catch (Exception ex)
            {
                return StatusCode(500, "Error interno del servidor");
            }
        }

        /// <summary>
        /// Obtiene un propietario por ID
        /// </summary>
        [HttpGet("{id}")]
        [ProducesResponseType(typeof(PropietarioOutputDto), 200)]
        [ProducesResponseType(404)]
        [ProducesResponseType(500)]
        public async Task<ActionResult<PropietarioOutputDto>> ObtenerPropietarioPorId(string id)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(id))
                {
                    return BadRequest("El ID del propietario es requerido");
                }

                var propietario = await _propietarioServicio.ObtenerPropietarioPorIdAsync(id);

                if (propietario == null)
                {
                    return NotFound($"No se encontró el propietario con ID: {id}");
                }

                return Ok(propietario);
            }
            catch (Exception ex)
            {
                return StatusCode(500, "Error interno del servidor");
            }
        }
        /// <summary>
        /// Actualiza un propietario existente
        /// </summary>
        [HttpPut("Actualizar")]
        [ProducesResponseType(typeof(PropietarioOutputDto), 200)]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        [ProducesResponseType(500)]
        public async Task<ActionResult<PropietarioOutputDto>> Actualizar([FromBody] PropietarioInputDto propietarioDto)
        {
            try
            {
                if (propietarioDto == null || string.IsNullOrWhiteSpace(propietarioDto.IdPropietario))
                {
                    return BadRequest("Los datos del propietario son requeridos.");
                }

                var propietarioActualizado = await _propietarioServicio.ActualizarPropietarioAsync(propietarioDto);

                if (propietarioActualizado == null)
                {
                    return NotFound($"No se encontró el propietario con ID: {propietarioDto.IdPropietario}");
                }

                return Ok(propietarioActualizado);
            }
            catch (Exception ex)
            {
                return StatusCode(500, "Error interno del servidor");
            }
        }

        /// <summary>
        /// Elimina un propietario por ID
        /// </summary>
        [HttpDelete("Eliminar/{id}")]
        [ProducesResponseType(200)]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        [ProducesResponseType(500)]
        public async Task<ActionResult> Eliminar(string id)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(id))
                {
                    return BadRequest("El ID del propietario es requerido");
                }

                var eliminado = await _propietarioServicio.EliminarPropietarioAsync(id);

                if (!eliminado)
                {
                    return NotFound($"No se encontró el propietario con ID: {id}");
                }

                return Ok($"Propietario con ID: {id} eliminado correctamente.");
            }
            catch (Exception ex)
            {
                return StatusCode(500, "Error interno del servidor");
            }
        }
    }
}