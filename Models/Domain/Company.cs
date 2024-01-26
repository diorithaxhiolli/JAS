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

        [ForeignKey(nameof(companyId))]
        public virtual JASUser User { get; set; }

        public virtual ICollection<JobListing> JobListing { get; set; }

        public virtual ICollection<City> City { get; set; }
    }
}