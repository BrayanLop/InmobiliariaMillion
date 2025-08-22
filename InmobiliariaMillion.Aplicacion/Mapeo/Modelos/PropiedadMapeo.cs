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
            return new Propiedad
            {
                IdPropiedad = dto.IdPropiedad,
                Nombre = dto.Nombre,
                Direccion = dto.Direccion,
                Precio = dto.Precio,
                CodigoInterno = dto.CodigoInterno,
                Anio = dto.Anio,
                IdPropietario = dto.IdPropietario
            };
        }
    }
}