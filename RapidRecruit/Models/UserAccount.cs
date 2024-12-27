using Microsoft.AspNetCore.Identity;

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

        public virtual ICollection<JobPosting> JobPostings { get; set; } = new List<JobPosting>();

    }
}