using InsuranceBackend.Enum;
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
            ChangeActorStatus(user.UserId, user.Type, user.Status);
            _context.SaveChanges();
        }

        public void ChangeActorStatus(int userId, UserTypeEnum type, StatusEnum status)
        {
            switch (type)
            {
                case UserTypeEnum.Company:
                {
                    var dbcompany = _context.Companies.First(c => c.UserId == userId);
                    if (dbcompany != null)
                    {
                        dbcompany.Status = (ActorStatusEnum)status;
                        _context.Companies.Update(dbcompany);
                        _context.SaveChanges();
                    }
                    break;
                }
                case UserTypeEnum.Agent:
                {
                    var dbagent = _context.Agents.First(c => c.UserId == userId);
                    if (dbagent != null)
                    {
                        dbagent.Status = (ActorStatusEnum)status;
                        _context.Agents.Update(dbagent);
                        _context.SaveChanges();
                    }
                    break;
                }
                case UserTypeEnum.Client:
                {
                    var dbclient = _context.Clients.First(c => c.UserId == userId);
                    if (dbclient != null)
                    {
                        dbclient.Status = (ActorStatusEnum)status;
                        _context.Clients.Update(dbclient);
                        _context.SaveChanges();
                    }
                    break;
                }
                default:
                    break;
            }
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
