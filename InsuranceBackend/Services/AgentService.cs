using InsuranceBackend.Enum;
using InsuranceBackend.Models;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using InsuranceBackend.Database;

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
                var con = new SqlConnection(DBConnection.ConnectionString);
                con.Open();
                var cmd = new SqlCommand(
                    "INSERT INTO Agents(userID,agentName,gender,phoneNum,dob,email,address,grade,profilePic,status) VALUES('"
                        + agent.UserId
                        + "','"
                        + agent.AgentName
                        + "','"
                        + agent.Gender
                        + "','"
                        + agent.PhoneNum
                        + "','dob','"
                        + agent.Email
                        + "','Address','1','"
                        + agent.ProfilePic
                        + "',0)",
                    con
                );
                cmd.ExecuteNonQuery();
                con.Close();
            }
            catch (Exception)
            {
                throw new Exception();
            }
            return GetAgentByName(agent.AgentName);
        }

        public Agent GetAgent(int agentID)
        {
            var res = _context.Agents.FirstOrDefault(a => a.AgentId == agentID);
            return res ?? throw new Exception();
        }

        public Agent GetAgentByName(string agentName)
        {
            var res = _context.Agents.FirstOrDefault(a => a.AgentName == agentName);
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
        public void ChangeAgentStatus(int _agentID, ActorStatusEnum e)
        {
            var dbagent = GetAgent(_agentID);
            if (!StatusEnum.IsDefined(typeof(StatusEnum), e))
            {
                throw new Exception();
            }
            dbagent.Status = e;
            UpdateAgent(_agentID, dbagent);
        }

        public void ChangeClientPolicyStatus(int clientpolicyID, StatusEnum e)
        {
            var dbclientpolicy = _context.ClientPolicies.Find(clientpolicyID);
            if (!StatusEnum.IsDefined(typeof(StatusEnum), e))
            {
                throw new Exception();
            }
            if (dbclientpolicy == null)
            {
                throw new NullReferenceException(null);
            }
            dbclientpolicy.Status = (ClientPolicyStatusEnum)e;
            _context.ClientPolicies.Update(dbclientpolicy);
            _context.SaveChanges();
        }

        public ClientDeath AddClientDeath(ClientDeath clientDeath)
        {
            var dbcd = _context.ClientDeaths.FirstOrDefault(
                cd => cd.ClientPolicyId == clientDeath.ClientPolicyId
            );
            if (dbcd != null)
            {
                return dbcd;
            }
            ValidateClientPolicy(clientDeath.ClientPolicyId);
            _context.ClientDeaths.Add(clientDeath);
            ClientPolicy clientPolicy = _context.ClientPolicies.First(
                p => p.ClientPolicyId == clientDeath.ClientPolicyId
            );
            if (clientPolicy != null)
            {
                clientPolicy.Status = ClientPolicyStatusEnum.Mature;
                _context.ClientPolicies.Update(clientPolicy);
            }
            _context.SaveChanges();
            return _context.ClientDeaths.OrderBy(d => d.ClientDeathId).Last();
        }

        public Maturity AddMaturity(Maturity maturity)
        {
            var dbm = _context.Maturities.FirstOrDefault(
                m => m.ClientPolicyId == maturity.ClientPolicyId
            );
            if (dbm != null)
            {
                return dbm;
            }
            ValidateClientPolicy(maturity.ClientPolicyId);
            _context.Maturities.Add(maturity);
            ClientPolicy? clientPolicy = _context.ClientPolicies.FirstOrDefault(
                p => p.ClientPolicyId == maturity.ClientPolicyId
            );
            if (clientPolicy != null)
            {
                clientPolicy.Status = ClientPolicyStatusEnum.Mature;
                _context.ClientPolicies.Update(clientPolicy);
            }
            _context.SaveChanges();
            return _context.Maturities.OrderBy(m => m.MaturityId).Last();
        }

        public Premium AddPenalty(Premium premium)
        {
            ValidateClientPolicy(premium.ClientPolicyId);
            _context.Premia.Add(premium);
            ClientPolicy? clientPolicy = _context.ClientPolicies.FirstOrDefault(
                p => p.ClientPolicyId == premium.ClientPolicyId
            );
            if (clientPolicy != null)
                premium.Status = PenaltyStatusEnum.Pending;
            _context.SaveChanges();
            return _context.Premia.OrderBy(p => p.PremiumId).Last();
        }

        public IEnumerable<Policy> ViewPolicies(int agentID)
        {
            ValidateAgent(agentID);
            List<AgentCompany> dbagentcompany = _context.AgentCompanies
                .Include(e => e.AgentId == agentID)
                .ToList();
            List<int> companiesIDs = dbagentcompany.Select(e => e.CompanyId).ToList();
            List<Policy> policies = new();
            foreach (var compID in companiesIDs)
            {
                policies.Add((Policy)_context.Policies.Include(c => c.CompanyId == compID));
            }
            return policies;
        }

        public void RequestCompany(int companyID, int agentID)
        {
            _context.AgentCompanies.Add(
                new AgentCompany { AgentId = agentID, CompanyId = companyID }
            );
            _context.SaveChanges();
        }

        public List<int> GetClientsbyAgent(int agentID)
        {
            return _ = _context.ClientPolicies
                .Include(c => c.AgentId == agentID)
                .Select(c => c.Client.ClientId)
                .ToList();
        }

        //Validators

        private bool ValidateAgent(int agentID)
        {
            return _context.Agents.FirstOrDefault(a => a.AgentId == agentID) != null;
        }

        private bool ValidateClientPolicy(int? clientPolicyId)
        {
            return _context.ClientPolicies.FirstOrDefault(cp => cp.ClientPolicyId == clientPolicyId)
                != null;
        }
    }
}
