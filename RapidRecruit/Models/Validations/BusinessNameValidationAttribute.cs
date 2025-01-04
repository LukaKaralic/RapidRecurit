using System.ComponentModel.DataAnnotations;

namespace RapidRecruit.Models.Validations
{
    public class BusinessNameValidationAttribute : ValidationAttribute
    {
        protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
        {
            var user = (UserAccount)validationContext.ObjectInstance;

            if (user.AccountType == AccountType.Business && string.IsNullOrEmpty(user.BusinessName))
            {
                return new ValidationResult("Business Name is required for Business accounts");
            }

            if (user.AccountType == AccountType.Person && !string.IsNullOrEmpty(user.BusinessName))
            {
                user.BusinessName = null;
            }

            return ValidationResult.Success;
        }
    }
}
