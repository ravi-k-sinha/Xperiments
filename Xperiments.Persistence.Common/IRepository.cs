namespace Xperiments.Persistence.Common
{
    using System;
    using System.Collections.Generic;
    using System.Linq.Expressions;
    using System.Threading.Tasks;
    
    public interface IRepository<T> where T : IPersistenceAggregate
    {
        /// <summary>
        /// Returns the entity identified by the given identifier
        /// </summary>
        /// <param name="id">The identifier to match against</param>
        /// <returns>The desired entity</returns>
        Task<T> Get(string id);

        /// <summary>
        /// Returns all instance of the entity which matches the given query
        /// </summary>
        /// <param name="query">Query expression to match</param>
        /// <param name="skip">how many to skip</param>
        /// <param name="quantity">how many to return</param>
        /// <returns>the list of queried entities</returns>
        Task<IEnumerable<T>> All(Expression<Func<T, bool>> query, int? skip = null, int? quantity = null);

        /// <summary>
        /// Inserts a new instance of the entity
        /// </summary>
        /// <param name="item">the entity to insert in the database</param>
        void Add(T item);

        /// <summary>
        /// Removes an existing instance of the entity
        /// </summary>
        /// <param name="item">the entity to be removed from the database</param>
        void Remove(T item);

        /// <summary>
        /// Uodates an existing instance of the entity with given details
        /// </summary>
        /// <param name="item">the entity data to be updated</param>
        void Update(T item);
        
        /// <summary>
        /// Returns the count of matching instances with the given query
        /// </summary>
        /// <param name="query">the query to run</param>
        /// <returns>Count of matched instances</returns>
        int Count(Expression<Func<T, bool>> query);
    }
}