using Xperiments.Models;
using Xperiments.Persistence.Common;

namespace Xperiments.Repository
{
    public interface IPersonRepository : IRepository<IPerson>
    {
        // TODO Maybe add more repo methods specific to Person
    }
}