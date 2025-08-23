using InmobiliariaMillion.Aplicacion.DTOs.Modelos.Propietario;
using MongoDB.Bson;

namespace InmobiliariaMillion.Aplicacion.DTOs.Modelos
{
    public class PropiedadOutputDto
    {
        public string IdPropiedad { get; set; }
        public string Nombre { get; set; }
        public string Direccion { get; set; }
        public decimal Precio { get; set; }
        public string CodigoInterno { get; set; }
        public int Anio { get; set; }
        public string IdPropietario { get; set; }
    }
}