using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace JAS.Models.Domain
{
    public class Education
    {
        [Key]
        [Required]
        public int educationId { get; set; }

        [Required]
        [StringLength(100)]
        public string institution {  get; set; }

        [Required]
        [StringLength(100)]
        public string degree { get; set; }

        [Required]
        [StringLength(100)]
        public string field { get; set; }

        [Required]
        public DateTime graduationDate { get; set; }

        [Required]
        public int cvId { get; set; }

        [ForeignKey(nameof(cvId))]
        public virtual CV CV { get; set; }
        
    }
}
