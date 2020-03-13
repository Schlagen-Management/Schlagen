using Microsoft.EntityFrameworkCore;
using Schlagen.Data;
using Schlagen.Data.EntityClasses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Schlagen.Services
{
    public class InformationServices : IInformationRequestServices
    {
        private ApplicationDbContext _context { get; set; }

        public InformationServices(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<bool> AddInformationRequest(
            string name, string email, string phone, 
            int informationRegardingId, string details)
        {
            _context.Add(new InformationRequest
            {
                Name = name,
                Email = email,
                Phone = phone,
                InformationRegardingId = informationRegardingId,
                Details = details
            });

            return await _context.SaveChangesAsync() != 0;
        }

        public async Task<List<InformationRequest>> GetInformationRequests()
        {
            return await _context.InformationRequests.ToListAsync();
        }

        public async Task<List<InformationType>> GetInformationTypes()
        {
            return await _context.InformationTypes.ToListAsync();
        }
    }
}
