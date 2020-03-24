using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace LearnShareProcessor.dto
{
    public class Level
    {
        [JsonIgnore]
        public virtual int? RemoteEntityId { get; set; }
        public virtual bool Active { get; set; }
        public virtual string Description { get; set; }
        public virtual string Code { get; set; }
        public virtual int MemberOf { get; set; }
        public virtual int DomainID { get; set; }
    }
}
