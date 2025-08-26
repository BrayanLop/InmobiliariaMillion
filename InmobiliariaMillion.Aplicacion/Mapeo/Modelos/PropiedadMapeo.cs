using InmobiliariaMillion.Aplicacion.DTOs.Modelos;
using InmobiliariaMillion.Aplicacion.DTOs.Modelos.Propiedad;
using InmobiliariaMillion.Dominio.Entidades;

namespace InmobiliariaMillion.Aplicacion.Mapeo.Modelos
{
    public static class PropiedadMapeo
    {
        public static PropiedadOutputDto ADto(Propiedad entidad)
        {
            if (entidad == null) return null;
            return new PropiedadOutputDto
            {
                IdPropiedad = entidad.IdPropiedad,
                Nombre = entidad.Nombre,
                Direccion = entidad.Direccion,
                Precio = entidad.Precio,
                CodigoInterno = entidad.CodigoInterno,
                Anio = entidad.Anio,
                IdPropietario = entidad.IdPropietario
            };
        }

        public static Propiedad ADominio(PropiedadInputDto dto)
        {
            if (dto == null) return null;
            return new Propiedad(
                dto._id,
                dto.IdPropiedad,
                dto.Nombre,
                dto.Precio,
                dto.Direccion,
                dto.CodigoInterno,
                dto.Anio,
                dto.IdPropietario
            );
        }

        public static List<PropiedadOutputDto> ADtoLista(List<Propiedad> entidades)
        {
            if (entidades == null) return null;
            return entidades.Select(ADto).ToList();
        }

        public static List<Propiedad> ADominioLista(List<PropiedadInputDto> dtos)
        {
            if (dtos == null) return null;
            return dtos.Select(ADominio).ToList();
        }
    }
}