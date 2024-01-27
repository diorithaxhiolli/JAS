using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace JAS.Models.Domain
{
    public class CV
    {
        [Key]
        [Required]
        public int cvId { get; set; }


        [Required]
        [StringLength(100)]
        public string name { get; set; }

        [Required]
        [StringLength(1000)]
        public string summary { get; set; }

        [Required]
        [StringLength(1000)]
        public string filePath { get; set; }

        [Required]
        public string jobSeekerId { get; set; }


        [ForeignKey(nameof(jobSeekerId))]
        public virtual JobSeeker JobSeeker { get; set; }



        public virtual ICollection<Application> Application { get; set; }
        public virtual ICollection<Experience> Experience { get; set; }

        public virtual ICollection<Education> Education { get; set; }

    }
}
