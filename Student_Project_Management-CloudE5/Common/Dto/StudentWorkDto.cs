using Common.Enum;
using Common.Model;
using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common.Dto
{
    public class StudentWorkDto
    {
        public string Id { get; set; }
        public string StudentId { get; set; }
        public string? Course { get; set; }
        public string Title { get; set; }
        public List<WorkVersion> Versions { get; set; } = new List<WorkVersion>();

        public uint? Reverted { get; set; }
        public WorkStatus Status { get; set; }
        public DateTime SubmissionDate { get; set; }
        public DateTime? EstimatedAnalysisCompletion { get; set; }
        public Feedback? Feedback { get; set; }
    }
}
