using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace JAS.Models.Domain
{
    public class City
    {
        [Key]
        [Required]
        public int cityId { get; set; }

        [Required]
        [StringLength(100)]
        public string cityName { get; set; }

        [Required]
        public string companyId { get; set; }

        [Required]
        public int countryId { get; set; }

        [ForeignKey(nameof(companyId))]
        public virtual Company Company { get; set; }

        [ForeignKey(nameof(countryId))]
        public virtual Country Country { get; set; }
    }
}
