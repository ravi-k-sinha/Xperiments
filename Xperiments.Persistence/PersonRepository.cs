namespace Xperiments.Persistence
{
    using System;
    using System.Reflection;
    using System.Threading.Tasks;
    using Common;
    using Common.Multilingual;
    using Models;
    using MongoDB.Bson;
    using MongoDB.Bson.Serialization;
    using MongoDB.Driver;
    using MongoDB.Driver.Linq;
    using Repository;

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

            try
            {
                person.Meta["Translations"].AsBsonDocument[request.PropertyName]
                    .AsBsonDocument[request.Language]["Value"] = request.Translation;
            }
            catch (Exception e)
            {
                throw new TranslationTargetException("Translation target to update was not found. Msg: " + e.Message, e);
            }

            // TODO Use UpdateDefinition, so that only targeted field is sent for update, not the whole object
            Update(person);

            return true;
        }
    }
}