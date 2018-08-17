using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using MongoDB.Bson;
using MongoDB.Bson.Serialization;
using MongoDB.Bson.Serialization.IdGenerators;
using MongoDB.Bson.Serialization.Serializers;
using MongoDB.Driver;
using MongoDB.Driver.Linq;

namespace Xperiments.Persistence.Common
{
    public class BaseMongoRepository<T, C> : IRepository<T> where T : IPersistenceAggregate 
                                                            where C : PersistenceAggregate, T
    {

        protected readonly IMongoCollection<T> MongoCollection;
        
        // TODO: Purpose of this collection is not fully clear
        private static readonly ConcurrentDictionary<string, IMongoCollection<T>> MongoCollections
            = new ConcurrentDictionary<string, IMongoCollection<T>>();
        
        static BaseMongoRepository()
        {
            BsonSerializer.RegisterSerializer(new ImpliedImplementationInterfaceSerializer<T,C>());

            if (BsonClassMap.IsClassMapRegistered(typeof(PersistenceAggregate)))
            {
                return;
            }

            BsonClassMap.RegisterClassMap<PersistenceAggregate>(map =>
            {
                map.AutoMap();
                map.MapIdField(m => m.Id)
                    .SetIdGenerator(StringObjectIdGenerator.Instance)
                    .SetSerializer(new StringSerializer(BsonType.ObjectId))
                    .SetIgnoreIfDefault(true);
            });
        }

        protected BaseMongoRepository(IMongoConfiguration mongoConfiguration, string collectionName)
        {
            var key = mongoConfiguration.ConnectionString + mongoConfiguration.DatabaseName + collectionName;

            if (MongoCollections.TryGetValue(key, out var tempCollection))
            {
                return;
            }
            
            var client = new MongoClient(mongoConfiguration.ConnectionString);
            var database = client.GetDatabase(mongoConfiguration.DatabaseName);
            MongoCollection = tempCollection = database.GetCollection<T>(collectionName);
            CreateIndexIfNotExists("tenant", Builders<T>.IndexKeys.Ascending(i => i.TenantId));
            MongoCollections.TryAdd(key, tempCollection);
        }

        public async void CreateIndexIfNotExists(string indexName, IndexKeysDefinition<T> index)
        {
            await MongoCollection.Indexes.ListAsync().ContinueWith(async t =>
            {
                var indexes = await t.Result.ToListAsync();
                if (indexes.All(i => i["name"] != indexName))
                {
                    await MongoCollection.Indexes.CreateOneAsync(index, new CreateIndexOptions
                    {
                        Name = indexName
                    });
                }
            });
        }
        
        public async void CreateIndexIfNotExists(string indexName, IndexKeysDefinition<T> index, bool unique = false)
        {
            await MongoCollection.Indexes.ListAsync().ContinueWith(async t =>
            {
                var indexes = await t.Result.ToListAsync();
                if (indexes.All(i => i["name"] != indexName))
                {
                    await MongoCollection.Indexes.CreateOneAsync(index, new CreateIndexOptions
                    {
                        Name = indexName,
                        Unique = unique
                    });
                }
            });
        }

        // Ideally tenant id will be available via a service or another facility, and for the same a predicate will be added
        private IMongoQueryable<T> Query => MongoCollection.AsQueryable();

        public async Task<T> Get(string id)
        {
            return await Query.FirstOrDefaultAsync(i => i.Id == id);
        }

        public async Task<IEnumerable<T>> All(Expression<Func<T, bool>> query, int? skip = null, int? quantity = null)
        {
            var cursor = Query.Where(query);

            if (skip.HasValue)
            {
                cursor.Skip(skip.Value);
            }

            if (quantity.HasValue)
            {
                cursor.Take(quantity.Value);
            }

            return await cursor.ToListAsync();
        }

        public void Add(T item)
        {
            EnsureNotNull(item);

            item.Id = ObjectId.GenerateNewId().ToString();
            item.TenantId = "TBD"; // Ideally Tenant Id should be retrieved from a service
            MongoCollection.InsertOne(item);
        }

        public void Remove(T item)
        {
            EnsureNotNull(item);
            MongoCollection.DeleteOne(FindByIdPredicate(item));
        }

        public void Update(T item)
        {
            EnsureNotNull(item);
            MongoCollection.ReplaceOne(FindByIdPredicate(item), item);
        }

        public int Count(Expression<Func<T, bool>> query)
        {
            return Query.Count(query);
        }

        private Expression<Func<T, bool>> FindByIdPredicate(T item)
        {
            return i =>
                // i.TenantId == "TBD" && // TODO Ideally to be matched with current tenants id
                i.TenantId == item.TenantId &&
                i.Id == item.Id;
        }

        private static void EnsureNotNull(T item)
        {
            if (item == null)
            {
                throw new ArgumentNullException(nameof(item));
            }
        }
    }
}