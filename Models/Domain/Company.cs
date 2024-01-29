using JAS.Areas.Identity.Data;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace JAS.Models.Domain
{
    public class Company
    {
        [Key]
        public string companyId { get; set; }

        [Required]
        [StringLength(100)]
        public string name { get; set; }

        [Required]
        [StringLength(1500)]
        public string description { get; set; }

        [Required]
        [StringLength(1500)]
        public string imagePath { get; set; }

        [Required]
        public int cityId { get; set; }

        [ForeignKey(nameof(companyId))]
        public virtual JASUser User { get; set; }

        [ForeignKey(nameof(cityId))]
        public virtual City City { get; set; }

        public virtual ICollection<JobListing> JobListing { get; set; }
    }
}