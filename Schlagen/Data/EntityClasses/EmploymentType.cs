using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Schlagen.Data.EntityClasses
{
    [Serializable]
    public class EmploymentType
    {
        public int EmploymentTypeId { get; set; }
        public string Name { get; set; }
    }
}
