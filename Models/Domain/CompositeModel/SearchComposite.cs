namespace JAS.Models.Domain.CompositeModel
{
    public class SearchComposite
    {
        public JobListing JobListing { get; set; }
        public JobCategory JobCategory { get; set; }
        public string SearchTerm { get; set; }

    }
}
