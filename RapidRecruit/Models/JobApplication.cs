using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace RapidRecruit.Models
{
    public enum Descision
    {
        Up,
        Down
    }
    public class JobApplication
    {
        public int Id { get; set; }

        [Required]
        public string UserId { get; set; }

        [ForeignKey("UserId")]
        public UserAccount User { get; set; }

        [Required]
        public int JobPostingId { get; set; }

        [ForeignKey("JobPostingId")]
        public JobPosting JobPosting { get; set; }

        public string? CandidateNote { get; set; }
        public string? ReviewerNote { get; set; }
        public Descision? ReviewDescision { get; set; }

        [Required]
        public string ResumeFileName { get; set; }

        [Required]
        public string ResumeFilePath { get; set; }

        public virtual Conversation Conversation { get; set; }

        [Display(Name = "Submited At")]
        public DateTime CreatedAt { get; set; } = DateTime.Now;

        [Display(Name = "Last Updated")]
        public DateTime UpdatedAt { get; set; } = DateTime.Now;
    }
}
