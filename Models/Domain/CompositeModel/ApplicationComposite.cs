using JAS.Areas.Identity.Data;

namespace JAS.Models.Domain.CompositeModel
{
    public class ApplicationComposite
    {
        public JobListing? JobListing { get; set; }

        public JASUser? JASUser { get; set; }

        public CV? CV { get; set; }

        public Status? Status { get; set; }

        public CoverLetter? CoverLetter { get; set; }
    }
}
