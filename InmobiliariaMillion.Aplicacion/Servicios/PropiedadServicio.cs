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

            var resultado = new List<PropiedadOutputDto>();

            foreach (var propiedad in propiedades)
            {
                var propietario = await _propietarioRepository.ObtenerPorIdAsync(propiedad.IdPropietario);
                var imagenes = await _imagenRepository.ObtenerPorPropiedadAsync(propiedad.IdPropiedad);

                var propiedadDto = PropiedadMapeo.ADto(propiedad);
                propiedadDto.Propietario = PropietarioMapeo.ADto(propietario);

                resultado.Add(propiedadDto);
            }

            return resultado;
        }

        public async Task<PropiedadOutputDto> ObtenerPropiedadPorIdAsync(string id)
        {
            var propiedad = await _propiedadRepository.ObtenerPorIdAsync(id);
            if (propiedad == null)
                return null;

            var propietario = await _propietarioRepository.ObtenerPorIdAsync(propiedad.IdPropietario);

            var propiedadDto = PropiedadMapeo.ADto(propiedad);
            propiedadDto.Propietario = PropietarioMapeo.ADto(propietario);

            return propiedadDto;
        }

        public async Task<PropiedadOutputDto> CrearPropiedadAsync(PropiedadInputDto propiedadDto)
        {
            if (propiedadDto == null)
                throw new ArgumentNullException(nameof(propiedadDto));

            var propiedad = PropiedadMapeo.ADominio(propiedadDto);

            var propiedadCreada = await _propiedadRepository.CrearAsync(propiedad);

            return PropiedadMapeo.ADto(propiedadCreada);
        }
    }
}