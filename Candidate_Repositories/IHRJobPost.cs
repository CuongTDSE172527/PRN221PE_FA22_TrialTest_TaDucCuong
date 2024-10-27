using Candidate_BusinessObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Candidate_Repositories
{
    public interface IHRJobPost
    {
        public List<JobPosting> GetJobPostings();
        public JobPosting GetJobPosting(string id);
        public bool Create(JobPosting jobPosting);
        public bool DeleteJobPost(JobPosting jobPosting);
        public bool UpdateJobpost(JobPosting jobPosting);
        public List<JobPosting> searchJobPostingByJobPostingTitle(string jobPostingTitle);
    }
}
