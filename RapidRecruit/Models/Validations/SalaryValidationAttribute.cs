using System.ComponentModel.DataAnnotations;

namespace RapidRecruit.Models.Validations
{
    public class SalaryValidationAttribute : ValidationAttribute
    {
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            var jobPosting = (JobPosting)validationContext.ObjectInstance;

            if (jobPosting.MaximumSalary <= jobPosting.MinimumSalary)
            {
                return new ValidationResult(ErrorMessage ?? "Maximum salary must be greater than minimum salary");
            }

            return ValidationResult.Success;
        }
    }
}
