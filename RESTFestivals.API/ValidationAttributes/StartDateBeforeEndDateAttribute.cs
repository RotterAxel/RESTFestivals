using System.ComponentModel.DataAnnotations;
using RESTFestivals.API.Models;

namespace RESTFestivals.API.ValidationAttributes
{
    public class StartDateBeforeEndDateAttribute : ValidationAttribute
    {
        protected override ValidationResult IsValid(object value,
            ValidationContext validationContext)
        {
            var festival = (FestivalAbstractBase)validationContext.ObjectInstance;

            if (!(festival.StartDate < festival.EndDate))
            {
                return new ValidationResult(ErrorMessage,
                    new[] { nameof(FestivalAbstractBase) });
            }

            return ValidationResult.Success;
        }
    }
}
