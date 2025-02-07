using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common.Model
{
    public class Feedback
    {
        [BsonElement("score")]
        public double Score { get; set; }

        [BsonElement("errors")]
        public List<string> Errors { get; set; } = new List<string>();

        [BsonElement("improvementSuggestions")]
        public List<string> ImprovementSuggestions { get; set; } = new List<string>();

        [BsonElement("recommendations")]
        public List<string> Recommendations { get; set; } = new List<string>();
    }

}
