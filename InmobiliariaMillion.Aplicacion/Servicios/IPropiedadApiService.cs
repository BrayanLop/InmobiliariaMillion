using InmobiliariaMillion.Application.DTOs;

namespace InmobiliariaMillion.Application.Servicios
{
    public interface IPropiedadApiService
    {
        Task<List<PropiedadDto>> ObtenerPropiedadesFiltradosAsync(FiltrosPropiedadDto filtros);
        Task<PropiedadDto> ObtenerPropiedadPorIdAsync(string id);
        Task<List<PropiedadDto>> ObtenerTodasLasPropiedadesAsync();
        Task<PropiedadDto> CrearPropiedadAsync(PropiedadDto propiedadDto);
    }
}