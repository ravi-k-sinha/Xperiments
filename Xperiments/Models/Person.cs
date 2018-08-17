using System;
using System.Collections.Generic;
using Xperiments.Persistence.Common;

namespace Xperiments.Models
{
    public class Person : PersistenceAggregate, IPerson
    {
        public string Name { get; set; }
        public string Email { get; set; }
        public DateTime DOB { get; set; }
        public Address CommunicationAddress { get; set; }
        public string AboutText { get; set; }
        public List<Cat> Cats { get; set; }
    }
}