using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Schlagen.Data;
using Schlagen.Data.EntityClasses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Schlagen.Services
{
    public class InformationRequestServices : IInformationRequestServices
    {
        private ApplicationDbContext _context { get; set; }

        private ILogger _logger { get; set; }

        public InformationRequestServices(ApplicationDbContext context, ILogger<InformationRequestServices> logger)
        {
            _context = context;

            _logger = logger;
        }

        public async Task<bool> AddInformationRequest(
            string name, string email, string phone, 
            int informationRegardingId, string details)
        {
            try
            {
                _context.Add(new InformationRequest
                {
                    Name = name,
                    Email = email,
                    Phone = phone,
                    InformationRegardingId
                    = informationRegardingId == 0
                    ? 1 : informationRegardingId,
                    Details = details
                });

                return await _context.SaveChangesAsync() != 0;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "InformationRequestServices:AddInformationRequest - Exception occurred executing ApplicationDbContext.SaveChangesAsync()");

                return false;
            }
        }

        public async Task<List<InformationRequest>> GetInformationRequests()
        {
            // Retrieve all the information requests
            return await _context.InformationRequests
                .AsNoTracking().ToListAsync();
        }

        public async Task<List<InformationType>> GetInformationTypes()
        {
            // Get the information types.  Ignore item = 1, "N/A"
            return await _context.InformationTypes
                .AsNoTracking()
                .Where(it => it.InformationTypeId > 1)
                .ToListAsync();
        }

        
        public async Task<InformationType> GetInformationType(int informationTypeId)
        {
            // Get the specified information type item
            return await _context.InformationTypes
                .AsNoTracking()
                .Where(it => it.InformationTypeId == informationTypeId)
                .FirstOrDefaultAsync();
        }

    }
}
