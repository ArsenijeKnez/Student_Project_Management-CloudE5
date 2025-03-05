using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common.Dto
{
    public class ClassProgress
    {
        public int TotalStudents { get; set; }
        public double AverageClassScore { get; set; }
        public List<StudentProgress> StudentProgressList { get; set; }
    }
}
