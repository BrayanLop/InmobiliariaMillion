using InmobiliariaMillion.Aplicacion.DTOs.Modelos.Propietario;
using InmobiliariaMillion.Aplicacion.DTOs.Utilidades;

namespace InmobiliariaMillion.Aplicacion.Servicios.Interfaces
{
    public interface IPropietarioServicio
    {
        Task<List<PropietarioOutputDto>> ObtenerPropietariosAsync(FiltrosPropietarioDto filtros);
        Task<PropietarioOutputDto> ObtenerPropietarioPorIdAsync(string id);
        Task<PropietarioOutputDto> CrearPropietarioAsync(PropietarioInputDto propietarioDto);
        Task<PropietarioOutputDto> ActualizarPropietarioAsync(PropietarioInputDto propietarioDto);
        Task<bool> EliminarPropietarioAsync(string id);
    }
}