namespace InmobiliariaMillion.Application.Commands.ImagenesPropiedades
{
    public class ActualizarImagenPropiedadCommand
    {
        public string IdImagenPropiedad { get; set; }
        public string Archivo { get; set; }
        public bool Habilitada { get; set; }
    }
}