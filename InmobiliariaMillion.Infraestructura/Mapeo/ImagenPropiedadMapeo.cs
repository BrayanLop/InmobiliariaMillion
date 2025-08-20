using MongoDB.Bson;
using MongoDB.Bson.Serialization;
using MongoDB.Bson.Serialization.Serializers;
using InmobiliariaMillion.Dominio;

namespace InmobiliariaMillion.Infrastructura.Mapeo
{
    public static class ImagenPropiedadMapeo
    {
        public static void ConfigurarMapeo()
        {
            if (BsonClassMap.IsClassMapRegistered(typeof(ImagenPropiedad)))
                return;
                
            BsonClassMap.RegisterClassMap<ImagenPropiedad>(cm =>
            {
                cm.AutoMap();
                
                // Configurar ID como ObjectId
                cm.MapIdMember(c => c.IdImagenPropiedad)
                  .SetSerializer(new StringSerializer(BsonType.ObjectId));
                  
                // Configurar FK como ObjectId
                cm.MapMember(c => c.IdPropiedad)
                  .SetSerializer(new StringSerializer(BsonType.ObjectId));
                  
                // Ignorar navigation property
                cm.MapMember(c => c.Propiedad).SetIgnoreIfNull(true);
                
                // Configurar nombres de elementos
                cm.MapMember(c => c.Archivo).SetElementName("archivo");
                cm.MapMember(c => c.Habilitada).SetElementName("habilitada");
            });
        }
    }
}