using MongoDB.Bson;

namespace Xperiments.Persistence.Common
{
    /// <summary>
    /// Entities that support meta data can implement this interface
    /// </summary>
    public interface ISupportsMeta
    {
        /// <summary>
        /// A container to house meta information about a document. This property must be set with [JsonIgnore]
        /// </summary>
        BsonDocument Meta { get; set; }
    }
}