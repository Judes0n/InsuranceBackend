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
            switch(user.Type)
            {
                case Enum.UserTypeEnum.Company:
                    {
                        var dbcompany = _context.Companies.FirstOrDefault(c => c.UserId == user.UserId);
                        if (dbcompany != null)
                        {
                            dbcompany.Status = (Enum.ActorStatusEnum)user.Status;
                            _context.Companies.Update(dbcompany);
                        }
                        break;
                    }
                case Enum.UserTypeEnum.Agent:
                    {
                        var dbagent = _context.Agents.FirstOrDefault(c => c.UserId == user.UserId);
                        if (dbagent != null)
                        {
                            dbagent.Status = (Enum.ActorStatusEnum)(user.Status);
                            _context.Agents.Update(dbagent);
                        }
                        break;
                    }
                case Enum.UserTypeEnum.Client:
                    {
                        var dbclient = _context.Clients.FirstOrDefault(c => c.UserId == user.UserId);
                        if(dbclient != null)
                        {
                            dbclient.Status = (Enum.ActorStatusEnum)(user.Status);
                            _context.Clients.Update(dbclient);
                        }
                        break;
                    }
                default:
                    break;
            }
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
