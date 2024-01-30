namespace JAS.Models.Domain.CompositeModel
{
    public class JobCategoryComposite
    {
        public JobCategory? JobCategory { get; set; }

        public List<JobCategory> JobCategoryList { get; set; }
    }
}
