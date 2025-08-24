using Microsoft.AspNetCore.Mvc;
using InmobiliariaMillion.Aplicacion.DTOs.Modelos.ImagenPropiedad;
using InmobiliariaMillion.Aplicacion.Servicios.Interfaces;
using System.IO;

namespace InmobiliariaMillion.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Produces("application/json")]
    public class ImagenPropiedadController : ControllerBase
    {
        private readonly IImagenPropiedadServicio _propiedadImagenServicio;

        public ImagenPropiedadController(IImagenPropiedadServicio propiedadImagenServicio)
        {
            _propiedadImagenServicio = propiedadImagenServicio;
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
            catch (Exception)
            {
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
            catch (Exception)
            {
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
            catch (Exception)
            {
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
            catch (Exception)
            {
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
            catch (Exception)
            {
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
            catch (Exception)
            {
                return StatusCode(500, "Error interno del servidor");
            }
        }

        /// <summary>
        /// Sube un archivo al servidor
        /// </summary>
        //[HttpPost("SubirArchivo")]
        //[Consumes("multipart/form-data")]
        //[ProducesResponseType(typeof(string), 201)]
        //[ProducesResponseType(400)]
        //public async Task<IActionResult> SubirArchivo([FromForm] IFormFile archivo)
        //{
        //    if (archivo == null || archivo.Length == 0)
        //        return BadRequest("No se ha enviado ningún archivo.");

        //    var carpetaDestino = Path.Combine(Directory.GetCurrentDirectory(), "uploads");
        //    if (!Directory.Exists(carpetaDestino))
        //        Directory.CreateDirectory(carpetaDestino);

        //    var nombreArchivo = Guid.NewGuid() + Path.GetExtension(archivo.FileName);
        //    var rutaCompleta = Path.Combine(carpetaDestino, nombreArchivo);

        //    using (var stream = new FileStream(rutaCompleta, FileMode.Create))
        //    {
        //        await archivo.CopyToAsync(stream);
        //    }

        //    var rutaRelativa = Path.Combine("uploads", nombreArchivo);
        //    return Created("", rutaRelativa);
        //}
    }
}