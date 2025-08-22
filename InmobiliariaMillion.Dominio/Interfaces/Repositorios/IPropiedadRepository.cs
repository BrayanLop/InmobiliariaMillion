using InmobiliariaMillion.Dominio.Entidades;

namespace InmobiliariaMillion.Dominio.Interfaces.Repositorios
{
    public interface IPropiedadRepository
    {
        Task<Propiedad> CrearAsync(Propiedad propiedad);
        Task<Propiedad> ObtenerPorIdAsync(string id);
        Task<List<Propiedad>> ObtenerAsync(string? nombre, string? direccion, decimal? precioMinimo, decimal? precioMaximo);
        Task<Propiedad> ActualizarAsync(Propiedad propiedad);
        Task<bool> EliminarAsync(string id);
        Task<bool> ExisteAsync(string id);
        Task<List<Propiedad>> ObtenerPorPropietarioAsync(string idPropietario);
        Task<List<Propiedad>> ObtenerPorRangoPrecioAsync(decimal minimo, decimal maximo);
        Task<bool> ExistePorCodigoInternoAsync(string codigo);
        Task<List<Propiedad>> BuscarPorNombreAsync(string nombre);
        Task<List<Propiedad>> ObtenerPorAnioAsync(int anio);
        Task<List<Propiedad>> ObtenerDisponiblesAsync();
        Task<long> ContarTotalAsync();
        Task<decimal> ObtenerPrecioPromedioAsync();
        Task<List<Propiedad>> ObtenerMasCarasAsync(int cantidad);
        Task<List<Propiedad>> ObtenerMasBaratasAsync(int cantidad);
    }
}