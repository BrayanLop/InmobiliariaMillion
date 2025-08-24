using InmobiliariaMillion.Aplicacion.DTOs.Modelos.ImagenPropiedad;
using InmobiliariaMillion.Aplicacion.DTOs.Modelos.TrazabilidadPropiedad;

namespace InmobiliariaMillion.Aplicacion.Servicios.Interfaces
{
    public interface ITrazabilidadPropiedadServicio
    {
        Task<TrazabilidadPropiedadOutputDto> CrearTrazabilidadPropiedadAsync(TrazabilidadPropiedadInputDto dto);
        Task<List<TrazabilidadPropiedadOutputDto>> ObtenerPorPropiedadAsync(string idPropiedad);
        Task<TrazabilidadPropiedadOutputDto> ObtenerTrazabilidadPropiedadPorIdAsync(string idTrazabilidadPropiedad);
        Task<TrazabilidadPropiedadOutputDto> ActualizarTrazabilidadPropiedadAsync(TrazabilidadPropiedadInputDto dto);
        Task<bool> EliminarTrazabilidadPropiedadAsync(string idTrazabilidadPropiedad);
        Task<List<TrazabilidadPropiedadOutputDto>> ObtenerVentasRecientesAsync(DateTime desde);
    }
}