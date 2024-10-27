using Candidate_BusinessObjects;
using Candidate_DAO;


namespace Candidate_Repositories
{
    public class HrAccountRepo : IHRAccountRepo
    {

        public HrAccountRepo()
        {

        }
        public Hraccount GetHraccountByEmail(string email) => HRAccountDAO.Instance.GetHraccountByEmail(email);

     
        public List<Hraccount> GetHraccounts() => HRAccountDAO.Instance.GetHraccounts();

    }
}
