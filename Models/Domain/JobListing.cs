using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace JAS.Models.Domain
{
    public class JobListing
    {
        [Key]
        [Required]
        public int positionId { get; set; }

        [Required]
        [StringLength(150)]
        public string title { get; set; }

        [Required]
        public int categoryId { get; set; }

        [Required]
        public int salary { get; set; }

        [Required]
        [StringLength(1500)]
        public string description { get; set; }

        [Required]
        public string companyId { get; set; }

        [ForeignKey(nameof(categoryId))]
        public virtual JobCategory JobCategory { get; set; }

        [ForeignKey(nameof(companyId))]
        public virtual Company Company { get; set; }


        public virtual ICollection<Application> Application { get; set; }

        public virtual ICollection<RequiredSkills> RequiredSkills { get; set; }
    }
}
