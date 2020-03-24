using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace LearnShareProcessor.dto
{
    public class Person
    {
        [JsonIgnore]
        public virtual string EntityId { get; set; }
        public virtual string EmployeeID { get; set; }
        public virtual int? PersonID { get; set; }
        public virtual string UserName { get; set; }
        public virtual string Password { get; set; }
        public virtual string FirstName { get; set; }
        public virtual string LastName { get; set; }
        public virtual string Phonenum { get; set; }
        public virtual string EMail { get; set; }
        public virtual string Active { get; set; }
        public virtual string SupervisorID { get; set; }
        public virtual Properties Properties { get; set; }
        public virtual Address Address { get; set; }
        public virtual List<Domain> Domains { get; set; }

        public bool ShouldSerializePersonID()
        {
            return (PersonID != null);
        }

        public Person()
        {
            Domains = new List<Domain>();
        }
    }
}
