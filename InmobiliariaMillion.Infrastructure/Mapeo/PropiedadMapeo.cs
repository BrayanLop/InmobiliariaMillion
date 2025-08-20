using MongoDB.Bson;
using MongoDB.Bson.Serialization;
using MongoDB.Bson.Serialization.Serializers;
using InmobiliariaMillion.Dominio;

namespace InmobiliariaMillion.Infrastructura.Mappings
{
    public static class PropiedadMapeo
    {
        public static void ConfigurarMapeo()
        {
            if (BsonClassMap.IsClassMapRegistered(typeof(Propiedad)))
                return;
                
            BsonClassMap.RegisterClassMap<Propiedad>(cm =>
            {
                cm.AutoMap();
                
                // Configurar ID como ObjectId
                cm.MapIdMember(c => c.IdPropiedad)
                  .SetSerializer(new StringSerializer(BsonType.ObjectId));
                  
                // Configurar FK como ObjectId
                cm.MapMember(c => c.IdPropietario)
                  .SetSerializer(new StringSerializer(BsonType.ObjectId));
                  
                // Ignorar navigation properties para evitar referencias circulares
                cm.MapMember(c => c.Propietario).SetIgnoreIfNull(true);
                
                // Mapear colecciones embebidas
                cm.MapMember(c => c.ImagenesPropiedad).SetIgnoreIfNull(true);
                cm.MapMember(c => c.TrazabilidadesPropiedad).SetIgnoreIfNull(true);
                
                // Mapear campos privados
                cm.MapMember(c => c.Nombre).SetElementName("nombre");
                cm.MapMember(c => c.Precio).SetElementName("precio");
            });
        }
    }
}