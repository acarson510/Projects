using System;
using System.Collections.Generic;
using System.Text;

namespace LearnShareProcessor
{
    public class LearnShareApiResponse
    {
        public virtual int StatusCode { get; set; }
        public virtual string StatusDescription { get; set; }
        public virtual string RequestMethod { get; set; }
        public virtual string RequestPayload { get; set; }
        public virtual string ResponsePayload { get; set; }
        public virtual string AccessToken { get; set; }
        public virtual string EmployeeID { get; set; }
        public virtual int? PersonID { get; set; }
        public virtual int? Level { get; set; }
        public virtual string ErrorMessage { get; set; }
    }
}
