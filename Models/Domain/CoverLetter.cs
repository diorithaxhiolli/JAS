using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace JAS.Models.Domain
{
    public class CoverLetter
    {
        [Required]
        [Key]
        public int coverLetterId { get; set; }

        public string filePath { get; set; }

        public virtual ICollection<Application> Application { get; set; }
    }
}
