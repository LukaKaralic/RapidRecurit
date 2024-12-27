using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;

namespace RapidRecruit.Models
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

    public enum AccountType
    {
        Business,
        Person
    }

    [BusinessNameValidation]
    public class UserAccount : IdentityUser
    {
        public AccountType AccountType { get; set; }
        public string? BusinessName { get; set; }
        public virtual ICollection<JobPosting> JobPostings { get; set; } = new List<JobPosting>();
    }
}