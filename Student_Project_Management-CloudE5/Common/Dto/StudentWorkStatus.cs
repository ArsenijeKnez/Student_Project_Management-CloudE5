using Common.Enum;
using Common.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common.Dto
{
    public class StudentWorkStatus
    {
        public string WorkId {  get; set; }
        public string Title { get; set; }
        public WorkStatus Status { get; set; }
        public DateTime SubmissionDate { get; set; }
        public DateTime? EstimatedAnalysisCompletion { get; set; }
    }
}
