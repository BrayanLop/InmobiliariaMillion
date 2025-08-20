using InmobiliariaMillion.Dominio;
using MongoDB.Bson;
using MongoDB.Bson.Serialization;
using MongoDB.Bson.Serialization.Serializers;

namespace InmobiliariaMillion.Infrastructura.Mappings
{
    public static class TrazabilidadPropiedadMapeo
    {
        public static void ConfigurarMapeo()
        {
            if (BsonClassMap.IsClassMapRegistered(typeof(TrazabilidadPropiedad)))
                return;
                
            BsonClassMap.RegisterClassMap<TrazabilidadPropiedad>(cm =>
            {
                cm.AutoMap();
                
                // Configurar ID como ObjectId
                cm.MapIdMember(c => c.IdTrazabilidadPropiedad)
                  .SetSerializer(new StringSerializer(BsonType.ObjectId));
                  
                // Configurar FK como ObjectId
                cm.MapMember(c => c.IdPropiedad)
                  .SetSerializer(new StringSerializer(BsonType.ObjectId));
                  
                // Ignorar navigation property
                cm.MapMember(c => c.Propiedad).SetIgnoreIfNull(true);
                
                // Ignorar propiedades calculadas
                cm.MapMember(c => c.ValorTotal).SetIgnoreIfDefault(true);
            });
        }
    }
}