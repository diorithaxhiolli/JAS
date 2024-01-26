using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace JAS.Models.Domain
{
    public class CV
    {
        [Key]
        [Required]
        public int cvId { get; set; }

        /* FUTURE USE MAYBE?
        [Required]
        [StringLength(1000)]
        public string cvName { get; set; }*/

        [Required]
        [StringLength(1000)]
        public string summary { get; set; }

        [Required]
        public string jobSeekerId { get; set; }

        [Required]
        public int applicationId { get; set; }

        [Required]
        public int skillId { get; set; }

        [ForeignKey(nameof(jobSeekerId))]
        public virtual JobSeeker JobSeeker { get; set; }

        [ForeignKey(nameof(applicationId))]
        public virtual Application Application { get; set; }

        [ForeignKey(nameof(skillId))]
        public virtual JobSkill JobSkill { get; set; }


        public virtual ICollection<Experience> Experience { get; set; }

        public virtual ICollection<Education> Education { get; set; }
    }
}
