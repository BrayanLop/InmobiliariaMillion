namespace InmobiliariaMillion.Application.Commands.Propiedades
{
    public class ActualizarPropiedadCommand
    {
        public string IdPropiedad { get; set; }
        public string Nombre { get; set; }
        public string Direccion { get; set; }
        public decimal Precio { get; set; }
        public string CodigoInterno { get; set; }
        public int Año { get; set; }
        public string IdPropietario { get; set; }
    }
}