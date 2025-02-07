using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Common.Enum;

namespace Common.Model
{
    public class StudentWork
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }

        [BsonElement("studentId")]
        public string StudentId { get; set; }

        [BsonElement("title")]
        public string Title { get; set; }

        [BsonElement("versions")]
        public List<WorkVersion> Versions { get; set; } = new List<WorkVersion>();

        [BsonElement("status")]
        public WorkStatus Status { get; set; }

        [BsonElement("submissionDate")]
        public DateTime SubmissionDate { get; set; }

        [BsonElement("estimatedAnalysisCompletion")]
        public DateTime? EstimatedAnalysisCompletion { get; set; }

        [BsonElement("feedback")]
        public Feedback Feedback { get; set; }
    }

}
