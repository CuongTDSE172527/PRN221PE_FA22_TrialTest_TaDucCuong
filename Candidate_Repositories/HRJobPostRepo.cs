using Candidate_BusinessObjects;
using Candidate_DAO;


namespace Candidate_Repositories
{
    public class HRJobPostRepo : IHRJobPost
    {
        public HRJobPostRepo() { }

        public bool Create(JobPosting jobPosting) => HRJobPostingDAO.Instance.Create(jobPosting);

        public bool DeleteJobPost(JobPosting jobPosting) => HRJobPostingDAO.Instance.Deletejobp(jobPosting);

        public JobPosting GetJobPosting(string id) => HRJobPostingDAO.Instance.GetJobPosting(id);

        public List<JobPosting> GetJobPostings() => HRJobPostingDAO.Instance.GetJobPostings();

        public List<JobPosting> searchJobPostingByJobPostingTitle(string jobPostingTitle) => HRJobPostingDAO.Instance.searchJobPostingByJobPostingTitle(jobPostingTitle);

        public bool UpdateJobpost(JobPosting jobPosting) => HRJobPostingDAO.Instance.Updatejobp(jobPosting);

    }
}
