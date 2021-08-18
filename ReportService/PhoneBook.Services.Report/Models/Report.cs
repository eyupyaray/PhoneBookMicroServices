using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PhoneBook.Services.Report.Models
{
    public class Report
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string UUID { get; set; }

        [BsonRepresentation(BsonType.DateTime)]
        public DateTime ReportRequestData { get; set; }
        public string ReportStatus { get; set; }
        public List<ReportContent> ReportContent { get; set; }
    }
}
