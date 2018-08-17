namespace Xperiments.Persistence.Common
{
    public class MongoConfiguration : IMongoConfiguration
    {
        public string ConnectionString { get; set; }
        public string DatabaseName { get; set; }
        
        public MongoConfiguration(string connectionString, string dbName)
        {
            ConnectionString = connectionString;
            DatabaseName = dbName;
        }
    }
}