using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace LearnShareProcessor.dto
{
    public class Domain
    {
        [JsonProperty(PropertyName = "Domain")]
        public virtual int DomainID { get; set; }
        public virtual string Description { get; set; }
        public virtual bool Active { get; set; }
        public virtual List<Level> Levels { get; set; }

        public Domain()
        {
            Levels = new List<Level>();
        }
    }
}
