using MongoDB.Bson;

namespace InmobiliariaMillion.Aplicacion.DTOs.Modelos.Propietario
{
    public class PropietarioOutputDto
    {
        public ObjectId _id { get; set; }
        public string IdPropietario { get; set; }
        public string Nombre { get; set; }
        public string Direccion { get; set; }
        public string Foto { get; set; }
        public string FechaNacimiento { get; set; }
    }
}