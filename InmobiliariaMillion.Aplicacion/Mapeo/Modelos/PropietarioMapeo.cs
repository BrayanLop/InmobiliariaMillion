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
            return new Propietario(
                dto._id,
                dto.IdPropietario,
                dto.Nombre,
                dto.Direccion,
                foto: dto.Foto,
                dto.FechaNacimiento
             );

        }
        public static List<PropietarioOutputDto> ADtoLista(List<Propietario> entidades)
        {
            if (entidades == null) return null;
            return entidades.Select(ADto).ToList();
        }

        public static List<Propietario> ADominioLista(List<PropietarioInputDto> dtos)
        {
            if (dtos == null) return null;
            return dtos.Select(ADominio).ToList();
        }
    }
}