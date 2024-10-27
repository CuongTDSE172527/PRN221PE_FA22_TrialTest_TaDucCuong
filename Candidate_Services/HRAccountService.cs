using Candidate_BusinessObjects;
using Candidate_DAO;
using Candidate_Repositories;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Candidate_Service
{
    public class HRAccountService : IHRAccountService
    {
        private IHRAccountRepo iAccountRepo;
        
        public HRAccountService()
        {
            iAccountRepo = new HrAccountRepo();
        }

        public async Task<(bool isAuthenticated, int userRole)> AuthenticateAsync(string email, string password)
        {
            try
            {
                // Replace this with your actual database query
                using (var context = new CandidateManagementContext())
                {
                    var user = await context.Hraccounts
                        .FirstOrDefaultAsync(u =>
                            u.Email == email &&
                            u.Password == password); // Note: Use proper password hashing in production

                    if (user != null && user.MemberRole.HasValue)
                    {
                        return (true, user.MemberRole.Value);
                    }
                }
                return (false, 0);
            }
            catch (Exception ex)
            {
                // Handle exception appropriately
                throw new Exception("Authentication failed", ex);
            }
        }

        public Hraccount GetHraccountByEmail(string email)
        {
            return iAccountRepo.GetHraccountByEmail(email);
        }

        public List<Hraccount> GetHraccounts()
        {
            return iAccountRepo.GetHraccounts();
        }
    }
}
