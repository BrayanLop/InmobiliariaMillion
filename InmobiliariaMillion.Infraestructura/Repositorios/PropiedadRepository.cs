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
            var cantidad = await _coleccion.CountDocumentsAsync(x => x.IdPropiedad == id);
            return cantidad > 0;
        }

        public async Task<List<Propiedad>> ObtenerPorPropietarioAsync(string idPropietario)
        {
            return await _coleccion.Find(x => x.IdPropietario == idPropietario).ToListAsync();
        }

        public async Task<List<Propiedad>> ObtenerPorRangoPrecioAsync(decimal minimo, decimal maximo)
        {
            return await _coleccion.Find(x => x.Precio >= minimo && x.Precio <= maximo)
                .SortBy(x => x.Precio).ToListAsync();
        }

        public async Task<bool> ExistePorCodigoInternoAsync(string codigo)
        {
            var cantidad = await _coleccion.CountDocumentsAsync(x => x.CodigoInterno == codigo);
            return cantidad > 0;
        }

        public async Task<List<Propiedad>> BuscarPorNombreAsync(string nombre)
        {
            return await _coleccion.Find(x => x.Nombre.Contains(nombre)).ToListAsync();
        }

        public async Task<List<Propiedad>> ObtenerPorAnioAsync(int anio)
        {
            return await _coleccion.Find(x => x.Anio == anio).ToListAsync();
        }

        public async Task<List<Propiedad>> ObtenerDisponiblesAsync()
        {
            var todasPropiedades = await _coleccion.Find(_ => true).ToListAsync();
            return todasPropiedades.Where(p => p.EstaDisponible()).ToList();
        }

        public async Task<long> ContarTotalAsync()
        {
            return await _coleccion.CountDocumentsAsync(_ => true);
        }

        public async Task<decimal> ObtenerPrecioPromedioAsync()
        {
            var propiedades = await _coleccion.Find(_ => true).ToListAsync();
            return propiedades.Count > 0 ? propiedades.Average(p => p.Precio) : 0;
        }

        public async Task<List<Propiedad>> ObtenerMasCarasAsync(int cantidad)
        {
            return await _coleccion.Find(_ => true)
                .SortByDescending(x => x.Precio)
                .Limit(cantidad)
                .ToListAsync();
        }

        public async Task<List<Propiedad>> ObtenerMasBaratasAsync(int cantidad)
        {
            return await _coleccion.Find(_ => true)
                .SortBy(x => x.Precio)
                .Limit(cantidad)
                .ToListAsync();
        }
    }
}