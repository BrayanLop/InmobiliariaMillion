using MongoDB.Bson;

namespace InmobiliariaMillion.Aplicacion.DTOs.Modelos.Propiedad
{
    public class PropiedadInputDto
    {
        public ObjectId _id { get; set; }
        public string IdPropiedad { get; set; }
        public string Nombre { get; set; }
        public string Direccion { get; set; }
        public decimal Precio { get; set; }
        public string CodigoInterno { get; set; }
        public int Anio { get; set; }
        public string IdPropietario { get; set; }
    }
}