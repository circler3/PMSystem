using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PMServer
{
    class WorkItem
    {
        public string Name { get; set; }
        public DateTime Deadline { get; set; }
        public int Percentage { get; set; }
        public string Description { get; set; }
        public int Priority { get; set; }
        //public string Owner { get; set; }
    }
}
