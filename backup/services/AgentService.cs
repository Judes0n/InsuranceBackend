using InsuranceBackend.Enum;


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
            _context.Agents.Add(agent);
            _context.SaveChangesAsync();
            return GetAgent(agent.AgentId);
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
        public void ChangeAgentStatus(int _agentID,ActorStatusEnum e)
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
