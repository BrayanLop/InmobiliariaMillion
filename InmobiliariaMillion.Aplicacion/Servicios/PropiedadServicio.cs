using InmobiliariaMillion.Aplicacion.DTOs.Modelos;
using InmobiliariaMillion.Aplicacion.DTOs.Modelos.Propiedad;
using InmobiliariaMillion.Aplicacion.DTOs.Utilidades;
using InmobiliariaMillion.Aplicacion.Mapeo.Modelos;
using InmobiliariaMillion.Aplicacion.Servicios.Interfaces;
using InmobiliariaMillion.Dominio.Interfaces.Repositorios;

namespace InmobiliariaMillion.Application.Servicios
{
    public class PropiedadServicio : IPropiedadServicio
    {
        private readonly IPropiedadRepository _propiedadRepository;
        private readonly IPropietarioServicio _propietarioServicio;

        public PropiedadServicio(IPropiedadRepository propiedadRepository, IPropietarioServicio propietarioServicio)
        {
            _propiedadRepository = propiedadRepository;
            _propietarioServicio = propietarioServicio;
        }

        public async Task<List<PropiedadOutputDto>> ObtenerPropiedadesAsync(FiltrosPropiedadDto filtros)
        {
            var propiedades = await _propiedadRepository.ObtenerAsync(
                filtros.Nombre, filtros.Direccion, filtros.PrecioMinimo, filtros.PrecioMaximo);

            return PropiedadMapeo.ADtoLista(propiedades);
        }

        public async Task<PropiedadOutputDto> ObtenerPropiedadPorIdAsync(string id)
        {
            var propiedad = await _propiedadRepository.ObtenerPorIdAsync(id);
            if (propiedad == null)
                throw new ArgumentException("No se encontro la propiedad");

            return PropiedadMapeo.ADto(propiedad);            
        }

        public async Task<PropiedadOutputDto> CrearPropiedadAsync(PropiedadInputDto propiedadDto)
        {
            if (propiedadDto == null)
                throw new ArgumentException(nameof(propiedadDto));
            
            var propietario = await _propietarioServicio.ObtenerPropietarioPorIdAsync(propiedadDto.IdPropietario);
            if (propietario == null)
                throw new ArgumentException("No existe el propietario asociado a la propiedad");

            var propiedadCreada = await _propiedadRepository.CrearAsync(PropiedadMapeo.ADominio(propiedadDto));

            return PropiedadMapeo.ADto(propiedadCreada);
        }

        public async Task<PropiedadOutputDto> ActualizarPropiedadAsync(PropiedadInputDto propietarioDto)
        {
            if (propietarioDto == null)
                throw new ArgumentNullException(nameof(propietarioDto));

            var propiedad = await _propiedadRepository.ObtenerPorIdAsync(propietarioDto.IdPropiedad);
            if (propiedad == null) throw new ArgumentException("No existe la propiedad");

            propietarioDto._id = propiedad._id;

            var propiedadActualizada = await _propiedadRepository.ActualizarAsync(PropiedadMapeo.ADominio(propietarioDto));

            return PropiedadMapeo.ADto(propiedadActualizada);           
        }

        public async Task<bool> EliminarPropiedadAsync(string id)
        {
            if (string.IsNullOrWhiteSpace(id))
                throw new ArgumentNullException(nameof(id));

            var resultado = await _propiedadRepository.EliminarAsync(id);
            return resultado;
        }
    }
}