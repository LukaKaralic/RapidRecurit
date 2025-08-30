using Microsoft.AspNetCore.Identity;

namespace RapidRecruit
{
    public class SerbianIdentityErrorDescriber : IdentityErrorDescriber
    {
        public override IdentityError DefaultError() => new IdentityError
        {
            Code = nameof(DefaultError),
            Description = "Došlo je do nepoznate greške."
        };

        public override IdentityError ConcurrencyFailure() => new IdentityError
        {
            Code = nameof(ConcurrencyFailure),
            Description = "Greška optimističke konkurentnosti, objekat je modifikovan."
        };

        public override IdentityError PasswordMismatch() => new IdentityError
        {
            Code = nameof(PasswordMismatch),
            Description = "Netačna lozinka."
        };

        public override IdentityError InvalidToken() => new IdentityError
        {
            Code = nameof(InvalidToken),
            Description = "Nevaljan token."
        };

        public override IdentityError LoginAlreadyAssociated() => new IdentityError
        {
            Code = nameof(LoginAlreadyAssociated),
            Description = "Korisnik sa ovim prijavom već postoji."
        };

        public override IdentityError InvalidUserName(string userName) => new IdentityError
        {
            Code = nameof(InvalidUserName),
            Description = $"Korisničko ime '{userName}' nije validno, može sadržavati samo slova ili brojeve."
        };

        public override IdentityError InvalidEmail(string email) => new IdentityError
        {
            Code = nameof(InvalidEmail),
            Description = $"Email '{email}' nije valjan."
        };

        public override IdentityError DuplicateUserName(string userName) => new IdentityError
        {
            Code = nameof(DuplicateUserName),
            Description = $"Korisničko ime '{userName}' je već zauzeto."
        };

        public override IdentityError DuplicateEmail(string email) => new IdentityError
        {
            Code = nameof(DuplicateEmail),
            Description = $"Email '{email}' je već zauzet."
        };

        public override IdentityError InvalidRoleName(string role) => new IdentityError
        {
            Code = nameof(InvalidRoleName),
            Description = $"Naziv uloge '{role}' nije valjan."
        };

        public override IdentityError DuplicateRoleName(string role) => new IdentityError
        {
            Code = nameof(DuplicateRoleName),
            Description = $"Naziv uloge '{role}' je već zauzet."
        };

        public override IdentityError UserAlreadyHasPassword() => new IdentityError
        {
            Code = nameof(UserAlreadyHasPassword),
            Description = "Korisnik već ima podešenu lozinku."
        };

        public override IdentityError UserLockoutNotEnabled() => new IdentityError
        {
            Code = nameof(UserLockoutNotEnabled),
            Description = "Zaključavanje nije omogućeno za ovog korisnika."
        };

        public override IdentityError UserAlreadyInRole(string role) => new IdentityError
        {
            Code = nameof(UserAlreadyInRole),
            Description = $"Korisnik je već u ulozi '{role}'."
        };

        public override IdentityError UserNotInRole(string role) => new IdentityError
        {
            Code = nameof(UserNotInRole),
            Description = $"Korisnik nije u ulozi '{role}'."
        };

        public override IdentityError PasswordTooShort(int length) => new IdentityError
        {
            Code = nameof(PasswordTooShort),
            Description = $"Lozinka mora imati najmanje {length} karaktera."
        };

        public override IdentityError PasswordRequiresNonAlphanumeric() => new IdentityError
        {
            Code = nameof(PasswordRequiresNonAlphanumeric),
            Description = "Lozinka mora imati najmanje jedan ne-alfanumerički karakter."
        };

        public override IdentityError PasswordRequiresDigit() => new IdentityError
        {
            Code = nameof(PasswordRequiresDigit),
            Description = "Lozinka mora imati najmanje jedan broj ('0'-'9')."
        };

        public override IdentityError PasswordRequiresLower() => new IdentityError
        {
            Code = nameof(PasswordRequiresLower),
            Description = "Lozinka mora imati najmanje jedno malo slovo ('a'-'z')."
        };

        public override IdentityError PasswordRequiresUpper() => new IdentityError
        {
            Code = nameof(PasswordRequiresUpper),
            Description = "Lozinka mora imati najmanje jedno veliko slovo ('A'-'Z')."
        };
    }
}