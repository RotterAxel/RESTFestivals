using System.ComponentModel.DataAnnotations;

namespace RESTFestivals.API.Models
{
    public abstract class AddressAbstractBase : DtoBase
    {
        [Required(AllowEmptyStrings = false, ErrorMessage = "Street is required")]
        [MaxLength(100, ErrorMessage = "Street may not contain more than 100 characters")]
        [MinLength(5, ErrorMessage = "Street must be at lease 5 characters long")]
        public string Street { get; set; }


        [MaxLength(100, ErrorMessage = "Street may not exceed 100 characters")]
        public string Street2 { get; set; }


        [Required(AllowEmptyStrings = false, ErrorMessage = "Number is required")]
        [MaxLength(15, ErrorMessage = "Number may not exceed 15 characters")]
        public string Number { get; set; }


        [Required(AllowEmptyStrings = false, ErrorMessage = "Postal Code is Required")]
        [MaxLength(15, ErrorMessage = "Postal Code may not exceed 15 characters")]
        public string PostalCode { get; set; }


        [Required(AllowEmptyStrings = false, ErrorMessage = "State is Required")]
        [MaxLength(50, ErrorMessage = "State may not exceed 50 characters")]
        [MinLength(5, ErrorMessage = "State may not contain less than 5 characters")]
        public string State { get; set; }


        [Required(AllowEmptyStrings = false, ErrorMessage = "Country is Required")]
        [MaxLength(50, ErrorMessage = "Country may not exceed 50 Characters")]
        [MinLength(3, ErrorMessage = "State may not contain less than 3 characters")]
        public string Country { get; set; }
        public float? Latitude { get; set; }
        public float? Longitude { get; set; }
    }
}
