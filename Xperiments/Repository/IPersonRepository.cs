using System.Threading.Tasks;
using Xperiments.Models;
using Xperiments.Persistence.Common;

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
    }
}