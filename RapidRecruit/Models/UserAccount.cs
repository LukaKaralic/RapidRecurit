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
        public string? BusinessName { get; set; }
        public virtual ICollection<JobPosting> JobPostings { get; set; } = new List<JobPosting>();
    }
}