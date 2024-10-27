using Candidate_BusinessObjects;
using Candidate_DAO;


namespace Candidate_Repositories
{
    public class HRCandidateRepo : IHRCandidateRepo
    {
        public bool Create(CandidateProfile can) => HRCandidateDAO.Instance.Create(can);

        public bool DeleteCandidate(CandidateProfile can) => HRCandidateDAO.Instance.DeleteCandidate(can);

        public List<CandidateProfile> GetCandidateProfiles() => HRCandidateDAO.Instance.GetCandidateProfiles();

        public CandidateProfile GetJobCandidate(string id) => HRCandidateDAO.Instance.GetcandidateByid(id);

        public bool UpdateCandidate(CandidateProfile can) => HRCandidateDAO.Instance.UpdateCandidate(can);

    }
}
