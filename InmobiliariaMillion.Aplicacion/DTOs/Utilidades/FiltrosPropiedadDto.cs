namespace InmobiliariaMillion.Aplicacion.DTOs.Utilidades
{
    public class FiltrosPropiedadDto
    {
        public string? Nombre { get; set; }
        public string? Direccion { get; set; }
        public decimal? PrecioMinimo { get; set; }
        public decimal? PrecioMaximo { get; set; }
    }
}