using MongoDB.Bson;
using System.Text.Json.Serialization;

namespace InmobiliariaMillion.Aplicacion.DTOs.Modelos.TrazabilidadPropiedad
{
    public class TrazabilidadPropiedadInputDto
    {
        [JsonIgnore]
        public ObjectId _id { get; set; }
        [JsonIgnore]
        public string? IdTrazabilidadPropiedad { get; set; }
        public DateTime FechaVenta { get; set; }
        public string Nombre { get; set; }
        public decimal Valor { get; set; }
        public decimal Impuesto { get; set; }
        public string IdPropiedad { get; set; } 
    }
}