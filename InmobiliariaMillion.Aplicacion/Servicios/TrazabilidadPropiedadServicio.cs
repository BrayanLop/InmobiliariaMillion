using InmobiliariaMillion.Aplicacion.DTOs.Modelos.TrazabilidadPropiedad;
using InmobiliariaMillion.Aplicacion.Mapeo.Modelos;
using InmobiliariaMillion.Aplicacion.Servicios.Interfaces;
using InmobiliariaMillion.Dominio.Interfaces.Repositorios;

namespace InmobiliariaMillion.Application.Servicios
{
    public class TrazabilidadPropiedadServicio : ITrazabilidadPropiedadServicio
    {
        private readonly ITrazabilidadPropiedadRepository _trazabilidadRepository;
        private readonly IPropiedadServicio _propiedadServicio;

        public TrazabilidadPropiedadServicio(ITrazabilidadPropiedadRepository trazabilidadRepository, IPropiedadServicio propiedadServicio)
        {
            _trazabilidadRepository = trazabilidadRepository;
            _propiedadServicio = propiedadServicio;
        }

        public async Task<TrazabilidadPropiedadOutputDto> CrearTrazabilidadPropiedadAsync(TrazabilidadPropiedadInputDto dto)
        {
            var propiedad = await _propiedadServicio.ObtenerPropiedadPorIdAsync(dto.IdPropiedad);
            if (propiedad == null)
                throw new ArgumentException("No se encontro la propiedad relacionada");

            dto.IdTrazabilidadPropiedad = Guid.NewGuid().ToString();

            var trazabilidad = await _trazabilidadRepository.CrearAsync(TrazabilidadPropiedadMapeo.ADominio(dto));
            return TrazabilidadPropiedadMapeo.ADto(trazabilidad);
        }

        public async Task<List<TrazabilidadPropiedadOutputDto>> ObtenerPorPropiedadAsync(string idPropiedad)
        {
            var trazabilidadPropiedades = await _trazabilidadRepository.ObtenerPorPropiedadAsync(idPropiedad);

            return TrazabilidadPropiedadMapeo.ADtoLista(trazabilidadPropiedades);
        }

        public async Task<TrazabilidadPropiedadOutputDto> ObtenerTrazabilidadPropiedadPorIdAsync(string idTrazabilidadPropiedad)
        {
            var trazabilidad = await _trazabilidadRepository.ObtenerPorIdAsync(idTrazabilidadPropiedad);

            if (trazabilidad == null)
                throw new ArgumentException("No se encontro el registro");

            return TrazabilidadPropiedadMapeo.ADto(trazabilidad);
        }

        public async Task<TrazabilidadPropiedadOutputDto> ActualizarTrazabilidadPropiedadAsync(TrazabilidadPropiedadInputDto trazabilidadDto)
        {
            if (trazabilidadDto == null)
                throw new ArgumentException(nameof(trazabilidadDto));

            var trazabilidad = await _trazabilidadRepository.ObtenerPorIdAsync(trazabilidadDto.IdTrazabilidadPropiedad);
            if (trazabilidad == null) throw new ArgumentException("No se ecnontro el registro");

            trazabilidadDto._id = trazabilidad._id;

            var trazabilidadActualizada = await _trazabilidadRepository.ActualizarAsync(TrazabilidadPropiedadMapeo.ADominio(trazabilidadDto));
            return TrazabilidadPropiedadMapeo.ADto(trazabilidadActualizada);
        }

        public async Task<bool> EliminarTrazabilidadPropiedadAsync(string idTrazabilidadPropiedad)
        {
            return await _trazabilidadRepository.EliminarAsync(idTrazabilidadPropiedad);
        }

        public async Task<List<TrazabilidadPropiedadOutputDto>> ObtenerVentasRecientesAsync(DateTime desde)
        {
            var trazabilidades = await _trazabilidadRepository.ObtenerVentasRecientesAsync(desde);
            return TrazabilidadPropiedadMapeo.ADtoLista(trazabilidades);
        }
    }
}