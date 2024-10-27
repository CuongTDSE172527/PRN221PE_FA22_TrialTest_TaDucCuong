using Candidate_BusinessObjects;
using System.ComponentModel;
using System.Linq.Expressions;

namespace Candidate_DAO
{
    public class HRJobPostingDAO
    {
        private CandidateManagementContext context;
        private static HRJobPostingDAO instance;
        public HRJobPostingDAO()
        {


            context = new CandidateManagementContext();
        }

        public static HRJobPostingDAO Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new HRJobPostingDAO();

                }
                return instance;
            }
        }

        public List<JobPosting> GetJobPostings()
        {
            return context.JobPostings.ToList();
        }
        public JobPosting GetJobPosting(string id)
        {
            return context.JobPostings.SingleOrDefault(x => x.PostingId == id);
        }
        public bool Create(JobPosting jobPosting)
        {
            bool issucced = false;
            try
            {
                var post = GetJobPosting(jobPosting.PostingId);
                if (post == null)
                {
                    context.JobPostings.Add(jobPosting);
                    context.SaveChanges();
                    issucced = true;
                }
            }
            catch (Exception ex)
            {
                //Log
            }
            return issucced;

        }
        public bool Deletejobp(JobPosting jobPosting)
        {
            bool issucced = false;
            try
            {
                var post = GetJobPosting(jobPosting.PostingId);
                if (post != null)
                {
                    context.JobPostings.Remove(post);
                    context.SaveChanges();
                    issucced = true;
                }
            }
            catch (Exception ex)
            {
                //Log
            }
            return issucced;

        }
        public bool Updatejobp(JobPosting jobPosting)
        {
            bool isSucceeded = false;
            try
            {
                var post = GetJobPosting(jobPosting.PostingId); // Retrieve existing job posting by ID
                if (post != null)
                {

                    post.JobPostingTitle = jobPosting.JobPostingTitle;
                    post.Description = jobPosting.Description;
                    post.PostedDate = jobPosting.PostedDate;


                    context.Entry(post).State = Microsoft.EntityFrameworkCore.EntityState.Modified;
                    context.SaveChanges();
                    isSucceeded = true;
                }
            }
            catch (Exception ex)
            {

            }
            return isSucceeded;
        }
        public List<JobPosting> searchJobPostingByJobPostingTitle(string jobPostingTitle)
        {
            List<JobPosting> listJob = null;
            try
            {
                listJob = context.JobPostings.Where(x => x.JobPostingTitle.ToUpper().Contains(jobPostingTitle.ToUpper())).ToList();
            }
            catch (Exception ex)
            {
            }

            return listJob;
        }
    }
}

