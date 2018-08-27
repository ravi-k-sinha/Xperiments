using System.Threading.Tasks;
using MongoDB.Driver;
using Xperiments.Models;
using Xperiments.Persistence.Common;
using Xperiments.Persistence.Common.Multilingual;

namespace Xperiments.Repository
{
    public interface IPersonRepository : IRepository<IPerson>
    {
        /// <summary>
        /// Returns the entity identified by the given identifier
        /// </summary>
        /// <param name="id">The identifier to match against</param>
        /// <returns>The desired entity</returns>
        Task<IPerson> GetByLocale(string id, string locale);

        /// <summary>
        /// Add a new translation for an existing property
        /// </summary>
        /// <param name="id">Id of the person in which translation will be added</param>
        /// <param name="request">Details of translation to be added</param>
        /// <returns><code>true</code>, if added successfully</returns>
        Task<bool> AddTranslation(string id, MultilingualDataRequest request);

        /// <summary>
        /// Updates an existing translation for an existing property
        /// </summary>
        /// <param name="id">Id of the person in which translation will be updated</param>
        /// <param name="request">Details of update to be made</param>
        /// <returns><code>true</code>, if updated successfully</returns>
        Task<bool> UpdateTranslation(string id, MultilingualDataRequest request);
    }
}