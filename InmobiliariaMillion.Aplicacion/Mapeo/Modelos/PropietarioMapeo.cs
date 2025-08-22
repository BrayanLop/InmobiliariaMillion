using InmobiliariaMillion.Aplicacion.DTOs.Modelos.Propietario;
using InmobiliariaMillion.Dominio.Entidades;

namespace InmobiliariaMillion.Aplicacion.Mapeo.Modelos
{
    public static class PropietarioMapeo
    {
        public static PropietarioOutputDto ADto(Propietario entidad)
        {
            if (entidad == null) return null;
            return new PropietarioOutputDto
            {
                IdPropietario = entidad.IdPropietario,
                Nombre = entidad.Nombre,
                Direccion = entidad.Direccion,
                Foto = entidad.Foto,
                FechaNacimiento = entidad.FechaNacimiento
            };
        }

        public static Propietario ADominio(PropietarioInputDto dto)
        {
            if (dto == null) return null;
            return new Propietario
            {
                IdPropietario = dto.IdPropietario,
                Nombre = dto.Nombre,
                Direccion = dto.Direccion,
                Foto = dto.Foto,
                FechaNacimiento = dto.FechaNacimiento
            };
        }
    }
}