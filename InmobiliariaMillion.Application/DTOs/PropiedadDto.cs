namespace InmobiliariaMillion.Application.DTOs
{
    public class PropiedadDto
    {
        public string IdPropiedad { get; set; }
        public string Nombre { get; set; }
        public string Direccion { get; set; }
        public decimal Precio { get; set; }
        public string CodigoInterno { get; set; }
        public int Año { get; set; }
        public string IdPropietario { get; set; }
        public PropietarioDto Propietario { get; set; }
        public List<ImagenPropiedadDto> ImagenesPropiedad { get; set; }
        public List<TrazabilidadPropiedadDto> TrazabilidadesPropiedad { get; set; }
    }
}