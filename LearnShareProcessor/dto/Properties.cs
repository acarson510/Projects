using System;
using System.Collections.Generic;
using System.Text;

namespace LearnShareProcessor.dto
{
    public class Properties
    {
        public virtual DateTime hiredate { get; set; }
        public virtual DateTime? termdate { get; set; }
        public virtual string costcategory { get; set; }
    }
}
