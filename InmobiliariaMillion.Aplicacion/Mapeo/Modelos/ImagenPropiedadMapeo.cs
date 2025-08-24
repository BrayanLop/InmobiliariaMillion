using InmobiliariaMillion.Aplicacion.DTOs.Modelos.ImagenPropiedad;
using InmobiliariaMillion.Dominio.Entidades;

namespace InmobiliariaMillion.Aplicacion.Mapeo.Modelos
{
    public static class ImagenPropiedadMapeo
    {
        public static ImagenPropiedadOutputDto ADto(ImagenPropiedad entidad)
        {
            if (entidad == null) return null;
            return new ImagenPropiedadOutputDto
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
                _id = dto._id,
                IdImagenPropiedad = dto.IdImagenPropiedad,
                IdPropiedad = dto.IdPropiedad,
                Archivo = dto.Archivo,
                Habilitada = dto.Habilitada
            };
        }

        public static List<ImagenPropiedadOutputDto> ADtoLista(List<ImagenPropiedad> entidades)
        {
            if (entidades == null) return null;
            return entidades.Select(ADto).ToList();
        }

        public static List<ImagenPropiedad> ADominioLista(List<ImagenPropiedadInputDto> dtos)
        {
            if (dtos == null) return null;
            return dtos.Select(ADominio).ToList();
        }
    }
}