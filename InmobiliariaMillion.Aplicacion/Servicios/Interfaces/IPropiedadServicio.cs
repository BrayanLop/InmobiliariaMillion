using InmobiliariaMillion.Aplicacion.DTOs.Modelos;
using InmobiliariaMillion.Aplicacion.DTOs.Modelos.Propiedad;
using InmobiliariaMillion.Aplicacion.DTOs.Utilidades;

namespace InmobiliariaMillion.Aplicacion.Servicios.Interfaces
{
    public interface IPropiedadServicio
    {
        Task<List<PropiedadOutputDto>> ObtenerPropiedadesAsync(FiltrosPropiedadDto filtros);
        Task<PropiedadOutputDto> ObtenerPropiedadPorIdAsync(string id);
        Task<PropiedadOutputDto> CrearPropiedadAsync(PropiedadInputDto propiedadDto);
        Task<PropiedadOutputDto> ActualizarPropiedadAsync(PropiedadInputDto propietarioDto);
        Task<bool> EliminarPropiedadAsync(string id);
    }
}