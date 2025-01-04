using System.ComponentModel.DataAnnotations;

namespace RapidRecruit.Models.Validations
{
    public class FutureDateAttribute : ValidationAttribute
    {
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            DateTime date = (DateTime)value;
            if (date.Date <= DateTime.Now.Date)
            {
                return new ValidationResult(ErrorMessage ?? "Date must be in the future");
            }
            return ValidationResult.Success;
        }
    }
}
