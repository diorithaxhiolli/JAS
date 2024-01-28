using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace JAS.Models.Domain
{
    public class Status
    {
        [Required]
        [Key]
        public int statusId {  get; set; }

        [Required]
        [StringLength(100)]
        public string statusName { get; set; }

        public virtual ICollection<Application> Application { get; set; }
    }
}
