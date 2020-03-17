using Microsoft.EntityFrameworkCore;
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

        public InformationRequestServices(ApplicationDbContext context)
        {
            _context = context;
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
            catch (Exception)
            {
                // TODO: log the exception
                return false;
            }
        }

        public async Task<List<InformationRequest>> GetInformationRequests()
        {
            return await _context.InformationRequests
                .AsNoTracking().ToListAsync();
        }

        public async Task<List<InformationType>> GetInformationTypes()
        {
            return await _context.InformationTypes
                .AsNoTracking()
                .Where(it => it.InformationTypeId > 1)
                .ToListAsync();
        }
        public async Task<InformationType> GetInformationType(int informationTypeId)
        {
            return await _context.InformationTypes
                .AsNoTracking()
                .Where(it => it.InformationTypeId == informationTypeId)
                .FirstOrDefaultAsync();
        }

    }
}
