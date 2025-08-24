using MongoDB.Bson;

namespace InmobiliariaMillion.Aplicacion.DTOs.Modelos.ImagenPropiedad
{
    public class ImagenPropiedadInputDto
    {
        public ObjectId _id { get; set; }
        public string IdImagenPropiedad { get; set; }
        public string IdPropiedad { get; set; }
        public string Archivo { get; set; }
        public bool Habilitada { get; set; }
    }
}