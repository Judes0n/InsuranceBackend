using InsuranceBackend.Enum;
using InsuranceBackend.Models;
using Microsoft.EntityFrameworkCore;

namespace InsuranceBackend.Services
{
    public class AgentService
    {
        readonly InsuranceDbContext _context;

        public AgentService()
        {
            _context = new InsuranceDbContext();
        }
        public Agent AddAgent(Agent agent)
        {
            try
            {
                _context.Database.ExecuteSqlRaw("SET IDENTITY_INSERT Users ON");
                _context.Agents.Add(agent);
                _context.SaveChanges();
                _context.Database.ExecuteSqlRaw("SET IDENTITY_INSERT Users OFF");
            }
            catch (Exception)
            {
                throw new Exception();
            }
            return GetAgentByName(agent.AgentName);
        }
        public Agent GetAgent(int agentID)
        {
            var res = _context.Agents.Find(agentID);
            return res ?? throw new Exception();
        }

        public Agent GetAgentByName(string agentName)
        {
            var res = _context.Agents.Find(agentName);
            return res ?? throw new Exception();
        }
        public void DeleteAgent(int agentID)
        {
            var dbagent = GetAgent(agentID);
            _context.Agents.Remove(dbagent);
            _context.SaveChanges();
        }

        public Agent UpdateAgent(int agentID, Agent agent)
        {
            if (GetAgent(agentID) == null)
            {
                throw new Exception();
            }
            _context.Agents.Update(agent);
            _context.SaveChanges();
            return GetAgent(agentID);
        }
        //approvals
        public void ChangeAgentStatus(int _agentID,ActorStatusEnum e)
        {
            var dbagent = GetAgent(_agentID);
            if (!StatusEnum.IsDefined(typeof(StatusEnum), e))
            {
                throw new Exception();
            }
            dbagent.Status = e;
            UpdateAgent(_agentID, dbagent);
        }

        public void ChangeClientPolicyStatus(int clientpolicyID,StatusEnum e)
        {
            var dbclientpolicy = _context.ClientPolicies.Find(clientpolicyID);
            if(!StatusEnum.IsDefined(typeof(StatusEnum),e))
            {
                throw new Exception();
            }
            if(dbclientpolicy == null)
            {
                throw new NullReferenceException(null);
            }
            dbclientpolicy.Status = e;
            _context.ClientPolicies.Update(dbclientpolicy);
            _context.SaveChanges();
        }

        public IEnumerable<Policy> ViewPolicies(int agentID)
        {
            ValidateAgent(agentID);
            List<AgentCompany> dbagentcompany = _context.AgentCompanies.Include(e => e.AgentId == agentID).ToList();
            List<int> companiesIDs = dbagentcompany.Select(e => e.CompanyId).ToList();
            List<Policy> policies = new();
            foreach (var compID in companiesIDs)
            {
                policies.Add((Policy)_context.Policies.Include(c => c.CompanyId == compID).ToList());
            }
            return policies;
        }
         
        public void RequestCompany(int companyID, int agentID)
        {
            _context.AgentCompanies.Add(new AgentCompany { AgentId = agentID, CompanyId = companyID });
            _context.SaveChanges();
        }

        public List<int> GetClientsbyAgent(int agentID)
        {
           return _ = _context.ClientPolicies.Include(c => c.AgentId == agentID).Select(c => c.Client.ClientId).ToList();
        }
        //Validators

        private bool ValidateAgent(int agentID)
        {
            return _context.Agents.FirstOrDefault(a => a.AgentId == agentID) != null;
        }

        private bool ValidateClient(int? clientId)
        {
            throw new NotImplementedException();
        }
    }
}
