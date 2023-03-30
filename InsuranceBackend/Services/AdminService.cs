using InsuranceBackend.Models;

namespace InsuranceBackend.Services
{
    public class AdminService
    {
        InsuranceDbContext _context;
        public AdminService()
        {
            _context = new InsuranceDbContext();
        }
        public IEnumerable<Agent> GetAllAgent()
        {
            return _context.Agents.ToList();
        }
        public IEnumerable<Company> GetAllCompanies()
        {
            return _context.Companies.ToList();
        }
        public IEnumerable<Client> GetAllClient()
        {
            return _context.Clients.ToList();
        }

        public PolicyType AddPolicytype(PolicyType policyType)
        {
            policyType.PolicytypeId = 0;
            _context.PolicyTypes.Add(policyType);
            _context.SaveChanges();
            return policyType;
        }

        //Approval
        public void ChangeUserStatus(User user)
        {   
            _context.Users.Update(user);
            _context.SaveChanges();
        }

        public IEnumerable<Policy> GetAllPolicies()
        {
            return _context.Policies.ToList();
        }


        public void ChangePolicyStatus(Policy policy)
        {
            _context.Policies.Update(policy);
            _context.SaveChanges();
        }
    }
}
