using System;
using System.Collections.Generic;
using Xperiments.Persistence.Common;

namespace Xperiments.Models
{
    /// <summary>
    /// Describes a person with few attributes
    /// </summary>
    public interface IPerson : IPersistenceAggregate
    {
        /// <summary>
        /// Name of the person
        /// </summary>
        string Name { get; set; }
        
        /// <summary>
        /// Email of the person
        /// </summary>
        string Email { get; set; }
        
        /// <summary>
        /// The date of birth of the person
        /// </summary>
        DateTime DOB { get; set; }
        
        /// <summary>
        /// The address for communication with the person
        /// </summary>
        Address CommunicationAddress { get; set; }
        
        /// <summary>
        /// Something about this person in brief that describes the person
        /// This field may be multilingual
        /// </summary>
        string AboutText { get; set; }

        /// <summary>
        /// A person may have zero or more animals as pets
        /// </summary>
        List<Cat> Cats { get; set; }
    }
}