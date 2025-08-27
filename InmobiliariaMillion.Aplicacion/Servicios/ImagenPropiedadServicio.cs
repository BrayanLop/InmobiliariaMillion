using InmobiliariaMillion.Aplicacion.DTOs.Modelos.ImagenPropiedad;
using InmobiliariaMillion.Aplicacion.Mapeo.Modelos;
using InmobiliariaMillion.Aplicacion.Servicios.Interfaces;
using InmobiliariaMillion.Dominio.Interfaces.Repositorios;
using System.Drawing;
using System.Drawing.Imaging; 

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
            var existePropiedad = await _propiedadRepository.ObtenerPorIdAsync(imagenPropiedadDto.IdPropiedad);
            if (existePropiedad == null)
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

        public async Task<string> GuardarImagenBase64Async(string base64Image, string nombreArchivo)
        {
            if (string.IsNullOrEmpty(base64Image) || string.IsNullOrEmpty(nombreArchivo))
                throw new ArgumentException("La imagen en base64 y el nombre del archivo son obligatorios");

            try
            {
                // Convertir Base64 a bytes
                byte[] imageBytes = Convert.FromBase64String(base64Image);

                // Verificar que es una imagen válida
                using (var ms = new MemoryStream(imageBytes))
                {
                    using var image = Image.FromStream(ms);

                    // Verificar dimensiones mínimas
                    if (image.Width < 10 || image.Height < 10)
                        throw new ArgumentException("La imagen es demasiado pequeña");

                    // Opcional: Optimizar imagen para almacenamiento
                    string extension = Path.GetExtension(nombreArchivo).ToLower();
                    ImageFormat formato = ImageFormat.Jpeg; // Formato por defecto

                    if (extension == ".png") formato = ImageFormat.Png;
                    else if (extension == ".gif") formato = ImageFormat.Gif;

                    // Crear directorio si no existe
                    string uploadFolder = Path.Combine(Directory.GetCurrentDirectory(), "uploads");
                    Directory.CreateDirectory(uploadFolder);

                    // Generar nombre único para evitar sobreescrituras
                    string nombreUnico = $"{Guid.NewGuid()}_{nombreArchivo}";
                    string filePath = Path.Combine(uploadFolder, nombreUnico);

                    // Guardar imagen con formato específico
                    using (var fs = new FileStream(filePath, FileMode.Create))
                    {
                        image.Save(fs, formato);
                    }

                    return $"/uploads/{nombreUnico}";
                }
            }
            catch (FormatException)
            {
                throw new ArgumentException("El formato Base64 no es válido");
            }
            catch (Exception ex) when (!(ex is ArgumentException))
            {
                throw new Exception($"Error al procesar la imagen: {ex.Message}", ex);
            }
        }

        public async Task<ImagenPropiedadOutputDto?> ObtenerImagenPorIdAsync(string idImagenPropiedad)
        {
            var imagen = await _imagenRepository.ObtenerPorIdAsync(idImagenPropiedad);
            return imagen != null ? ImagenPropiedadMapeo.ADto(imagen) : null;
        }
        }
}