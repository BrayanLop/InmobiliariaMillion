using InmobiliariaMillion.Aplicacion.DTOs.Modelos.Propietario;
using InmobiliariaMillion.Aplicacion.DTOs.Utilidades;
using InmobiliariaMillion.Aplicacion.Mapeo.Modelos;
using InmobiliariaMillion.Aplicacion.Servicios.Interfaces;
using InmobiliariaMillion.Dominio.Interfaces.Repositorios;

namespace InmobiliariaMillion.Application.Servicios
{
    public class PropietarioServicio : IPropietarioServicio
    {
        private readonly IPropietarioRepository _propietarioRepository;

        public PropietarioServicio(IPropietarioRepository propietarioRepository)
        {
            _propietarioRepository = propietarioRepository;
        }

        public async Task<List<PropietarioOutputDto>> ObtenerPropietariosAsync(FiltrosPropietarioDto filtros)
        {
            var propietarios = await _propietarioRepository.ObtenerAsync(filtros.Nombre);

            return PropietarioMapeo.ADtoLista(propietarios);
        }

        public async Task<PropietarioOutputDto> ObtenerPropietarioPorIdAsync(string id)
        {
            var propietario = await _propietarioRepository.ObtenerPorIdAsync(id);
            if (propietario == null)
                throw new ArgumentException("No se encontro el propietario");

            return PropietarioMapeo.ADto(propietario);
        }

        public async Task<PropietarioOutputDto> CrearPropietarioAsync(PropietarioInputDto propietarioDto)
        {
            if (propietarioDto == null)
                throw new ArgumentException(nameof(propietarioDto));

            var propietarioCreado = await _propietarioRepository.CrearAsync(PropietarioMapeo.ADominio(propietarioDto));

            return PropietarioMapeo.ADto(propietarioCreado);
        }

        public async Task<PropietarioOutputDto> ActualizarPropietarioAsync(PropietarioInputDto propietarioDto)
        {
            var propietario = await _propietarioRepository.ObtenerPorIdAsync(propietarioDto.IdPropietario);
            if (propietario == null) throw new ArgumentException("No se encontro el propietario");

            propietarioDto._id = propietario._id;

            var propietarioActualizado = await _propietarioRepository.ActualizarAsync(PropietarioMapeo.ADominio(propietarioDto));

            return PropietarioMapeo.ADto(propietarioActualizado);
        }

        public async Task<bool> EliminarPropietarioAsync(string id)
        {
            return await _propietarioRepository.EliminarAsync(id);
        }
    }
}