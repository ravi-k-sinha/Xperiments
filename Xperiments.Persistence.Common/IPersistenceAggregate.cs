namespace Xperiments.Persistence.Common
{
    public interface IPersistenceAggregate
    {
        /// <summary>
        /// Every persistable entity should belong to a tenant 
        /// </summary>
        string TenantId { get; set; }
        
        /// <summary>
        /// Every persistable entity should have a unique identifier
        /// </summary>
        string Id { get; set; }
    }
}