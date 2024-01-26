using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace JAS.Models.Domain
{
    public class RequiredSkills
    {
        [Key]
        [Required]
        public int requiredSkillsId { get; set; }

        [Required]
        public int skillId { get; set; }

        [Required]
        public int positionId { get; set; }

        [ForeignKey(nameof(skillId))]
        public virtual JobSkill JobSkill { get; set; }

        [ForeignKey(nameof(positionId))]
        public virtual JobListing JobListing { get; set; }

    }
}
