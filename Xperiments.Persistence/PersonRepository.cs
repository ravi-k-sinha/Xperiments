using System.Reflection;
using System.Threading.Tasks;
using MongoDB.Bson;
using MongoDB.Bson.Serialization;
using MongoDB.Driver;
using MongoDB.Driver.Linq;
using Xperiments.Models;
using Xperiments.Persistence.Common;
using Xperiments.Persistence.Common.Multilingual;
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

        public async Task<IPerson> GetByLocale(string id, string locale)
        {
            
            var bson = Query
                .Where(i => i.Id == id)
                .FirstOrDefaultAsync().ToBsonDocument();
            
            
            // TODO Implement the logic to read translation by locale and then set it in the object before returning

            return await Task.Run(() => BsonSerializer.Deserialize<IPerson>(bson));
        }

        public async Task<bool> AddTranslation(string id, MultilingualDataRequest request)
        {
            var person = await Get(id);

            var translations = person.Meta["Translations"].AsBsonDocument;
            
            translations[request.PropertyName].AsBsonDocument[request.Language] = new BsonDocument("Value", request.Translation);
            
            Update(person);

            return true;
        }

        public async Task<bool> UpdateTranslation(string id, MultilingualDataRequest request)
        {
            var person = await Get(id);

            var target = person.Meta["Translations"].AsBsonDocument[request.PropertyName]
                .AsBsonDocument[request.Language];

            target["Value"] = request.Translation;
            
            Update(person);

            return true;
        }
    }
}