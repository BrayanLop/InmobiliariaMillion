using InmobiliariaMillion.Dominio.Entidades;

namespace InmobiliariaMillion.Dominio.Interfaces.Repositorios
{
    public interface IPropietarioRepository
    {
        Task<Propietario> CrearAsync(Propietario propietario);
        Task<Propietario> ObtenerPorIdAsync(string id);
        Task<List<Propietario>> ObtenerAsync(string nombre);
        Task<Propietario> ActualizarAsync(Propietario propietario);
        Task<bool> EliminarAsync(string id);
        Task<bool> ExisteAsync(string id);
    }
}