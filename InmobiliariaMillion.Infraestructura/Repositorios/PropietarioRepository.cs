using InmobiliariaMillion.Dominio.Entidades;
using InmobiliariaMillion.Dominio.Interfaces.Repositorios;
using Microsoft.Extensions.Logging;
using MongoDB.Bson;
using MongoDB.Driver;

namespace InmobiliariaMillion.Infrastructura.Repositorio
{
    public class PropietarioRepository : IPropietarioRepository
    {
        private readonly IMongoCollection<Propietario> _coleccion;
        private readonly ILogger<PropietarioRepository> _logger;

        public PropietarioRepository(IMongoDatabase baseDatos, ILogger<PropietarioRepository> logger)
        {
            _coleccion = baseDatos.GetCollection<Propietario>("Propietario");
            _logger = logger;
        }

        public async Task<List<Propietario>> ObtenerAsync(string nombre)
        {
            try
            {
                var builder = Builders<Propietario>.Filter;
                var filtro = builder.Empty;

                if (!string.IsNullOrEmpty(nombre))
                    filtro &= builder.Regex(x => x.Nombre, new BsonRegularExpression(nombre, "i"));

                return await _coleccion.Find(filtro).ToListAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al obtener propietarios con nombre: {Nombre}", nombre);
                throw new Exception($"Error al obtener propietarios: {ex.Message}");
            }
        }

        public async Task<Propietario> ObtenerPorIdAsync(string id)
        {
            try
            {
                return await _coleccion.Find(x => x.IdPropietario == id).FirstOrDefaultAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al obtener propietario por ID: {Id}", id);
                throw new Exception($"Error al obtener propietario por ID: {ex.Message}");
            }
        }

        public async Task<Propietario> CrearAsync(Propietario propietario)
        {
            try
            {
                propietario.IdPropietario = ObjectId.GenerateNewId().ToString();
                await _coleccion.InsertOneAsync(propietario);
                return propietario;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al crear propietario.");
                throw new Exception($"Error al crear propietario: {ex.Message}");
            }
        }

        public async Task<Propietario> ActualizarAsync(Propietario propietario)
        {
            try
            {
                var resultado = await _coleccion.ReplaceOneAsync(x => x.IdPropietario == propietario.IdPropietario, propietario);
                return resultado.IsAcknowledged && resultado.ModifiedCount > 0 ? propietario : null;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al actualizar propietario con ID: {Id}", propietario.IdPropietario);
                throw new Exception($"Error al actualizar propietario: {ex.Message}");
            }
        }

        public async Task<bool> EliminarAsync(string id)
        {
            try
            {
                var resultado = await _coleccion.DeleteOneAsync(x => x.IdPropietario == id);
                return resultado.IsAcknowledged && resultado.DeletedCount > 0;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al eliminar propietario con ID: {Id}", id);
                throw new Exception($"Error al eliminar propietario: {ex.Message}");
            }
        }

        public async Task<bool> ExisteAsync(string id)
        {
            try
            {
                var cantidad = await _coleccion.CountDocumentsAsync(x => x.IdPropietario == id);
                return cantidad > 0;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al verificar la existencia del propietario con ID: {Id}", id);
                throw new Exception($"Error al verificar la existencia del propietario: {ex.Message}");
            }
        }
    }
}