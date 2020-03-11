using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Schlagen.Data.EntityClasses
{
    [Serializable]
    public class JobRequisitionForList
    {
        public int JobRequisitionId { get; set; }
        public string Title { get; set; }
        public int DaysSincePosted { get; set; }
        public string Location { get; set; }
    }
}
