using InmobiliariaMillion.Aplicacion.DTOs.Modelos.TrazabilidadPropiedad;
using InmobiliariaMillion.Aplicacion.Mapeo.Modelos;
using InmobiliariaMillion.Aplicacion.Servicios.Interfaces;
using InmobiliariaMillion.Dominio.Interfaces.Repositorios;

namespace InmobiliariaMillion.Application.Servicios
{
    public class TrazabilidadPropiedadServicio : ITrazabilidadPropiedadServicio
    {
        private readonly ITrazabilidadPropiedadRepository _trazabilidadRepository;

        public TrazabilidadPropiedadServicio(ITrazabilidadPropiedadRepository trazabilidadRepository)
        {
            _trazabilidadRepository = trazabilidadRepository;
        }

        public async Task<TrazabilidadPropiedadOutputDto> CrearTrazabilidadPropiedadAsync(TrazabilidadPropiedadInputDto dto)
        {
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
            return TrazabilidadPropiedadMapeo.ADto(trazabilidad);
        }

        public async Task<TrazabilidadPropiedadOutputDto> ActualizarTrazabilidadPropiedadAsync(TrazabilidadPropiedadInputDto trazabilidadDto)
        {
            var trazabilidad = await _trazabilidadRepository.ActualizarAsync(TrazabilidadPropiedadMapeo.ADominio(trazabilidadDto));
            return TrazabilidadPropiedadMapeo.ADto(trazabilidad);
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