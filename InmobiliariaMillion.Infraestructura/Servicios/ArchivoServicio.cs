using InmobiliariaMillion.Infrastructura.Interfaces;
using System.Drawing;
using System.Drawing.Imaging;

namespace InmobiliariaMillion.Infrastructura.Servicios
{
    public class ArchivoServicio : IArchivoServicio
    {
        public async Task<string> GuardarImagenBase64Async(string base64Image, string nombreArchivo)
        {
            if (string.IsNullOrEmpty(base64Image) || string.IsNullOrEmpty(nombreArchivo))
                throw new ArgumentException("La imagen en base64 y el nombre del archivo son obligatorios");

            try
            {
                // Ejecutar la operación en un subproceso en segundo plano
                return await Task.Run(() =>
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
                });
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
    }
}