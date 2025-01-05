using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using RapidRecruit.Models.Validations;

namespace RapidRecruit.Models
{
    public class Conversation
    {
        public int Id { get; set; }

        public ICollection<Message>? Messages { get; set; }

        [Required]
        public string CandidateId { get; set; }
        [ForeignKey("CandidateId")]
        public UserAccount Candidate { get; set; }

        [Required]
        public string HiringManagerId { get; set; }
        [ForeignKey("HiringManagerId")]
        public UserAccount HiringManager { get; set; }

        [Required]
        public int JobApplicationId { get; set; }

        [ForeignKey("JobApplicationId")]
        public JobApplication JobApplication { get; set; }

        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
    }
}
