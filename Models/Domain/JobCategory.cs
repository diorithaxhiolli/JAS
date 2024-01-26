using System.ComponentModel.DataAnnotations;

namespace JAS.Models.Domain
{
    public class JobCategory
    {
        [Key]
        [Required]
        public int categoryId { get; set; }

        [Required]
        [StringLength(100)]
        public string name { get; set; }

        [Required]
        [StringLength(1500)]
        public string description { get; set; }

        public virtual ICollection<JobListing> JobListing { get; set; }
    }
}
