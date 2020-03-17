using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace Schlagen.Data.EntityClasses
{
    [Serializable]
    public class InformationRequest
    {
        public int InformationRequestId { get; set; }
        [Required]
        [MaxLength(100)]
        [Display(Name = "Name")]
        public string Name { get; set; }
        [Required]
        [EmailAddress]
        [MaxLength(100)]
        [Display(Name = "Email Address")]
        public string Email { get; set; }
        [Required]
        [Phone]
        [MaxLength(25)]
        [Display(Name = "Phone Number")]
        public string Phone { get; set; }
        public int InformationRegardingId { get; set; }
        public InformationType InformationRegarding { get; set; }
        [Required]
        [MaxLength(400)]
        [Display(Name = "Request Details")]
        public string Details { get; set; }
    }
}
