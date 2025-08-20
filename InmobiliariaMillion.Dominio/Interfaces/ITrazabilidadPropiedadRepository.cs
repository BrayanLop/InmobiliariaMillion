namespace InmobiliariaMillion.Dominio.Interfaces
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