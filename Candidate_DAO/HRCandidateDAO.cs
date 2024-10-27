using Candidate_BusinessObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Candidate_DAO
{
    public class HRCandidateDAO
    {
        private CandidateManagementContext context;
        private static HRCandidateDAO instance;
        public HRCandidateDAO()
        {


            context = new CandidateManagementContext();
        }

        public static HRCandidateDAO Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new HRCandidateDAO();

                }
                return instance;
            }
        }

        public List<CandidateProfile> GetCandidateProfiles()
        {
            return context.CandidateProfiles.ToList();
        }
        public CandidateProfile GetcandidateByid(string id)
        {
            return context.CandidateProfiles.SingleOrDefault(x => x.CandidateId == id);
        }
        public bool Create(CandidateProfile can)
        {
            bool issucced = false;
            try
            {
                var post = GetcandidateByid(can.CandidateId);
                if (post == null)
                {
                    context.CandidateProfiles.Add(can);
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
        public bool DeleteCandidate(CandidateProfile can)
        {
            bool issucced = false;
            try
            {
                var post = GetcandidateByid(can.CandidateId);
                if (post != null)
                {
                    context.CandidateProfiles.Remove(post);
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
        public bool UpdateCandidate(CandidateProfile can)
        {
            bool isSucceeded = false;
            try
            {
                var post = GetcandidateByid(can.CandidateId); // Retrieve existing job posting by ID
                if (post != null)
                {

                    post.Fullname = can.Fullname;
                    post.ProfileUrl = can.ProfileUrl;
                    post.Birthday = can.Birthday;
                    post.ProfileShortDescription = can.ProfileShortDescription;
                    post.PostingId = can.PostingId;

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


    }
}
