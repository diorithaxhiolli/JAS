using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace JAS.Models.Domain
{
    public class Country
    {
        [Key]
        [Required]
        public int countryId { get; set; }

        [Required]
        [StringLength(100)]
        public string countryName { get; set; }

        [Required]
        [StringLength(100)]
        public string language { get; set; }

        public virtual ICollection<City> City { get; set; }
    }
}
