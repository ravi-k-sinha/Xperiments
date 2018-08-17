using Xperiments.Persistence.Common;

namespace Xperiments.Models
{
    /// <summary>
    /// An animal
    /// </summary>
    public interface IAnimal : IPersistenceAggregate
    {
        /// <summary>
        /// Name of the animal if any
        /// </summary>
        string Name { get; set;}
        
        /// <summary>
        /// Number of legs this animal has
        /// </summary>
        int NumOfLegs { get; set; }
    }
}