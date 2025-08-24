using MongoDB.Bson;
using System.Text.Json.Serialization;

namespace InmobiliariaMillion.Aplicacion.DTOs.Modelos.Propietario
{
    public class PropietarioInputDto
    {
        [JsonIgnore]
        public ObjectId _id { get; set; }
        [JsonIgnore]
        public string? IdPropietario { get; set; }
        public string Nombre { get; set; }
        public string Direccion { get; set; }
        public string Foto { get; set; }
        public DateTime FechaNacimiento { get; set; }
    }
}