using InmobiliariaMillion.Aplicacion.DTOs.Modelos.TrazabilidadPropiedad;
using InmobiliariaMillion.Dominio.Entidades;

namespace InmobiliariaMillion.Aplicacion.Mapeo.Modelos
{
    public static class TrazabilidadPropiedadMapeo
    {
        public static TrazabilidadPropiedadOutputDto ADto(TrazabilidadPropiedad entidad)
        {
            if (entidad == null) return null;
            return new TrazabilidadPropiedadOutputDto
            {
                IdTrazabilidadPropiedad = entidad.IdTrazabilidadPropiedad,
                FechaVenta = entidad.FechaVenta,
                Nombre = entidad.Nombre,
                Valor = entidad.Valor,
                Impuesto = entidad.Impuesto,
                IdPropiedad = entidad.IdPropiedad
            };
        }

        public static TrazabilidadPropiedad ADominio(TrazabilidadPropiedadInputDto dto)
        {
            if (dto == null) return null;
            return new TrazabilidadPropiedad(
                dto._id,
                dto.IdTrazabilidadPropiedad,
                dto.FechaVenta,
                dto.Nombre,
                dto.Valor,
                dto.Impuesto,
                dto.IdPropiedad
            );
        }

        public static List<TrazabilidadPropiedadOutputDto> ADtoLista(List<TrazabilidadPropiedad> entidades)
        {
            if (entidades == null) return null;
            return entidades.Select(ADto).ToList();
        }

        public static List<TrazabilidadPropiedad> ADominioLista(List<TrazabilidadPropiedadInputDto> dtos)
        {
            if (dtos == null) return null;
            return dtos.Select(ADominio).ToList();
        }
    }
}