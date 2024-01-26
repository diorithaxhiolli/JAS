using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace JAS.Models.Domain
{
    public class Application
    {
        [Key]
        [Required]
        public int applicationId { get; set; }

        [Required]
        public int positionId { get; set; }

        [Required]
        public string jobSeekerId { get; set; }

        [ForeignKey(nameof(jobSeekerId))]
        public virtual JobSeeker JobSeeker { get; set; }


        [ForeignKey(nameof(positionId))]
        public virtual JobListing JobListing { get; set; }


        public virtual ICollection<Status> Status { get; set; }

        public virtual ICollection<CoverLetter> CoverLetter { get; set; }

        public virtual ICollection<CV> CV { get; set; }
    }
}
