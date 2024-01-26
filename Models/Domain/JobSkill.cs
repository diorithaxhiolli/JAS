using System.ComponentModel.DataAnnotations;

namespace JAS.Models.Domain
{
    public class JobSkill
    {
        [Key]
        [Required]
        public int skillId { get; set; }

        [Required]
        [StringLength(150)]
        public int skillName { get; set; }

        public virtual ICollection<RequiredSkills> RequiredSkills { get; set; }

        public virtual ICollection<CV> CV { get; set; }
    }
}
