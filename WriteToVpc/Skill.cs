using System;
using System.Collections.Generic;
using System.Text;

namespace WriteToVpc
{
    public class Skill
    {        
        public virtual string Code { get; set; }
        public virtual string Name { get; set; }
        public virtual string Description { get; set; }
        public virtual string Category { get; set; }
        public virtual bool Active { get; set; }
        public virtual int SortOrder { get; set; }
    }
}
