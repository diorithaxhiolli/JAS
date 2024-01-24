using JAS.Areas.Identity.Data;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace JAS.Models.Domain
{
    public class JobSeeker
    {
        [Key, ForeignKey("User")]
        public string jobSeekerId { get; set; }

        public virtual JASUser User { get; set; }
    }
}