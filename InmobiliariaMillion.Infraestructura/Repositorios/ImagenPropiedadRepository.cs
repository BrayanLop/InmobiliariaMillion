using MongoDB.Driver;
using MongoDB.Bson;
using InmobiliariaMillion.Dominio.Interfaces.Repositorios;
using InmobiliariaMillion.Dominio.Entidades;

namespace InmobiliariaMillion.Infrastructura.Repositorio
{
    public class ImagenPropiedadRepository : IImagenPropiedadRepository
    {
        private readonly IMongoCollection<ImagenPropiedad> _coleccion;

        public ImagenPropiedadRepository(IMongoDatabase baseDatos)
        {
            _coleccion = baseDatos.GetCollection<ImagenPropiedad>("PropiedadImagen");
        }

        public async Task<ImagenPropiedad> CrearAsync(ImagenPropiedad imagen)
        {
            imagen.IdImagenPropiedad = ObjectId.GenerateNewId().ToString();
            await _coleccion.InsertOneAsync(imagen);
            return imagen;
        }

        public async Task<ImagenPropiedad> ObtenerPorIdAsync(string id)
        {
            return await _coleccion.Find(x => x.IdImagenPropiedad == id).FirstOrDefaultAsync();
        }

        public async Task<List<ImagenPropiedad>> ObtenerPorPropiedadAsync(string idPropiedad)
        {
            return await _coleccion.Find(x => x.IdPropiedad == idPropiedad).ToListAsync();
        }

        public async Task<ImagenPropiedad> ActualizarAsync(ImagenPropiedad imagen)
        {
            var resultado = await _coleccion.ReplaceOneAsync(x => x.IdImagenPropiedad == imagen.IdImagenPropiedad, imagen);
            return resultado.IsAcknowledged && resultado.ModifiedCount > 0 ? imagen : null;
        }

        public async Task<bool> EliminarAsync(string id)
        {
            var resultado = await _coleccion.DeleteOneAsync(x => x.IdImagenPropiedad == id);
            return resultado.IsAcknowledged && resultado.DeletedCount > 0;
        }

        public async Task<bool> ExisteAsync(string id)
        {
            var cantidad = await _coleccion.CountDocumentsAsync(x => x.IdImagenPropiedad == id);
            return cantidad > 0;
        }
    }
}