using Schlagen.Data.EntityClasses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Schlagen.Services
{
    public interface IInformationRequestServices
    {
        public Task<bool> AddInformationRequest(
           string name, string email, string phone,
           int informationRegardingId, string details);

        public Task<List<InformationRequest>> GetInformationRequests();

        public Task<List<InformationType>> GetInformationTypes();
    }
}
