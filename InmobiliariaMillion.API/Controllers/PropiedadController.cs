using InmobiliariaMillion.Aplicacion.DTOs.Modelos;
using InmobiliariaMillion.Aplicacion.DTOs.Modelos.Propiedad;
using InmobiliariaMillion.Aplicacion.DTOs.Utilidades;
using InmobiliariaMillion.Aplicacion.Servicios.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace InmobiliariaMillion.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Produces("application/json")]
    public class PropiedadController : ControllerBase
    {
        private readonly IPropiedadServicio _propiedadServicio;

        public PropiedadController(IPropiedadServicio propiedadApiService)
        {
            _propiedadServicio = propiedadApiService;
        }

        /// <summary>
        /// Obtiene propiedades con filtros
        /// </summary>
        /// <param name="nombre">Filtro por nombre</param>
        /// <param name="direccion">Filtro por dirección</param>
        /// <param name="precioMinimo">Precio mínimo</param>
        /// <param name="PrecioMaximo">Precio máximo</param>
        /// <returns>Lista de propiedades filtradas</returns>
        [HttpGet("Obtener")]
        [ProducesResponseType(typeof(List<PropiedadOutputDto>), 200)]
        [ProducesResponseType(400)]
        [ProducesResponseType(500)]
        public async Task<ActionResult<List<PropiedadOutputDto>>> Obtener([FromQuery] FiltrosPropiedadDto filtros)
        {
            try
            {
                if (filtros.PrecioMinimo.HasValue && filtros.PrecioMaximo.HasValue && filtros.PrecioMinimo > filtros.PrecioMaximo)
                {
                    return BadRequest("El precio mínimo no puede ser mayor al precio máximo");
                }

                var propiedades = await _propiedadServicio.ObtenerPropiedadesAsync(filtros);
                return Ok(propiedades);
            }
            catch (Exception ex)
            {
                return StatusCode(500, "Error interno del servidor");
            }
        }

        /// <summary>
        /// Crea una nueva propiedad
        /// </summary>
        /// <param name="propiedadDto">Datos de la propiedad a crear</param>
        /// <returns>Propiedad creada</returns>
        [HttpPost("Crear")]
        [ProducesResponseType(typeof(PropiedadOutputDto), 201)]
        [ProducesResponseType(400)]
        [ProducesResponseType(500)]
        public async Task<ActionResult<PropiedadOutputDto>> Crear([FromBody] PropiedadInputDto propiedadDto)
        {
            try
            {
                if (propiedadDto == null)
                {
                    return BadRequest("Los datos de la propiedad son requeridos.");
                }

                var propiedadCreada = await _propiedadServicio.CrearPropiedadAsync(propiedadDto);

                return CreatedAtAction(nameof(ObtenerPorId), new { id = propiedadCreada.IdPropiedad }, propiedadCreada);
            }
            catch (Exception ex)
            {
                return StatusCode(500, "Error interno del servidor");
            }
        }

        /// <summary>
        /// Obtiene una propiedad por ID
        /// </summary>
        /// <param name="id">ID de la propiedad</param>
        /// <returns>Propiedad encontrada</returns>
        [HttpGet("{id}")]
        [ProducesResponseType(typeof(PropiedadOutputDto), 200)]
        [ProducesResponseType(404)]
        [ProducesResponseType(500)]
        public async Task<ActionResult<PropiedadOutputDto>> ObtenerPorId(string id)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(id))
                {
                    return BadRequest("El ID de la propiedad es requerido");
                }

                var propiedad = await _propiedadServicio.ObtenerPropiedadPorIdAsync(id);
                
                if (propiedad == null)
                {
                    return NotFound($"No se encontró la propiedad con ID: {id}");
                }

                return Ok(propiedad);
            }
            catch (Exception ex)
            {
                return StatusCode(500, "Error interno del servidor");
            }
        }
        /// <summary>
        /// Actualiza una propiedad existente
        /// </summary>
        /// <param name="id">ID de la propiedad a actualizar</param>
        /// <param name="propiedadDto">Datos actualizados de la propiedad</param>
        /// <returns>Propiedad actualizada</returns>
        [HttpPut("Actualizar/{id}")]
        [ProducesResponseType(typeof(PropiedadOutputDto), 200)]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        [ProducesResponseType(500)]
        public async Task<ActionResult<PropiedadOutputDto>> Actualizar(string id, [FromBody] PropiedadInputDto propiedadDto)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(id) || propiedadDto == null)
                {
                    return BadRequest("Los datos del propietario son requeridos.");
                }

                propiedadDto.IdPropiedad = id;
                var propiedadActualizada = await _propiedadServicio.ActualizarPropiedadAsync(propiedadDto);

                if (propiedadActualizada == null)
                {
                    return NotFound($"No se encontró la propiedad con ID: {id}");
                }

                return Ok(propiedadActualizada);
            }
            catch (Exception)
            {
                return StatusCode(500, "Error interno del servidor");
            }
        }

        /// <summary>
        /// Elimina una propiedad por ID
        /// </summary>
        /// <param name="id">ID de la propiedad a eliminar</param>
        /// <returns>Resultado de la eliminación</returns>
        [HttpDelete("Eliminar/{id}")]
        [ProducesResponseType(204)]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        [ProducesResponseType(500)]
        public async Task<IActionResult> Eliminar(string id)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(id))
                {
                    return BadRequest("El ID de la propiedad es requerido.");
                }

                var eliminado = await _propiedadServicio.EliminarPropiedadAsync(id);

                if (!eliminado)
                {
                    return NotFound($"No se encontró la propiedad con ID: {id}");
                }

                return NoContent();
            }
            catch (Exception)
            {
                return StatusCode(500, "Error interno del servidor");
            }
        }
    }
}