using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;
using RapidRecruit.Models.Validations;

namespace RapidRecruit.Models
{
    

    public enum AccountType
    {
        Business,
        Person
    }

    public class UserAccount : IdentityUser
    {
        public AccountType AccountType { get; set; }
        [BusinessNameValidation]

        [Required]
        public string FirstName { get; set; }
        [Required]
        public string LastName { get; set; }
        public string? BusinessName { get; set; }
        public virtual ICollection<JobPosting> JobPostings { get; set; } = new List<JobPosting>();
    }
}