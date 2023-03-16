using InsuranceBackend.Models;
using InsuranceBackend.Enum;

namespace InsuranceBackend.Services
{
    public class AgentServices
    {
        InsuranceDbContext _context;

        public AgentServices()
        {
            _context = new InsuranceDbContext();
        }
        public Agent GetAgent(int agentID)
        {
            var res = _context.Agents.Find(agentID);
            return res == null ? throw new Exception() : res;
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
        public void ApproveAgent(int _agentID)
        {
            var dbagent=GetAgent(_agentID);
            dbagent.Status=StatusEnum.Active;
            UpdateAgent(_agentID, dbagent);
        }
        public void RejectAgent(int _agentID)
        {
            var dbagent=GetAgent(_agentID);
            dbagent.Status = StatusEnum.Inactive; 
            UpdateAgent(_agentID, dbagent);
        }
        public void BlockAgent(int _agentID)
        {
            var dbagent=GetAgent(_agentID);
            dbagent.Status = StatusEnum.Blocked;
            UpdateAgent(_agentID, dbagent);
        }
        //
    }
}
