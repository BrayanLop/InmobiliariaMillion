using InmobiliariaMillion.Aplicacion.DTOs.Modelos.ImagenPropiedad;

namespace InmobiliariaMillion.Aplicacion.Servicios.Interfaces
{
    public interface IPropiedadImagenServicio
    {
        Task<List<ImagenPropiedadOutputDto>> ObtenerPropiedadImagenAsync();
        Task<List<ImagenPropiedadOutputDto>> ObtenerImagenesPorPropiedadAsync(string idPropiedad);
        Task<ImagenPropiedadOutputDto> AgregarImagenAPropiedadAsync(ImagenPropiedadInputDto imagenPropiedadDto);
        Task<bool> EliminarImagenAsync(string idImagenPropiedad);
        Task<bool> EliminarPropiedadAsync(string idPropiedad); // Elimina todas las imágenes de una propiedad
        Task<bool> HabilitarImagenAsync(string idImagenPropiedad, bool habilitar);
    }
}