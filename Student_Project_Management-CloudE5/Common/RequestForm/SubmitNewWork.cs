using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common.RequestForm
{
    public class SubmitNewWork
    {
        public IFormFile file { get; set; }

        public string title { get; set; }

        public string studentId { get; set; }
    }
}
