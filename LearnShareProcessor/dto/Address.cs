using System;
using System.Collections.Generic;
using System.Text;

namespace LearnShareProcessor.dto
{
    public class Address
    {
        public virtual string Street1 { get; set; }
        public virtual string Street2 { get; set; }
        public virtual string City { get; set; }
        public virtual string State { get; set; }
        public virtual string Postal { get; set; }
        public virtual string Country { get; set; }
    }
}
