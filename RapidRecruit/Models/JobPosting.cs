using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace RapidRecruit.Models
{
    public class JobPosting
    {
        public int Id { get; set; }

        [Required]
        public string UserId { get; set; }

        [ForeignKey("UserId")]
        public UserAccount User {  get; set; }

        [Required]
        public string Title { get; set; }

        [Required]
        public string Description { get; set; }

        [Required]
        [Display(Name = "Minimum Salary")]
        [Range(0, int.MaxValue, ErrorMessage = "Salary must be positive")]
        public int MinimumSalary { get; set; }

        [Required]
        [Display(Name = "Maximum Salary")]
        [Range(0, int.MaxValue, ErrorMessage = "Salary must be positive")]
        public int MaximumSalary { get; set; }

        [Required]
        public string Location { get; set; }

        [Required]
        [Display(Name = "End Date")]
        [DataType(DataType.Date)]
        public DateTime EndDate { get; set; }

        [Display(Name = "Posted Date")]
        public DateTime CreatedAt { get; set; } = DateTime.Now;

        [Display(Name = "Last Updated")]
        public DateTime UpdatedAt { get; set; } = DateTime.Now;
    }


}
