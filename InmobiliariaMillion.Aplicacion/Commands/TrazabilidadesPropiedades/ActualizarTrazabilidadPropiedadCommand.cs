namespace InmobiliariaMillion.Application.Commands.TrazabilidadesPropiedades
{
    public class ActualizarTrazabilidadPropiedadCommand
    {
        public string IdTrazabilidadPropiedad { get; set; }
        public DateTime FechaVenta { get; set; }
        public string Nombre { get; set; }
        public decimal Valor { get; set; }
        public decimal Impuesto { get; set; }
    }
}