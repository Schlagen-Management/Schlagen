using Schlagen.Data.EntityClasses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Schlagen.Services
{
    public interface IEmploymentServices
    {
        public Task<List<JobRequisitionForList>> GetAllJobRequisitionsAsync();
        public Task<List<JobRequisitionForList>> GetJobRequisitionsAsync(
            int employmentLocationId, int jobTypeId, int employmentTypeId);
        public Task<List<JobType>> GetJobTypesAsync();
        public Task<List<EmploymentType>> GetEmploymentTypesAsync();
        public Task<List<EmploymentLocation>> GetEmploymentLocationsAsync();
    }
}
