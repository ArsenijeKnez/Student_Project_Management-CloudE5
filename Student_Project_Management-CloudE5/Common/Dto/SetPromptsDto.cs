using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common.Dto
{
    public class SetPromptsDto
    {
        public string? ErrorPrompt { get; set; }
        public string? ImprovementPrompt { get; set; }
        public string? ScorePrompt { get; set; }
    }

}
