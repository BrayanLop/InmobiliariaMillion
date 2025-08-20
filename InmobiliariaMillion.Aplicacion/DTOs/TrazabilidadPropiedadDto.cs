namespace InmobiliariaMillion.Application.DTOs
{
    public class TrazabilidadPropiedadDto
    {
        public string IdTrazabilidadPropiedad { get; set; }
        public DateTime FechaVenta { get; set; }
        public string Nombre { get; set; }
        public decimal Valor { get; set; }
        public decimal Impuesto { get; set; }
        public string IdPropiedad { get; set; }
    }
}