using Microsoft.EntityFrameworkCore;
using Schlagen.Data;
using Schlagen.Data.EntityClasses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Schlagen.Services
{
    public class EmploymentServices : IEmploymentServices
    {
        private ApplicationDbContext _context;

        public EmploymentServices(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<List<JobRequisitionForList>> GetAllJobRequisitionsAsync()
        {
            return await _context.JobRequisitions
                .AsNoTracking()
                .Select(jr =>
                new JobRequisitionForList
                {
                    JobRequisitionId = jr.JobRequisitionId,
                    Title = jr.Title,
                    DaysSincePosted 
                        = Convert.ToInt32((DateTime.Today - jr.DateToPost).TotalDays),
                    Location = jr.Location.Name
                }).ToListAsync();
        }

        public async Task<List<EmploymentType>> GetEmploymentTypesAsync()
        {
            return await _context.EmploymentTypes
                .AsNoTracking()
                .ToListAsync();
        }

        public async Task<List<JobRequisitionForList>> GetJobRequisitionsAsync(
            int employmentLocationId, int jobTypeId, int employmentTypeId)
        {
            return await _context.JobRequisitions
                .AsNoTracking()
                .Where(jr =>
                // Employment location filtering
                employmentLocationId > 0 
                ? jr.EmploymentLocationId == employmentLocationId 
                : true
                // Job type filtering
                && jobTypeId > 0 
                ? jr.JobTypeId == jobTypeId 
                : true
                // Employment type filtering
                && employmentTypeId > 0 
                ? jr.EmploymentTypeId == employmentTypeId 
                : true
                )
                .Select(jr =>
                new JobRequisitionForList
                {
                    JobRequisitionId = jr.JobRequisitionId,
                    Title = jr.Title,
                    DaysSincePosted
                        = Convert.ToInt32((DateTime.Today - jr.DateToPost).TotalDays),
                    Location = jr.Location.Name
                }).ToListAsync();
        }

        public async Task<JobRequisition> GetJobRequisitionAsync(int jobRequisitionId)
        {
            return await _context.JobRequisitions
                .AsNoTracking()
                .FirstOrDefaultAsync(jr 
                => jr.JobRequisitionId == jobRequisitionId);
        }

        public async Task<List<JobType>> GetJobTypesAsync()
        {
            return await _context.JobTypes
                .AsNoTracking()
                .ToListAsync();
        }

        public async Task<List<EmploymentLocation>> GetEmploymentLocationsAsync()
        {
            return await _context.EmploymentLocations
                .AsNoTracking()
                .ToListAsync();
        }
    }
}
