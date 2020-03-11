using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Schlagen.Data.EntityClasses
{
    [Serializable]
    public class JobRequisition
    {
        public int JobRequisitionId { get; set; }
        public string Description { get; set; }
        public string Title { get; set; }
        public DateTime DateToPost { get; set; }
        public DateTime? DateToRemove { get; set; }
        public int JobTypeId { get; set; }
        public virtual JobType JobType { get; set; }
        public int EmploymentLocationId { get; set; }
        public virtual EmploymentLocation Location { get; set; }
        public int EmploymentTypeId { get; set; }
        public virtual EmploymentType EmploymentType { get; set; }
        public virtual IList<JobApplicant> Applicants { get; set; }
    }
}
