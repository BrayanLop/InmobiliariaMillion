using InmobiliariaMillion.Aplicacion.DTOs.Modelos.ImagenPropiedad;
using InmobiliariaMillion.Aplicacion.Mapeo.Modelos;
using InmobiliariaMillion.Aplicacion.Servicios.Interfaces;
using InmobiliariaMillion.Dominio.Interfaces.Repositorios;

namespace InmobiliariaMillion.Application.Servicios
{
    public class ImagenPropiedadServicio : IImagenPropiedadServicio
    {
        private readonly IPropiedadRepository _propiedadRepository;
        private readonly IImagenPropiedadRepository _imagenRepository;

        public ImagenPropiedadServicio(
            IPropiedadRepository propiedadRepository,
            IImagenPropiedadRepository imagenRepository)
        {
            _propiedadRepository = propiedadRepository;
            _imagenRepository = imagenRepository;
        }

        public async Task<List<ImagenPropiedadOutputDto>> ObtenerPropiedadImagenAsync()
        {
            var imagenes = await _imagenRepository.ObtenerPorPropiedadAsync(null);
            return ImagenPropiedadMapeo.ADtoLista(imagenes);
        }

        public async Task<List<ImagenPropiedadOutputDto>> ObtenerImagenesPorPropiedadAsync(string idPropiedad)
        {
            var imagenes = await _imagenRepository.ObtenerPorPropiedadAsync(idPropiedad);
            return ImagenPropiedadMapeo.ADtoLista(imagenes);
        }

        public async Task<ImagenPropiedadOutputDto> AgregarImagenAPropiedadAsync(ImagenPropiedadInputDto imagenPropiedadDto)
        {
            var existePropiedad = await _propiedadRepository.ExisteAsync(imagenPropiedadDto.IdPropiedad);
            if (!existePropiedad)
                throw new ArgumentException("La propiedad no existe.");

            var creada = await _imagenRepository.CrearAsync(ImagenPropiedadMapeo.ADominio(imagenPropiedadDto));
            return ImagenPropiedadMapeo.ADto(creada);
        }

        public async Task<bool> EliminarImagenAsync(string idImagenPropiedad)
        {
            return await _imagenRepository.EliminarAsync(idImagenPropiedad);
        }

        public async Task<bool> EliminarPropiedadAsync(string idPropiedad)
        {
            var imagenes = await _imagenRepository.ObtenerPorPropiedadAsync(idPropiedad);
            bool todasEliminadas = true;
            foreach (var img in imagenes)
            {
                var eliminado = await _imagenRepository.EliminarAsync(img.IdImagenPropiedad);
                if (!eliminado) todasEliminadas = false;
            }
            return todasEliminadas;
        }

        public async Task<bool> HabilitarImagenAsync(string idImagenPropiedad, bool habilitar)
        {
            var imagen = await _imagenRepository.ObtenerPorIdAsync(idImagenPropiedad);

            if (imagen == null) return false;

            if (habilitar) imagen.Habilitar();
            else imagen.Deshabilitar();

            var actualizada = await _imagenRepository.ActualizarAsync(imagen);
            return actualizada != null;
        }
    }
}