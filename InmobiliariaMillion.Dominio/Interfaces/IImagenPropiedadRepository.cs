namespace InmobiliariaMillion.Dominio.Interfaces
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