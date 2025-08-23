using InmobiliariaMillion.Aplicacion.DTOs.Modelos.ImagenPropiedad;
using InmobiliariaMillion.Dominio.Entidades;

namespace InmobiliariaMillion.Aplicacion.Mapeo.Modelos
{
    public static class ImagenPropiedadMapeo
    {
        public static ImagenPropiedadInputDto ADto(ImagenPropiedad entidad)
        {
            if (entidad == null) return null;
            return new ImagenPropiedadInputDto
            {
                IdImagenPropiedad = entidad.IdImagenPropiedad,
                IdPropiedad = entidad.IdPropiedad,
                Archivo = entidad.Archivo,
                Habilitada = entidad.Habilitada
            };
        }

        public static ImagenPropiedad ADominio(ImagenPropiedadInputDto dto)
        {
            if (dto == null) return null;
            return new ImagenPropiedad
            {
                IdImagenPropiedad = dto.IdImagenPropiedad,
                IdPropiedad = dto.IdPropiedad,
                Archivo = dto.Archivo,
                Habilitada = dto.Habilitada
            };
        }
    }
}