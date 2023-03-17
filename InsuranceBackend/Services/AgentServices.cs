using InsuranceBackend.Models;
using InsuranceBackend.Enum;

namespace InsuranceBackend.Services
{
    public class AgentServices
    {
        readonly InsuranceDbContext _context;

        public AgentServices()
        {
            _context = new InsuranceDbContext();
        }
        public Agent GetAgent(int agentID)
        {
            var res = _context.Agents.Find(agentID);
            return res ?? throw new Exception();
        }

        public void DeleteAgent(int agentID)
        {
            var dbagent = GetAgent(agentID);
            _context.Agents.Remove(dbagent);
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
        public void ChangeAgentStatus(int _agentID,StatusEnum e)
        {
            var dbagent=GetAgent(_agentID);
            if (!StatusEnum.IsDefined(typeof(StatusEnum), e))
            {
                throw new Exception();
            }
            dbagent.Status = e;
            UpdateAgent(_agentID, dbagent);
        }
        //
    }
}
