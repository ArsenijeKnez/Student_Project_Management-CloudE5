using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common.Dto
{
    public class StudentProgress
    {
        public string StudentId { get; set; }
        public int TotalWorks { get; set; }
        public double AverageScore { get; set; }
        public Dictionary<DateTime, double> ScoreHistory { get; set; } = new Dictionary<DateTime, double>();
    }
}



//public class StudentProgress
//{
//    [BsonId]
//    [BsonRepresentation(BsonType.ObjectId)]
//    public string Id { get; set; }

//    [BsonElement("studentId")]
//    public string StudentId { get; set; }

//    [BsonElement("totalWorks")]
//    public int TotalWorks { get; set; }

//    [BsonElement("averageScore")]
//    public double AverageScore { get; set; }

//    [BsonElement("scoreHistory")]
//    public Dictionary<DateTime, double> ScoreHistory { get; set; } = new Dictionary<DateTime, double>();
//}