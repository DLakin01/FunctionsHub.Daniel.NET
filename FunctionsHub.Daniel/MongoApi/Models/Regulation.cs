using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace MongoApi.Models
{
    public class Regulation
    {
        [BsonId]
        public ObjectId InternalId { get; set; }

        [BsonDateTimeOptions]
        public DateTime UpdatedOn { get; set; } = DateTime.Now;

        // External ID, easier to reference
        public string Id { get; set; }
        public string RegTitle { get; set; }
        public string CollectionName { get; set; }
        public string RegText { get; set; }
        public string jurisdiction { get; set; }
        public string RegType { get; set; }
    }
}
