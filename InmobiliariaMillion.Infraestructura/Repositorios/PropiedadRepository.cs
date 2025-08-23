using MongoDB.Driver;
using MongoDB.Bson;
using InmobiliariaMillion.Dominio.Interfaces.Repositorios;
using InmobiliariaMillion.Dominio.Entidades;

namespace InmobiliariaMillion.Infrastructura.Repositorio
{
    public class PropiedadRepository : IPropiedadRepository
    {
        private readonly IMongoCollection<Propiedad> _coleccion;

        public PropiedadRepository(IMongoDatabase baseDatos)
        {
            _coleccion = baseDatos.GetCollection<Propiedad>("Propiedad");
        }

        public async Task<Propiedad> CrearAsync(Propiedad propiedad)
        {
            propiedad.IdPropiedad = ObjectId.GenerateNewId().ToString();
            await _coleccion.InsertOneAsync(propiedad);
            return propiedad;
        }

        public async Task<Propiedad> ObtenerPorIdAsync(string id)
        {
            return await _coleccion.Find(x => x.IdPropiedad == id).FirstOrDefaultAsync();
        }

        public async Task<List<Propiedad>> ObtenerAsync(string? nombre, string? direccion, decimal? precioMinimo, decimal? precioMaximo)
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

        public async Task<Propiedad> ActualizarAsync(Propiedad propiedad)
        {
            propiedad._id = propiedad._id;
            var resultado = await _coleccion.ReplaceOneAsync(x => x.IdPropiedad == propiedad.IdPropiedad, propiedad);
            return resultado.IsAcknowledged && resultado.ModifiedCount > 0 ? propiedad : null;
        }

        public async Task<bool> EliminarAsync(string id)
        {
            var resultado = await _coleccion.DeleteOneAsync(x => x.IdPropiedad == id);
            return resultado.IsAcknowledged && resultado.DeletedCount > 0;
        }
        public async Task<bool> ExisteAsync(string id)
        {
            var count = await _coleccion.CountDocumentsAsync(x => x.IdPropiedad == id);
            return count > 0;
        }
    }
}