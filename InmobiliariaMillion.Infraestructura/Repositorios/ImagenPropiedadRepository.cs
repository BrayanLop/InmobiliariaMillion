using MongoDB.Driver;
using MongoDB.Bson;
using InmobiliariaMillion.Dominio.Interfaces.Repositorios;
using InmobiliariaMillion.Dominio.Entidades;
using Microsoft.Extensions.Logging;

namespace InmobiliariaMillion.Infrastructura.Repositorio
{
    public class ImagenPropiedadRepository : IImagenPropiedadRepository
    {
        private readonly IMongoCollection<ImagenPropiedad> _coleccion;
        private readonly ILogger<ImagenPropiedadRepository> _logger;

        public ImagenPropiedadRepository(IMongoDatabase baseDatos, ILogger<ImagenPropiedadRepository> logger)
        {
            _coleccion = baseDatos.GetCollection<ImagenPropiedad>("PropiedadImagen");
            _logger = logger;
        }

        public async Task<ImagenPropiedad> CrearAsync(ImagenPropiedad imagen)
        {
            try
            {
                imagen.IdImagenPropiedad = ObjectId.GenerateNewId().ToString();
                await _coleccion.InsertOneAsync(imagen);
                return imagen;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al crear la imagen de propiedad.");
                throw new Exception($"Error al crear la imagen de propiedad: {ex.Message}");
            }
        }

        public async Task<ImagenPropiedad> ObtenerPorIdAsync(string id)
        {
            try
            {
                return await _coleccion.Find(x => x.IdImagenPropiedad == id).FirstOrDefaultAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al obtener la imagen de propiedad por ID.");
                throw new Exception($"Error al obtener la imagen de propiedad por ID: {ex.Message}");
            }
        }

        public async Task<List<ImagenPropiedad>> ObtenerPorPropiedadAsync(string idPropiedad)
        {
            try
            {
                return await _coleccion.Find(x => x.IdPropiedad == idPropiedad).ToListAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al obtener las imágenes de propiedad por ID de propiedad.");
                throw new Exception($"Error al obtener las imágenes de propiedad por ID de propiedad: {ex.Message}");
            }
        }

        public async Task<ImagenPropiedad> ActualizarAsync(ImagenPropiedad imagen)
        {
            try
            {
                var resultado = await _coleccion.ReplaceOneAsync(x => x.IdImagenPropiedad == imagen.IdImagenPropiedad, imagen);
                return resultado.IsAcknowledged && resultado.ModifiedCount > 0 ? imagen : null;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al actualizar la imagen de propiedad.");
                throw new Exception($"Error al actualizar la imagen de propiedad: {ex.Message}");
            }
        }

        public async Task<bool> EliminarAsync(string id)
        {
            try
            {
                var resultado = await _coleccion.DeleteOneAsync(x => x.IdImagenPropiedad == id);
                return resultado.IsAcknowledged && resultado.DeletedCount > 0;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al eliminar la imagen de propiedad.");
                throw new Exception($"Error al eliminar la imagen de propiedad: {ex.Message}");
            }
        }

        public async Task<bool> ExisteAsync(string id)
        {
            try
            {
                var cantidad = await _coleccion.CountDocumentsAsync(x => x.IdImagenPropiedad == id);
                return cantidad > 0;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al verificar la existencia de la imagen de propiedad.");
                throw new Exception($"Error al verificar la existencia de la imagen de propiedad: {ex.Message}");
            }
        }
    }
}