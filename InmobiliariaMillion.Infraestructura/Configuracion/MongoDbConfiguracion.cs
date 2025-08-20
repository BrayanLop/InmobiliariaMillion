using MongoDB.Driver;
using InmobiliariaMillion.Infrastructura.Mappings;

namespace InmobiliariaMillion.Infrastructura.Configuration
{
    public class MongoDbConfiguracion
    {
        public string ConnectionString { get; set; }
        public string DatabaseName { get; set; }
        
        public static IMongoDatabase ConfigurarBaseDatos(string connectionString, string databaseName)
        {
            // Configurar mapeos ANTES de crear cualquier conexión
            MongoMapeoConfiguracion.ConfigurarTodosLosMapeos();
            
            var client = new MongoClient(connectionString);
            return client.GetDatabase(databaseName);
        }
    }
}