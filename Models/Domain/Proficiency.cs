using System.ComponentModel.DataAnnotations;

namespace JAS.Models.Domain
{
    public class Proficiency
    {
        [Key]
        [Required]
        public int proficiencyId { get; set; }

        [Required]
        [StringLength(150)]
        public string proficiencyName { get; set; }

        //NEEDS CONNECTION, FORGOT IN DATABASE
    }
}
