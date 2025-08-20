using InmobiliariaMillion.Infrastructura.Mapeo;

namespace InmobiliariaMillion.Infrastructura.Mappings
{
    public static class MongoMapeoConfiguracion
    {
        private static bool _mappingsConfigured = false;
        
        public static void ConfigurarTodosLosMapeos()
        {
            if (_mappingsConfigured)
                return;
                
            PropietarioMapeo.ConfigurarMapeo();
            PropiedadMapeo.ConfigurarMapeo();
            ImagenPropiedadMapeo.ConfigurarMapeo();
            TrazabilidadPropiedadMapeo.ConfigurarMapeo();
            
            _mappingsConfigured = true;
        }
    }
}