namespace JAS.Models.Domain.CompositeModel
{
    public class SearchComposite
    {
        public List<JobListing> JobListing { get; set; }
        public string SearchTerm { get; set; }
    }
}
