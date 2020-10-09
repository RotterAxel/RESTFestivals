using System;
using System.ComponentModel.DataAnnotations;
using RESTFestivals.API.ValidationAttributes;

namespace RESTFestivals.API.Models
{

    [StartDateBeforeEndDate(ErrorMessage = "Start date must happen earlier than the end date.")]
    [FestivalTitleMustBeDifferentFromDescription(ErrorMessage = "Title must be different from description.")]
    public abstract class FestivalAbstractBase : DtoBase
    {
        [Required(AllowEmptyStrings = false, ErrorMessage = "Title is required.")]
        [MaxLength(50, ErrorMessage = "Title is too long.")]
        [MinLength(5, ErrorMessage = "Title must be at least 5 characters long.")]
        public string Title { get; set; }

        [MaxLength(1000, ErrorMessage = "Description is too long.")]
        public string Description { get; set; }

        [Required(AllowEmptyStrings = false, ErrorMessage = "The start date is required.")]
        public DateTime StartDate { get; set; }
        [Required(AllowEmptyStrings = false, ErrorMessage = "The end date is required.")]
        public DateTime EndDate { get; set; }
        public string Website { get; set; }
    }
}
