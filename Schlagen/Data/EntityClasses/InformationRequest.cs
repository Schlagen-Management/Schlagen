using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace Schlagen.Data.EntityClasses
{
    public class InformationRequest
    {
        public int InformationRequestId { get; set; }
        [Required]
        [MaxLength(100)]
        public string Name { get; set; }
        [Required]
        [EmailAddress]
        [MaxLength(100)]
        public string Email { get; set; }
        [Required]
        [Phone]
        [MaxLength(25)]
        public string Phone { get; set; }
        public int InformationRegardingId { get; set; }
        public InformationType InformationRegarding { get; set; }
        [Required]
        [MaxLength(400)]
        public string Details { get; set; }
        public string ReCaptcha { get; set; }
    }
}
