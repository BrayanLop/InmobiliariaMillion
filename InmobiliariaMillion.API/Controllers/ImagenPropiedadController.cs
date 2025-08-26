using InmobiliariaMillion.API.Models;
using InmobiliariaMillion.Aplicacion.DTOs.Modelos.ImagenPropiedad;
using InmobiliariaMillion.Aplicacion.Servicios.Interfaces;
using InmobiliariaMillion.Infrastructura.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace InmobiliariaMillion.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ImagenPropiedadController : ControllerBase
    {
        private readonly IImagenPropiedadServicio _propiedadImagenServicio;
        private readonly IArchivoServicio _archivoServicio;
        private readonly ILogger<ImagenPropiedadController> _logger;

        public ImagenPropiedadController(
            IImagenPropiedadServicio propiedadImagenServicio,
            IArchivoServicio archivoServicio,
            ILogger<ImagenPropiedadController> logger)
        {
            _propiedadImagenServicio = propiedadImagenServicio;
            _archivoServicio = archivoServicio;
            _logger = logger;
        }

        /// <summary>
        /// Obtiene todas las imágenes de propiedades
        /// </summary>
        [HttpGet("Obtener")]
        [ProducesResponseType(typeof(List<ImagenPropiedadOutputDto>), 200)]
        [ProducesResponseType(500)]
        public async Task<ActionResult<List<ImagenPropiedadOutputDto>>> Obtener()
        {
            try
            {
                var imagenes = await _propiedadImagenServicio.ObtenerPropiedadImagenAsync();
                return Ok(imagenes);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al obtener imágenes de propiedades");
                return StatusCode(500, "Error interno del servidor");
            }
        }

        /// <summary>
        /// Obtiene imágenes por propiedad
        /// </summary>
        [HttpGet("id")]
        [ProducesResponseType(typeof(List<ImagenPropiedadOutputDto>), 200)]
        [ProducesResponseType(404)]
        [ProducesResponseType(500)]
        public async Task<ActionResult<List<ImagenPropiedadOutputDto>>> ObtenerPorId(string idPropiedad)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(idPropiedad))
                    return BadRequest("El ID de la propiedad es requerido.");

                var imagenes = await _propiedadImagenServicio.ObtenerImagenesPorPropiedadAsync(idPropiedad);
                if (imagenes == null || imagenes.Count == 0)
                    return NotFound($"No se encontraron imágenes para la propiedad con ID: {idPropiedad}");

                return Ok(imagenes);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al obtener imágenes para la propiedad {IdPropiedad}", idPropiedad);
                return StatusCode(500, "Error interno del servidor");
            }
        }

        /// <summary>
        /// Agrega una imagen a una propiedad
        /// </summary>
        [HttpPost("Crear")]
        [ProducesResponseType(typeof(ImagenPropiedadOutputDto), 201)]
        [ProducesResponseType(400)]
        [ProducesResponseType(500)]
        public async Task<ActionResult<ImagenPropiedadOutputDto>> Crear([FromBody] ImagenPropiedadInputDto dto)
        {
            try
            {
                if (dto == null || string.IsNullOrWhiteSpace(dto.IdPropiedad) || string.IsNullOrWhiteSpace(dto.Archivo))
                    return BadRequest("Datos de la imagen o propiedad incompletos.");

                var imagen = await _propiedadImagenServicio.AgregarImagenAPropiedadAsync(dto);
                return CreatedAtAction(nameof(ObtenerPorId), new { idPropiedad = imagen.IdPropiedad }, imagen);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al crear imagen para propiedad {IdPropiedad}", dto?.IdPropiedad);
                return StatusCode(500, "Error interno del servidor");
            }
        }

        /// <summary>
        /// Elimina una imagen por su ID
        /// </summary>
        [HttpDelete("Eliminar/{id}")]
        [ProducesResponseType(204)]
        [ProducesResponseType(404)]
        [ProducesResponseType(500)]
        public async Task<IActionResult> Eliminar(string id)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(id))
                    return BadRequest("El ID de la imagen es requerido.");

                var eliminado = await _propiedadImagenServicio.EliminarImagenAsync(id);
                if (!eliminado)
                    return NotFound($"No se encontró la imagen con ID: {id}");

                return NoContent();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al eliminar imagen {IdImagen}", id);
                return StatusCode(500, "Error interno del servidor");
            }
        }

        /// <summary>
        /// Elimina todas las imágenes de una propiedad
        /// </summary>
        [HttpDelete("EliminarPorPropiedad/{id}")]
        [ProducesResponseType(204)]
        [ProducesResponseType(404)]
        [ProducesResponseType(500)]
        public async Task<IActionResult> EliminarPorPropiedad(string id)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(id))
                    return BadRequest("El ID de la propiedad es requerido.");

                var eliminado = await _propiedadImagenServicio.EliminarPropiedadAsync(id);
                if (!eliminado)
                    return NotFound($"No se encontraron imágenes para la propiedad con ID: {id}");

                return NoContent();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al eliminar imágenes de propiedad {IdPropiedad}", id);
                return StatusCode(500, "Error interno del servidor");
            }
        }

        /// <summary>
        /// Habilita o deshabilita una imagen
        /// </summary>
        [HttpPut("Habilitar/{id}")]
        [ProducesResponseType(204)]
        [ProducesResponseType(404)]
        [ProducesResponseType(500)]
        public async Task<IActionResult> Habilitar(string id, [FromQuery] bool habilitar)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(id))
                    return BadRequest("El ID de la imagen es requerido.");

                var resultado = await _propiedadImagenServicio.HabilitarImagenAsync(id, habilitar);
                if (!resultado)
                    return NotFound($"No se encontró la imagen con ID: {id}");

                return NoContent();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al {Accion} imagen {IdImagen}", habilitar ? "habilitar" : "deshabilitar", id);
                return StatusCode(500, "Error interno del servidor");
            }
        }

        [HttpPost("Subir")]
        public async Task<IActionResult> SubirImagen([FromBody] ArchivoRequest request)
        {
            try
            {
                if (string.IsNullOrEmpty(request.ImagenBase64) || string.IsNullOrEmpty(request.NombreArchivo))
                {
                    return BadRequest("La imagen o el nombre del archivo no pueden estar vacíos");
                }

                // Usar el servicio de infraestructura directamente
                string rutaImagen = await _archivoServicio.GuardarImagenBase64Async(
                    request.ImagenBase64, 
                    request.NombreArchivo
                );

                return Ok(new { url = rutaImagen });
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al subir imagen");
                return StatusCode(500, $"Error interno: {ex.Message}");
            }
        }
    }
}