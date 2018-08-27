using System.Collections.Generic;
using System.Threading.Tasks;
using Xperiments.Models;
using Xperiments.Persistence.Common.Multilingual;

namespace Xperiments.Service
{
    public interface IPersonService
    {

        /// <summary>
        /// Get all the persons in the database
        /// </summary>
        /// <returns></returns>
        Task<List<IPerson>> GetAll();

        /// <summary>
        /// Returns a person whose id matched the input
        /// </summary>
        /// <param name="id">The id to match</param>
        /// <returns>Matched person</returns>
        Task<IPerson> Get(string id);

        /// <summary>
        /// Returns a person whose id matched the input. Attributes which are multilingual will only have specified
        /// language or the default language
        /// </summary>
        /// <param name="id">The id to match</param>
        /// <param name="locale">The locale in which translations need to be loaded</param>
        /// <returns>Matched person</returns>
        Task<IPerson> GetByLocale(string id, string locale);

        /// <summary>
        /// Returns all persons whose name match fully or partially the given input
        /// </summary>
        /// <param name="name">The name to be matched</param>
        /// <returns>All persons whose name matched the input</returns>
        Task<List<IPerson>> GetByName(string name);

        /// <summary>
        /// Adds a new person to the database
        /// </summary>
        /// <param name="person">The person to be added</param>
        /// <returns><code>true</code> if created successfully</returns>
        Task<bool> Add(IPerson person);

        /// <summary>
        /// Updates an existing person with new representation
        /// </summary>
        /// <param name="person">The person to be updated</param>
        /// <returns><code>true</code> if created successfully</returns>
        Task<bool> Update(IPerson person);
        
        /// <summary>
        /// Deletes an existing person
        /// </summary>
        /// <param name="id">The id of person to be deleted</param>
        /// <returns><code>true</code> if created successfully</returns>
        Task<bool> Delete(string id);

        /// <summary>
        /// Adds a translation for an existing property in the object
        /// </summary>
        /// <param name="id">Id of the person in which translation need to be added</param>
        /// <param name="request">Details of translation to be added</param>
        /// <returns><code>true</code> if added successfully</returns>
        Task<bool> AddTranslation(string id, MultilingualDataRequest request);

        /// <summary>
        /// Updates an existing translation for an existing property in the object
        /// </summary>
        /// <param name="id">Id of the person in which translation needs to be updated</param>
        /// <param name="request">Details of translation to be updated</param>
        /// <returns><code>true</code> if updated successfully</returns>
        Task<bool> UpdateTranslation(string id, MultilingualDataRequest request);
    }
}