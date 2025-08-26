using MongoDB.Driver;
using MongoDB.Bson;
using InmobiliariaMillion.Dominio.Interfaces.Repositorios;
using InmobiliariaMillion.Dominio.Entidades;
using Microsoft.Extensions.Logging;

namespace InmobiliariaMillion.Infrastructura.Repositorio
{
    public class TrazabilidadPropiedadRepository : ITrazabilidadPropiedadRepository
    {
        private readonly IMongoCollection<TrazabilidadPropiedad> _coleccion;
        private readonly ILogger<TrazabilidadPropiedadRepository> _logger;

        public TrazabilidadPropiedadRepository(IMongoDatabase baseDatos, ILogger<TrazabilidadPropiedadRepository> logger)
        {
            _coleccion = baseDatos.GetCollection<TrazabilidadPropiedad>("PropiedadTrazabilidad");
            _logger = logger;
        }

        public async Task<TrazabilidadPropiedad> CrearAsync(TrazabilidadPropiedad trazabilidad)
        {
            try
            {
                trazabilidad.IdTrazabilidadPropiedad = ObjectId.GenerateNewId().ToString();
                await _coleccion.InsertOneAsync(trazabilidad);
                return trazabilidad;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al crear trazabilidad de propiedad");
                throw new Exception($"Error al crear trazabilidad de propiedad: {ex.Message}");
            }
        }

        public async Task<TrazabilidadPropiedad> ObtenerPorIdAsync(string id)
        {
            try
            {
                return await _coleccion.Find(x => x.IdTrazabilidadPropiedad == id).FirstOrDefaultAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al obtener trazabilidad de propiedad por ID");
                throw new Exception($"Error al obtener trazabilidad de propiedad por ID: {ex.Message}");
            }
        }

        public async Task<List<TrazabilidadPropiedad>> ObtenerPorPropiedadAsync(string idPropiedad)
        {
            try
            {
                return await _coleccion.Find(x => x.IdPropiedad == idPropiedad)
                    .SortByDescending(x => x.FechaVenta)
                    .ToListAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al obtener trazabilidad de propiedad por ID de propiedad");
                throw new Exception($"Error al obtener trazabilidad de propiedad por ID de propiedad: {ex.Message}");
            }
        }

        public async Task<TrazabilidadPropiedad> ActualizarAsync(TrazabilidadPropiedad trazabilidad)
        {
            try
            {
                var resultado = await _coleccion.ReplaceOneAsync(x => x.IdTrazabilidadPropiedad == trazabilidad.IdTrazabilidadPropiedad, trazabilidad);
                return resultado.IsAcknowledged && resultado.ModifiedCount > 0 ? trazabilidad : null;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al actualizar trazabilidad de propiedad");
                throw new Exception($"Error al actualizar trazabilidad de propiedad: {ex.Message}");
            }
        }

        public async Task<bool> EliminarAsync(string id)
        {
            try
            {
                var resultado = await _coleccion.DeleteOneAsync(x => x.IdTrazabilidadPropiedad == id);
                return resultado.IsAcknowledged && resultado.DeletedCount > 0;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al eliminar trazabilidad de propiedad");
                throw new Exception($"Error al eliminar trazabilidad de propiedad: {ex.Message}");
            }
        }

        public async Task<List<TrazabilidadPropiedad>> ObtenerVentasRecientesAsync(DateTime desde)
        {
            try
            {
                return await _coleccion.Find(x => x.FechaVenta >= desde)
                    .SortByDescending(x => x.FechaVenta)
                    .ToListAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al obtener ventas recientes de trazabilidad de propiedad");
                throw new Exception($"Error al obtener ventas recientes de trazabilidad de propiedad: {ex.Message}");
            }
        }
    }
}