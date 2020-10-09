using System.ComponentModel.DataAnnotations;

namespace RESTFestivals.API.Infrastructure.Entities
{
    public class Address : AuditableEntity
    {
        [Key]
        public int Id { get; set; }
        [Required]
        [MaxLength(50)]
        public string Street { get; set; }
        [Required]
        [MaxLength(15)]
        public string Number { get; set; }
        [Required]
        [MaxLength(15)]
        public string PostalCode { get; set; }
        [Required]
        [MaxLength(50)]
        public string State { get; set; }
        [Required]
        [MaxLength(50)]
        public string Country { get; set; }
    }
}
