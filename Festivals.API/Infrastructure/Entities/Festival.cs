using System;
using System.ComponentModel.DataAnnotations;

namespace Festivals.API.Infrastructure.Entities
{
    public class Festival : AuditableEntity
    {
        [Key]
        public int Id { get; set; }
        [Required]
        [MinLength(5)]
        [MaxLength(50)]
        public string Title { get; set; }
        [MaxLength(1000)]
        public string Description { get; set; }
        [Required]
        public DateTime StartDate { get; set; }
        [Required]
        public DateTime EndDate { get; set; }
        public string Website { get; set; }
        [Required]
        public int AddressId { get; set; }
        [Required]
        public Address Address { get; set; }
    }
}
