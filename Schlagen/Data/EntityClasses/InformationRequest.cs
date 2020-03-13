using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Schlagen.Data.EntityClasses
{
    public class InformationRequest
    {
        public int InformationRequestId { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
        public int InformationRegardingId { get; set; }
        public InformationType InformationRegarding { get; set; }
        public string Details { get; set; }
    }
}
