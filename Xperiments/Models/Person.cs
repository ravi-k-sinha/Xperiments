using System;
using System.Collections.Generic;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using Newtonsoft.Json;
using Xperiments.Persistence;
using Xperiments.Persistence.Common;

namespace Xperiments.Models
{
    public class Person : PersistenceAggregate, IPerson
    {

        public Person()
        {
            //TranslationDoc = new BsonDocument("A", "B");
        }
        
        public string Name { get; set; }
        
        public string Email { get; set; }
        public DateTime DOB { get; set; }
        public Address CommunicationAddress { get; set; }
        public string AboutText { get; set; }
        public List<Cat> Cats { get; set; }
        
        public string Remarks { get; set; }

        /*public MultilingualText Name2 { get; set; }
        [BsonIgnore]
        public List<MultilingualText> AllMultilingualTexts => new List<MultilingualText>
        {
            Name2
        };*/

        [JsonIgnore]
        public BsonDocument Meta { get; set; }
    }
}