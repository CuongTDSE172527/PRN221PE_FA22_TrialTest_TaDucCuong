using Candidate_BusinessObjects;
using Candidate_Repositories;

namespace Candidate_Service
{
    public class HRCandidateService: IHRCandidateService
    {
        private IHRCandidateRepo repo;
        public HRCandidateService() {
            repo = new HRCandidateRepo();
        }

        public bool Create(CandidateProfile can)
        {
            return repo.Create(can);
        }

        public bool DeleteCandidate(CandidateProfile can)
        {
            return repo.DeleteCandidate(can);
        }

        public List<CandidateProfile> GetCandidateProfiles()
        {
            return repo.GetCandidateProfiles();
        }

        public CandidateProfile GetJobCandidate(string id)
        {
            return repo.GetJobCandidate(id);
        }

        public bool UpdateCandidate(CandidateProfile can)
        {
            return repo.UpdateCandidate(can);
        }
    }
}
