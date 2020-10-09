using System.ComponentModel.DataAnnotations;

namespace RESTFestivals.API.Models
{
    public class FestivalForCreationDto : FestivalAbstractBase
    {
        [Required(ErrorMessage = "A Valid Adress Object is required")]
        public AddressForCreationDto Address { get; set; }
    }
}
