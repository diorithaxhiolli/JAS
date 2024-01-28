using Microsoft.AspNetCore.Identity;

namespace JAS.Models.Domain.CompositeModel
{
    public class StatusComposite
    {
        public Status? Status { get; set; }

        public List<Status> StatusList { get; set; }
    }
}
