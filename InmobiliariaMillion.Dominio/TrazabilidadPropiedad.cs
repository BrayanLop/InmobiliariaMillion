namespace InmobiliariaMillion.Dominio
{
    public class TrazabilidadPropiedad
    {
        public string IdTrazabilidadPropiedad { get; set; }
        
        public DateTime FechaVenta { get; set; }
        public string Nombre { get; set; }
        public decimal Valor { get; set; }
        public decimal Impuesto { get; set; }
        
        public string IdPropiedad { get; set; }
        
        public Propiedad Propiedad { get; set; }
        
        public decimal ValorTotal => Valor + Impuesto;
    }
}