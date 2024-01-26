using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace JAS.Models.Domain
{
    public class Experience
    {
        [Key]
        [Required]
        public int experienceId { get; set; }

        [Required]
        [StringLength(100)]
        public string title { get; set; }

        [Required]
        [StringLength(1500)]
        public string description { get; set; }

        [Required]
        public int cvId { get; set; }

        [ForeignKey(nameof(cvId))]
        public virtual CV CV { get; set; }
    }
}
