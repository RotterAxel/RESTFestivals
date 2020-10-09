using System;
using System.ComponentModel.DataAnnotations;

namespace Festivals.API.Infrastructure.Entities
{
    public class AuditableEntity
    {
        public string CreatedBy { get; set; }
        
        public string ModifiedBy { get; set; }
        
        [Required]
        public DateTime CreatedOn { get; set; }

        [Required]
        public DateTime RowVersion { get; set; }
    }
}
