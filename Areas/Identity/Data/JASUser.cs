using JAS.Models.Domain;
using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations.Schema;

namespace JAS.Areas.Identity.Data;

// Add profile data for application users by adding properties to the JASUser class
public class JASUser : IdentityUser
{
    [PersonalData]
    [Column(TypeName = "nvarchar(100)")]
    public string firstName { get; set; }

    [PersonalData]
    [Column(TypeName = "nvarchar(100)")]
    public string lastName { get; set; }

    public virtual ICollection<Company> Companies { get; set; }

    public virtual ICollection<JobSeeker> JobSeekers { get; set; }

}