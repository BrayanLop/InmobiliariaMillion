namespace InmobiliariaMillion.Infrastructura.Interfaces
{
    public interface IArchivoServicio
    {
        Task<string> GuardarImagenBase64Async(string base64Image, string nombreArchivo);
    }
}