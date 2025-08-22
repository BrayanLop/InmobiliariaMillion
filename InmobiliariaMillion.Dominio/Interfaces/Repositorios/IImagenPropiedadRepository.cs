using InmobiliariaMillion.Dominio.Entidades;

namespace InmobiliariaMillion.Dominio.Interfaces.Repositorios
{
    public interface IImagenPropiedadRepository
    {
        Task<ImagenPropiedad> CrearAsync(ImagenPropiedad imagen);
        Task<ImagenPropiedad> ObtenerPorIdAsync(string id);
        Task<List<ImagenPropiedad>> ObtenerPorPropiedadAsync(string idPropiedad);
        Task<ImagenPropiedad> ActualizarAsync(ImagenPropiedad imagen);
        Task<bool> EliminarAsync(string id);
        Task<bool> ExisteAsync(string id);
    }
}