using MongoDB.Driver;
using MongoDB.Bson;
using Microsoft.Extensions.Logging;
using InmobiliariaMillion.Dominio.Interfaces.Repositorios;
using InmobiliariaMillion.Dominio.Entidades;

namespace InmobiliariaMillion.Infrastructure.Repositorio
{
    public class PropiedadRepository : IPropiedadRepository
    {
        private readonly IMongoCollection<Propiedad> _coleccion;
        private readonly ILogger<PropiedadRepository> _logger;

        public PropiedadRepository(IMongoDatabase baseDatos, ILogger<PropiedadRepository> logger)
        {
            _coleccion = baseDatos.GetCollection<Propiedad>("Propiedad");
            _logger = logger;
        }

        public async Task<Propiedad> CrearAsync(Propiedad propiedad)
        {
            try
            {
                propiedad.IdPropiedad = ObjectId.GenerateNewId().ToString();
                await _coleccion.InsertOneAsync(propiedad);
                return propiedad;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al crear la propiedad.");
                throw new Exception($"Error al crear la propiedad: {ex.Message}");
            }
        }

        public async Task<Propiedad> ObtenerPorIdAsync(string id)
        {
            try
            {
                return await _coleccion.Find(x => x.IdPropiedad == id).FirstOrDefaultAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al obtener la propiedad por ID: {Id}", id);
                throw new Exception($"Error al obtener la propiedad por ID: {ex.Message}");
            }
        }

        public async Task<List<Propiedad>> ObtenerAsync(string? nombre, string? direccion, decimal? precioMinimo, decimal? precioMaximo)
        {
            try
            {
                var builder = Builders<Propiedad>.Filter;
                var filtro = builder.Empty;

                if (!string.IsNullOrEmpty(nombre))
                    filtro &= builder.Regex(x => x.Nombre, new BsonRegularExpression(nombre, "i"));

                if (!string.IsNullOrEmpty(direccion))
                    filtro &= builder.Regex(x => x.Direccion, new BsonRegularExpression(direccion, "i"));

                if (precioMinimo.HasValue)
                    filtro &= builder.Gte(x => x.Precio, precioMinimo.Value);

                if (precioMaximo.HasValue)
                    filtro &= builder.Lte(x => x.Precio, precioMaximo.Value);

                return await _coleccion.Find(filtro).ToListAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al obtener las propiedades con filtros: Nombre={Nombre}, Direccion={Direccion}, PrecioMinimo={PrecioMinimo}, PrecioMaximo={PrecioMaximo}", nombre, direccion, precioMinimo, precioMaximo);
                throw new Exception($"Error al obtener las propiedades: {ex.Message}");
            }
        }

        public async Task<Propiedad> ActualizarAsync(Propiedad propiedad)
        {
            try
            {
                var resultado = await _coleccion.ReplaceOneAsync(x => x.IdPropiedad == propiedad.IdPropiedad, propiedad);
                return resultado.IsAcknowledged && resultado.ModifiedCount > 0 ? propiedad : null;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al actualizar la propiedad con ID: {Id}", propiedad.IdPropiedad);
                throw new Exception($"Error al actualizar la propiedad: {ex.Message}");
            }
        }

        public async Task<bool> EliminarAsync(string id)
        {
            try
            {
                var resultado = await _coleccion.DeleteOneAsync(x => x.IdPropiedad == id);
                return resultado.IsAcknowledged && resultado.DeletedCount > 0;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al eliminar la propiedad con ID: {Id}", id);
                throw new Exception($"Error al eliminar la propiedad: {ex.Message}");
            }
        }
        public async Task<bool> ExisteAsync(string id)
        {
            try
            {
                var count = await _coleccion.CountDocumentsAsync(x => x.IdPropiedad == id);
                return count > 0;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al verificar la existencia de la propiedad con ID: {Id}", id);
                throw new Exception($"Error al verificar la existencia de la propiedad: {ex.Message}");
            }
        }
    }
}