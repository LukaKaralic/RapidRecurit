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
    }
}