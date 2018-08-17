namespace Xperiments.Persistence.Common
{
    public abstract class PersistenceAggregate : IPersistenceAggregate
    {
        public string TenantId { get; set; }
        public string Id { get; set; }
    }
}