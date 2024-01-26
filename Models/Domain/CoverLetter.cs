using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace JAS.Models.Domain
{
    public class CoverLetter
    {
        [Required]
        [Key]
        public int coverLetterId { get; set; }

        //public string coverLetterPath { get; set; } ***FUTURE USE***

        [Required]
        public int applicationId { get; set; }

        [ForeignKey(nameof(applicationId))]
        public virtual Application Application { get; set; }
    }
}
