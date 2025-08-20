using MongoDB.Bson;
using MongoDB.Bson.Serialization;
using MongoDB.Bson.Serialization.Serializers;
using InmobiliariaMillion.Dominio;

namespace InmobiliariaMillion.Infrastructura.Mappings
{
    public static class PropietarioMapeo
    {
        public static void ConfigurarMapeo()
        {
            if (BsonClassMap.IsClassMapRegistered(typeof(Propietario)))
                return;
                
            BsonClassMap.RegisterClassMap<Propietario>(cm =>
            {
                cm.AutoMap();
                
                // Configurar ID como ObjectId
                cm.MapIdMember(c => c.IdPropietario)
                  .SetSerializer(new StringSerializer(BsonType.ObjectId));
                  
                // Ignorar navigation property
                cm.MapMember(c => c.Propiedades).SetIgnoreIfNull(true);
                
                // Configurar nombres de elementos
                cm.MapMember(c => c.Nombre).SetElementName("nombre");
                cm.MapMember(c => c.Direccion).SetElementName("direccion");
                cm.MapMember(c => c.FechaNacimiento).SetElementName("fechaNacimiento");
            });
        }
    }
}