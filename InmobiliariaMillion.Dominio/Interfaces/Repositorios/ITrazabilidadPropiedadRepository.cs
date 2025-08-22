using InmobiliariaMillion.Dominio.Entidades;

namespace InmobiliariaMillion.Dominio.Interfaces.Repositorios
{
    public interface ITrazabilidadPropiedadRepository
    {
        Task<TrazabilidadPropiedad> CrearAsync(TrazabilidadPropiedad trazabilidad);
        Task<TrazabilidadPropiedad> ObtenerPorIdAsync(string id);
        Task<List<TrazabilidadPropiedad>> ObtenerPorPropiedadAsync(string idPropiedad);
        Task<TrazabilidadPropiedad> ActualizarAsync(TrazabilidadPropiedad trazabilidad);
        Task<bool> EliminarAsync(string id);
        Task<List<TrazabilidadPropiedad>> ObtenerVentasRecientesAsync(DateTime desde);
    }
}