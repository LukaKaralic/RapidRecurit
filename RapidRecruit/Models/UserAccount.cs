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

        [Required(ErrorMessage = "Ime je obavezno")]
        public string FirstName { get; set; }

        [Required(ErrorMessage = "Prezime je obavezno")]
        public string LastName { get; set; }
        public string? BusinessName { get; set; }
        public virtual ICollection<JobPosting> JobPostings { get; set; } = new List<JobPosting>();
    }
}