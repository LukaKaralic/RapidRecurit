using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using RapidRecruit.Models.Validations;
namespace RapidRecruit.Models
{
    public class JobPosting
    {
        public int Id { get; set; }
        [Required]
        public string UserId { get; set; }
        [ForeignKey("UserId")]
        public UserAccount User { get; set; }
        public ICollection<JobApplication>? JobApplications { get; set; }
        [Required(ErrorMessage = "Naslov je obavezan")]
        public string Title { get; set; }
        [Required(ErrorMessage = "Opis je obavezan")]
        public string Description { get; set; }
        [Required(ErrorMessage = "Minimalna plata je obavezna")]
        [Display(Name = "Minimum Salary")]
        [Range(0, int.MaxValue, ErrorMessage = "Minimalna plata mora biti pozitivan broj")]
        public int MinimumSalary { get; set; }
        [Required(ErrorMessage = "Maksimalna plata je obavezna")]
        [Display(Name = "Maximum Salary")]
        [SalaryValidation(ErrorMessage = "Maksimalna plata mora biti veća od minimalne plate")]
        public int MaximumSalary { get; set; }
        [Required(ErrorMessage = "Lokacija je obavezna")]
        public string Location { get; set; }
        [Required(ErrorMessage = "Datum isteka je obavezan")]
        [Display(Name = "End Date")]
        [DataType(DataType.Date)]
        [FutureDate(ErrorMessage = "Datum isteka mora biti najmanje sutra")]
        public DateTime EndDate { get; set; }
        [Display(Name = "Posted Date")]
        public DateTime CreatedAt { get; set; } = DateTime.Now;
        [Display(Name = "Last Updated")]
        public DateTime UpdatedAt { get; set; } = DateTime.Now;
    }
}