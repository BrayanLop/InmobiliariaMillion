using Microsoft.AspNetCore.Mvc;
using InmobiliariaMillion.Application.DTOs;
using InmobiliariaMillion.Application.Servicios;

namespace InmobiliariaMillion.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Produces("application/json")]
    public class PropiedadesController : ControllerBase
    {
        private readonly IPropiedadApiService _propiedadApiService;
        private readonly ILogger<PropiedadesController> _logger;

        public PropiedadesController(
            IPropiedadApiService propiedadApiService,
            ILogger<PropiedadesController> logger)
        {
            _propiedadApiService = propiedadApiService;
            _logger = logger;
        }

        /// <summary>
        /// Obtiene propiedades con filtros
        /// </summary>
        /// <param name="name">Filtro por nombre</param>
        /// <param name="address">Filtro por dirección</param>
        /// <param name="minPrice">Precio mínimo</param>
        /// <param name="maxPrice">Precio máximo</param>
        /// <returns>Lista de propiedades filtradas</returns>
        [HttpGet("filtrar")]
        [ProducesResponseType(typeof(List<PropiedadDto>), 200)]
        [ProducesResponseType(400)]
        [ProducesResponseType(500)]
        public async Task<ActionResult<List<PropiedadDto>>> ObtenerPropiedadesFiltradas(
            [FromQuery] string? name = null,
            [FromQuery] string? address = null,
            [FromQuery] decimal? minPrice = null,
            [FromQuery] decimal? maxPrice = null)
        {
            try
            {
                // Validar rango de precios
                if (minPrice.HasValue && maxPrice.HasValue && minPrice > maxPrice)
                {
                    return BadRequest("El precio mínimo no puede ser mayor al precio máximo");
                }

                var filtros = new FiltrosPropiedadDto
                {
                    Name = name,
                    Address = address,
                    MinPrice = minPrice,
                    MaxPrice = maxPrice
                };

                var propiedades = await _propiedadApiService.ObtenerPropiedadesFiltradosAsync(filtros);
                return Ok(propiedades);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al obtener propiedades filtradas con parámetros: name={Name}, address={Address}, minPrice={MinPrice}, maxPrice={MaxPrice}", 
                    name, address, minPrice, maxPrice);
                return StatusCode(500, "Error interno del servidor");
            }
        }

        /// <summary>
        /// Obtiene una propiedad por ID
        /// </summary>
        /// <param name="id">ID de la propiedad</param>
        /// <returns>Propiedad encontrada</returns>
        [HttpGet("{id}")]
        [ProducesResponseType(typeof(PropiedadDto), 200)]
        [ProducesResponseType(404)]
        [ProducesResponseType(500)]
        public async Task<ActionResult<PropiedadDto>> ObtenerPropiedadPorId(string id)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(id))
                {
                    return BadRequest("El ID de la propiedad es requerido");
                }

                var propiedad = await _propiedadApiService.ObtenerPropiedadPorIdAsync(id);
                
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

        /// <summary>
        /// Crea una nueva propiedad
        /// </summary>
        /// <param name="propiedadDto">Datos de la propiedad a crear</param>
        /// <returns>Propiedad creada</returns>
        [HttpPost]
        [ProducesResponseType(typeof(PropiedadDto), 201)]
        [ProducesResponseType(400)]
        [ProducesResponseType(500)]
        public async Task<ActionResult<PropiedadDto>> CrearPropiedad([FromBody] PropiedadDto propiedadDto)
        {
            try
            {
                if (propiedadDto == null)
                {
                    return BadRequest("Los datos de la propiedad son requeridos.");
                }

                // Puedes agregar validaciones adicionales aquí si lo necesitas

                var propiedadCreada = await _propiedadApiService.CrearPropiedadAsync(propiedadDto);

                return CreatedAtAction(nameof(ObtenerPropiedadPorId), new { id = propiedadCreada.IdPropiedad }, propiedadCreada);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al crear la propiedad");
                return StatusCode(500, "Error interno del servidor");
            }
        }
    }
}