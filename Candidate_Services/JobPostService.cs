using Candidate_BusinessObjects;
using Candidate_DAO;
using Candidate_Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Candidate_Service
{
    public class JobPostService : IJobpostService
    {
        private IHRJobPost jobpost;
        public JobPostService()
        {
            jobpost = new HRJobPostRepo();
        }

        public bool Create(JobPosting jobPosting)
        {
            return jobpost.Create(jobPosting);

        }

        public bool DeleteJobPost(JobPosting jobPost)
        {
            try
            {
                using (var context = new CandidateManagementContext()) // Replace with your actual context
                {
                    var existingPost = context.JobPostings.Find(jobPost.PostingId);
                    if (existingPost != null)
                    {
                        context.JobPostings.Remove(existingPost);
                        return context.SaveChanges() > 0;
                    }
                    return false;
                }
            }
            catch (Exception)
            {
                return false;
            }
        }

        public JobPosting GetJobPosting(string id)
        {
            return jobpost.GetJobPosting(id);
        }

        public List<JobPosting> GetJobPostings()
        {
            return jobpost.GetJobPostings();
        }

        public List<JobPosting> searchJobPostingByJobPostingTitle(string jobPostingTitle)
        {
            return jobpost.searchJobPostingByJobPostingTitle(jobPostingTitle);
        }

        public bool UpdateJobpost(JobPosting jobPosting)
        {
            return jobpost.UpdateJobpost(jobPosting);
        }


    }
}
