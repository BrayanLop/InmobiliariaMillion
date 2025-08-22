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
        private readonly ILogger<PropiedadController> _logger;

        public PropiedadController(
            IPropiedadServicio propiedadApiService,
            ILogger<PropiedadController> logger)
        {
            _propiedadServicio = propiedadApiService;
            _logger = logger;
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
        public async Task<ActionResult<List<PropiedadOutputDto>>> Obtener(
            [FromQuery] string? nombre = null,
            [FromQuery] string? direccion = null,
            [FromQuery] decimal? precioMinimo = null,
            [FromQuery] decimal? PrecioMaximo = null)
        {
            try
            {
                if (precioMinimo.HasValue && PrecioMaximo.HasValue && precioMinimo > PrecioMaximo)
                {
                    return BadRequest("El precio mínimo no puede ser mayor al precio máximo");
                }

                var filtros = new FiltrosPropiedadDto
                {
                    Nombre = nombre,
                    Direccion = direccion,
                    PrecioMinimo = precioMinimo,
                    PrecioMaximo = PrecioMaximo
                };

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

                return CreatedAtAction(nameof(ObtenerPropiedadPorId), new { id = propiedadCreada.IdPropiedad }, propiedadCreada);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al crear la propiedad");
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
        public async Task<ActionResult<PropiedadOutputDto>> ObtenerPropiedadPorId(string id)
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
                _logger.LogError(ex, "Error al obtener propiedad por ID: {Id}", id);
                return StatusCode(500, "Error interno del servidor");
            }
        }
    }
}