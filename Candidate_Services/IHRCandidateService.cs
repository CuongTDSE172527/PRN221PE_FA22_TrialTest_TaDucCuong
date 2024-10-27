using Candidate_BusinessObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Candidate_Service
{
    public interface IHRCandidateService
    {
        public List<CandidateProfile> GetCandidateProfiles();
        public CandidateProfile GetJobCandidate(string id);
        public bool Create(CandidateProfile can);
        public bool DeleteCandidate(CandidateProfile can);
        public bool UpdateCandidate(CandidateProfile can);
    }
}
