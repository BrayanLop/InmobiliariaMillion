using InmobiliariaMillion.Aplicacion.DTOs.Modelos.TrazabilidadPropiedad;
using InmobiliariaMillion.Dominio.Entidades;

namespace InmobiliariaMillion.Aplicacion.Mapeo.Modelos
{
    public static class TrazabilidadPropiedadMapeo
    {
        public static TrazabilidadPropiedadDto ADto(TrazabilidadPropiedad entidad)
        {
            if (entidad == null) return null;
            return new TrazabilidadPropiedadDto
            {
                IdTrazabilidadPropiedad = entidad.IdTrazabilidadPropiedad,
                FechaVenta = entidad.FechaVenta,
                Nombre = entidad.Nombre,
                Valor = entidad.Valor,
                Impuesto = entidad.Impuesto,
                IdPropiedad = entidad.IdPropiedad
            };
        }

        public static TrazabilidadPropiedad ADominio(TrazabilidadPropiedadDto dto)
        {
            if (dto == null) return null;
            return new TrazabilidadPropiedad
            {
                IdTrazabilidadPropiedad = dto.IdTrazabilidadPropiedad,
                FechaVenta = dto.FechaVenta,
                Nombre = dto.Nombre,
                Valor = dto.Valor,
                Impuesto = dto.Impuesto,
                IdPropiedad = dto.IdPropiedad
            };
        }
    }
}