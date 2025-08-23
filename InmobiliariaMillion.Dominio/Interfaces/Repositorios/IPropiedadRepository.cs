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
    }
}