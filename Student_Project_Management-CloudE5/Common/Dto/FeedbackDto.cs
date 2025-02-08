using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common.Dto
{
    public class FeedbackDto
    {
        public double Score { get; set; }
        public List<string> Errors { get; set; }
        public List<string> ImprovementSuggestions { get; set; } 
        public List<string> Recommendations { get; set; }
    }
}
