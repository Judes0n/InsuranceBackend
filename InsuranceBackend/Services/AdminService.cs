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

        public void AddPolicytype(PolicyType policyType)
        {
            _context.PolicyTypes.Add(policyType);
            _context.SaveChanges();
        }

        //Approval
        public void ChangeUserStatus(UserTypeEnum Actortype,int userID,ActorStatusEnum e)
        {
            var dbuser = _context.Users.FirstOrDefault(l => l.UserId == userID) ?? throw new NullReferenceException();
            if(!ActorStatusEnum.IsDefined(typeof(ActorStatusEnum), e)) throw new ArgumentException();
            if(!UserTypeEnum.IsDefined(typeof(UserTypeEnum), Actortype)) throw new ArgumentException();
            switch (Actortype)
            {
                
                case UserTypeEnum.Company:
                    {
                        var dbcompany = _context.Companies.FirstOrDefault(c=>c.UserId == userID) ?? throw new NullReferenceException();
                        dbcompany.Status = e;
                        _context.Companies.Update(dbcompany);
                        break;
                    }
                case UserTypeEnum.Agent:
                    {
                        var dbagent = _context.Agents.FirstOrDefault(a=>a.UserId ==  userID) ?? throw new NullReferenceException();
                        dbagent.Status = e;
                        _context.Agents.Update(dbagent);
                        break;
                    }
                case UserTypeEnum.Client:
                    {
                        var dbclient = _context.Clients.FirstOrDefault(c=>c.UserId==userID) ?? throw new NullReferenceException();  
                        dbclient.Status = e;
                        _context.Clients.Update(dbclient);
                        break;
                    }
            }
            _context.SaveChanges();
        }
    }
}
