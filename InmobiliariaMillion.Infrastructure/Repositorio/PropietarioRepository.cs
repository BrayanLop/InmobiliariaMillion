using MongoDB.Driver;
using MongoDB.Bson;
using InmobiliariaMillion.Dominio;
using InmobiliariaMillion.Dominio.Interfaces;

namespace InmobiliariaMillion.Infrastructura.Repositorio
{
    public class PropietarioRepository : IPropietarioRepository
    {
        private readonly IMongoCollection<Propietario> _coleccion;
        private readonly IMongoCollection<Propiedad> _coleccionPropiedades;

        public PropietarioRepository(IMongoDatabase baseDatos)
        {
            _coleccion = baseDatos.GetCollection<Propietario>("propietarios");
            _coleccionPropiedades = baseDatos.GetCollection<Propiedad>("propiedades");
        }

        public async Task<Propietario> CrearAsync(Propietario propietario)
        {
            propietario.IdPropietario = ObjectId.GenerateNewId().ToString();
            await _coleccion.InsertOneAsync(propietario);
            return propietario;
        }

        public async Task<Propietario> ObtenerPorIdAsync(string id)
        {
            return await _coleccion.Find(x => x.IdPropietario == id).FirstOrDefaultAsync();
        }

        public async Task<List<Propietario>> ObtenerTodosAsync()
        {
            return await _coleccion.Find(_ => true).ToListAsync();
        }

        public async Task<Propietario> ActualizarAsync(Propietario propietario)
        {
            var resultado = await _coleccion.ReplaceOneAsync(x => x.IdPropietario == propietario.IdPropietario, propietario);
            return resultado.IsAcknowledged && resultado.ModifiedCount > 0 ? propietario : null;
        }

        public async Task<bool> EliminarAsync(string id)
        {
            var resultado = await _coleccion.DeleteOneAsync(x => x.IdPropietario == id);
            return resultado.IsAcknowledged && resultado.DeletedCount > 0;
        }

        public async Task<bool> ExisteAsync(string id)
        {
            var cantidad = await _coleccion.CountDocumentsAsync(x => x.IdPropietario == id);
            return cantidad > 0;
        }

        public async Task<List<Propietario>> BuscarPorNombreAsync(string nombre)
        {
            return await _coleccion.Find(x => x.Nombre.Contains(nombre)).ToListAsync();
        }

        public async Task<List<Propietario>> ObtenerPropietariosConPropiedadesAsync()
        {
            var propietariosConPropiedades = await _coleccionPropiedades
                .Distinct<string>("idPropietario", Builders<Propiedad>.Filter.Empty)
                .ToListAsync();

            return await _coleccion
                .Find(x => propietariosConPropiedades.Contains(x.IdPropietario))
                .ToListAsync();
        }

        public async Task<int> ObtenerCantidadPropiedadesPorPropietarioAsync(string idPropietario)
        {
            var cantidad = await _coleccionPropiedades.CountDocumentsAsync(x => x.IdPropietario == idPropietario);
            return (int)cantidad;
        }
    }
}