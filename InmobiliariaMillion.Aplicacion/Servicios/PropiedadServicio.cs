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
        private readonly IImagenPropiedadRepository _imagenRepository;
        private readonly IPropietarioRepository _propietarioRepository;

        public PropiedadServicio(
            IPropiedadRepository propiedadRepository,
            IImagenPropiedadRepository imagenRepository,
            IPropietarioRepository propietarioRepository)
        {
            _propiedadRepository = propiedadRepository;
            _imagenRepository = imagenRepository;
            _propietarioRepository = propietarioRepository;
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
                return null;

            return PropiedadMapeo.ADto(propiedad);            
        }

        public async Task<PropiedadOutputDto> CrearPropiedadAsync(PropiedadInputDto propiedadDto)
        {
            if (propiedadDto == null)
                throw new ArgumentNullException(nameof(propiedadDto));

            var propiedadCreada = await _propiedadRepository.CrearAsync(PropiedadMapeo.ADominio(propiedadDto));

            return PropiedadMapeo.ADto(propiedadCreada);
        }

        public async Task<PropiedadOutputDto> ActualizarPropiedadAsync(PropiedadInputDto propietarioDto)
        {
            if (propietarioDto == null)
                throw new ArgumentNullException(nameof(propietarioDto));

            var propiedad = await _propiedadRepository.ObtenerPorIdAsync(propietarioDto.IdPropiedad);
            if (propiedad == null) return null;

            propietarioDto._id = propiedad._id;

            var propiedadActualizada = await _propiedadRepository.ActualizarAsync(PropiedadMapeo.ADominio(propietarioDto));

            return PropiedadMapeo.ADto(propiedadActualizada);           
        }

        public async Task<bool> EliminarPropiedadAsync(string id)
        {
            if (string.IsNullOrWhiteSpace(id))
                throw new ArgumentNullException(nameof(id));

            var resultado = await _propietarioRepository.EliminarAsync(id);
            return resultado;
        }
    }
}