using MongoDB.Bson;
using System.Text.Json.Serialization;

namespace InmobiliariaMillion.Aplicacion.DTOs.Modelos.ImagenPropiedad
{
    public class ImagenPropiedadInputDto
    {
        [JsonIgnore]
        public ObjectId _id { get; set; }
        [JsonIgnore]
        public string? IdImagenPropiedad { get; set; }
        public string IdPropiedad { get; set; }
        public string Archivo { get; set; }
        public bool Habilitada { get; set; }
    }
}