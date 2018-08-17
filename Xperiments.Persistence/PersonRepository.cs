using System.Reflection;
using MongoDB.Bson.Serialization;
using MongoDB.Driver;
using Xperiments.Models;
using Xperiments.Persistence.Common;
using Xperiments.Repository;

namespace Xperiments.Persistence
{
    public class PersonRepository : BaseMongoRepository<IPerson, Person>, IPersonRepository
    {

        static PersonRepository()
        {
            BsonClassMap.RegisterClassMap<Person>(
                map =>
                {
                    map.AutoMap();
                    var type = typeof(Person);
                    map.SetDiscriminator($"{type.FullName}, {type.GetTypeInfo().Assembly.GetName().Name}");
                    map.SetIsRootClass(true);
                }
            );
        }
        
        public PersonRepository(IMongoConfiguration mongoConfiguration) : base(mongoConfiguration, "persons")
        {
            CreateIndexIfNotExists("person-index",
                Builders<IPerson>.IndexKeys.Ascending(p => p.TenantId).Ascending(p => p.Id));
        }
    }
}