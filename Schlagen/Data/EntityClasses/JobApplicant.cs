using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Schlagen.Data.EntityClasses
{
    [Serializable]
    public class JobApplicant
    {
        public int JobApplicantId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string PhoneNumber { get; set; }
        public string EmailAddress { get; set; }
        public string ResumeText { get; set; }
        public int JobRequisitionId { get; set; }
        public virtual JobRequisition JobRequisition { get; set; }
    }
}
