using InmobiliariaMillion.Dominio.Entidades;

namespace InmobiliariaMillion.Dominio.Interfaces.Repositorios
{
    public interface IPropietarioRepository
    {
        Task<Propietario> CrearAsync(Propietario propietario);
        Task<Propietario> ObtenerPorIdAsync(string id);
        Task<List<Propietario>> ObtenerTodosAsync();
        Task<Propietario> ActualizarAsync(Propietario propietario);
        Task<bool> EliminarAsync(string id);
        Task<bool> ExisteAsync(string id);
        Task<List<Propietario>> BuscarPorNombreAsync(string nombre);
        Task<List<Propietario>> ObtenerPropietariosConPropiedadesAsync();
        Task<int> ObtenerCantidadPropiedadesPorPropietarioAsync(string idPropietario);
    }
}