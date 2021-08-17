using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PhoneBook.Services.Person.Models
{
    public class Contact
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string UUID { get; set; }
        [BsonRepresentation(BsonType.ObjectId)]
        public string ContactTypeUUID { get; set; }
        public string Content { get; set; }
        [BsonIgnore]
        public ContactType ContactType { get; set; }

    }
}
