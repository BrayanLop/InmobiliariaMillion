using InmobiliariaMillion.Dominio.Entidades;
using InmobiliariaMillion.Dominio.Interfaces.Repositorios;
using MongoDB.Bson;
using MongoDB.Driver;

namespace InmobiliariaMillion.Infrastructura.Repositorio
{
    public class PropietarioRepository : IPropietarioRepository
    {
        private readonly IMongoCollection<Propietario> _coleccion;

        public PropietarioRepository(IMongoDatabase baseDatos)
        {
            _coleccion = baseDatos.GetCollection<Propietario>("Propietario");
        }

        public async Task<List<Propietario>> ObtenerAsync(string nombre)
        {
            var builder = Builders<Propietario>.Filter;
            var filtro = builder.Empty;

            if (!string.IsNullOrEmpty(nombre))
                filtro &= builder.Regex(x => x.Nombre, new BsonRegularExpression(nombre, "i"));

            return await _coleccion.Find(filtro).ToListAsync();
        }

        public async Task<Propietario> ObtenerPorIdAsync(string id)
        {
            return await _coleccion.Find(x => x.IdPropietario == id).FirstOrDefaultAsync();
        }

        public async Task<Propietario> CrearAsync(Propietario propietario)
        {
            propietario.IdPropietario = ObjectId.GenerateNewId().ToString();
            await _coleccion.InsertOneAsync(propietario);
            return propietario;
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
    }
}