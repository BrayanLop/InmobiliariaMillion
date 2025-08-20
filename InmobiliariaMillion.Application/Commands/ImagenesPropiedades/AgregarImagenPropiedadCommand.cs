namespace InmobiliariaMillion.Application.Commands.ImagenesPropiedades
{
    public class AgregarImagenPropiedadCommand
    {
        public string IdPropiedad { get; set; }
        public string Archivo { get; set; }
        public bool Habilitada { get; set; } = true;
    }
}