using MongoDB.Driver;

namespace InmobiliariaMillion.Infrastructura.Configuration
{
    public class MongoDbConfiguracion
    {
        public string ConnectionString { get; set; }
        public string DatabaseName { get; set; }
        
        public static IMongoDatabase ConfigurarBaseDatos(string connectionString, string databaseName)
        {            
            var client = new MongoClient(connectionString);
            return client.GetDatabase(databaseName);
        }
    }
}