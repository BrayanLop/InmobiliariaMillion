namespace InmobiliariaMillion.Application.Commands.Propiedades
{
    public class CrearPropiedadCommand
    {
        public string Nombre { get; set; }
        public string Direccion { get; set; }
        public decimal Precio { get; set; }
        public string CodigoInterno { get; set; }
        public int Año { get; set; }
        public string IdPropietario { get; set; }
        public List<CrearImagenPropiedadCommand> ImagenesPropiedad { get; set; } = new();
    }
    
    public class CrearImagenPropiedadCommand
    {
        public string Archivo { get; set; }
        public bool Habilitada { get; set; } = true;
    }
}