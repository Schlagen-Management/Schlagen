using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Schlagen.Data.EntityClasses
{
    [Serializable]
    public class JobType
    {
        public int JobTypeId { get; set; }
        public string Name { get; set; }
    }
}
