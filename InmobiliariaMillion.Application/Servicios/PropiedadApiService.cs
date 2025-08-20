using InmobiliariaMillion.Application.DTOs;
using InmobiliariaMillion.Dominio.Interfaces;

namespace InmobiliariaMillion.Application.Servicios
{
    public class PropiedadApiService : IPropiedadApiService
    {
        private readonly IPropiedadRepository _propiedadRepository;
        private readonly IImagenPropiedadRepository _imagenRepository;
        private readonly IPropietarioRepository _propietarioRepository;

        public PropiedadApiService(
            IPropiedadRepository propiedadRepository,
            IImagenPropiedadRepository imagenRepository,
            IPropietarioRepository propietarioRepository)
        {
            _propiedadRepository = propiedadRepository;
            _imagenRepository = imagenRepository;
            _propietarioRepository = propietarioRepository;
        }

        public async Task<List<PropiedadDto>> ObtenerPropiedadesFiltradosAsync(FiltrosPropiedadDto filtros)
        {
            var propiedades = await _propiedadRepository.ObtenerTodosAsync();

            // Aplicar filtros
            if (!string.IsNullOrWhiteSpace(filtros.Name))
            {
                propiedades = propiedades.Where(p =>
                    p.Nombre.Contains(filtros.Name, StringComparison.OrdinalIgnoreCase)).ToList();
            }

            if (!string.IsNullOrWhiteSpace(filtros.Address))
            {
                propiedades = propiedades.Where(p =>
                    p.Direccion.Contains(filtros.Address, StringComparison.OrdinalIgnoreCase)).ToList();
            }

            if (filtros.MinPrice.HasValue)
            {
                propiedades = propiedades.Where(p => p.Precio >= filtros.MinPrice.Value).ToList();
            }

            if (filtros.MaxPrice.HasValue)
            {
                propiedades = propiedades.Where(p => p.Precio <= filtros.MaxPrice.Value).ToList();
            }

            // Convertir a DTO y obtener imágenes
            var resultado = new List<PropiedadDto>();

            foreach (var propiedad in propiedades)
            {
                var propietario = await _propietarioRepository.ObtenerPorIdAsync(propiedad.IdPropietario);
                var imagenes = await _imagenRepository.ObtenerPorPropiedadAsync(propiedad.IdPropiedad);
                var imagenesDto = imagenes?
                    .Where(i => i.Habilitada)
                    .Select(i => new ImagenPropiedadDto
                    {
                        IdImagenPropiedad = i.IdImagenPropiedad,
                        IdPropiedad = i.IdPropiedad,
                        Archivo = i.Archivo,
                        Habilitada = i.Habilitada
                    })
                    .ToList();

                resultado.Add(new PropiedadDto
                {
                    IdPropiedad = propiedad.IdPropiedad,
                    Nombre = propiedad.Nombre,
                    Direccion = propiedad.Direccion,
                    Precio = propiedad.Precio,
                    ImagenesPropiedad = imagenesDto
                });
            }

            return resultado;
        }

        public async Task<PropiedadDto> ObtenerPropiedadPorIdAsync(string id)
        {
            var propiedad = await _propiedadRepository.ObtenerPorIdAsync(id);
            if (propiedad == null)
                return null;

            var propietario = await _propietarioRepository.ObtenerPorIdAsync(propiedad.IdPropietario);
            var imagenes = await _imagenRepository.ObtenerPorPropiedadAsync(propiedad.IdPropiedad);

            // Mapear las imágenes de dominio a DTO
            var imagenesDto = imagenes?
                .Where(i => i.Habilitada)
                .Select(i => new ImagenPropiedadDto
                {
                    IdImagenPropiedad = i.IdImagenPropiedad,
                    IdPropiedad = i.IdPropiedad,
                    Archivo = i.Archivo,
                    Habilitada = i.Habilitada
                })
                .ToList();

            return new PropiedadDto
            {
                IdPropiedad = propiedad.IdPropiedad,
                Nombre = propiedad.Nombre,
                Direccion = propiedad.Direccion,
                Precio = propiedad.Precio,
                ImagenesPropiedad = imagenesDto
            };
        }

        public async Task<List<PropiedadDto>> ObtenerTodasLasPropiedadesAsync()
        {
            return await ObtenerPropiedadesFiltradosAsync(new FiltrosPropiedadDto());
        }
    }
}