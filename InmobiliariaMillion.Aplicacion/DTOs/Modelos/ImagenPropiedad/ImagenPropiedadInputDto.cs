namespace InmobiliariaMillion.Aplicacion.DTOs.Modelos.ImagenPropiedad
{
    public class ImagenPropiedadInputDto
    {
        public string IdImagenPropiedad { get; set; }
        public string IdPropiedad { get; set; }
        public string Archivo { get; set; }
        public bool Habilitada { get; set; }
    }
}