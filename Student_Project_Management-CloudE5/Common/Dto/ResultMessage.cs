using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.Serialization;

namespace Common.Dto
{

    [DataContract]
    public class ResultMessage
    {
        [DataMember]
        public bool Success { get; set; }

        [DataMember]
        public string Message { get; set; }

        public ResultMessage(bool success, string message)
        {
            Success = success;
            Message = message;
        }

        public ResultMessage() { }
    }

}
