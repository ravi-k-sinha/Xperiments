namespace Xperiments.Persistence.Common
{
    /// <summary>
    /// Defines the configuration that can be used to establish a connection with a MongoDB instance
    /// </summary>
    public interface IMongoConfiguration
    {
        /// <summary>
        /// The connection string to be used for connecting with MongoDB instance
        /// </summary>
        string ConnectionString { get; set; }
        
        /// <summary>
        /// The name of the database in which the work needs to be done
        /// </summary>
        string DatabaseName { get; set; }
    }
}