using MongoDB.Driver;
using MongoDB.Bson;
using InmobiliariaMillion.Dominio;
using InmobiliariaMillion.Dominio.Interfaces;

namespace InmobiliariaMillion.Infrastructura.Repositorio
{
    public class TrazabilidadPropiedadRepository : ITrazabilidadPropiedadRepository
    {
        private readonly IMongoCollection<TrazabilidadPropiedad> _coleccion;

        public TrazabilidadPropiedadRepository(IMongoDatabase baseDatos)
        {
            _coleccion = baseDatos.GetCollection<TrazabilidadPropiedad>("trazabilidades_propiedades");
        }

        public async Task<TrazabilidadPropiedad> CrearAsync(TrazabilidadPropiedad trazabilidad)
        {
            trazabilidad.IdTrazabilidadPropiedad = ObjectId.GenerateNewId().ToString();
            await _coleccion.InsertOneAsync(trazabilidad);
            return trazabilidad;
        }

        public async Task<TrazabilidadPropiedad> ObtenerPorIdAsync(string id)
        {
            return await _coleccion.Find(x => x.IdTrazabilidadPropiedad == id).FirstOrDefaultAsync();
        }

        public async Task<List<TrazabilidadPropiedad>> ObtenerPorPropiedadAsync(string idPropiedad)
        {
            return await _coleccion.Find(x => x.IdPropiedad == idPropiedad)
                .SortByDescending(x => x.FechaVenta)
                .ToListAsync();
        }

        public async Task<TrazabilidadPropiedad> ActualizarAsync(TrazabilidadPropiedad trazabilidad)
        {
            var resultado = await _coleccion.ReplaceOneAsync(x => x.IdTrazabilidadPropiedad == trazabilidad.IdTrazabilidadPropiedad, trazabilidad);
            return resultado.IsAcknowledged && resultado.ModifiedCount > 0 ? trazabilidad : null;
        }

        public async Task<bool> EliminarAsync(string id)
        {
            var resultado = await _coleccion.DeleteOneAsync(x => x.IdTrazabilidadPropiedad == id);
            return resultado.IsAcknowledged && resultado.DeletedCount > 0;
        }

        public async Task<List<TrazabilidadPropiedad>> ObtenerVentasRecientesAsync(DateTime desde)
        {
            return await _coleccion.Find(x => x.FechaVenta >= desde)
                .SortByDescending(x => x.FechaVenta)
                .ToListAsync();
        }
    }
}