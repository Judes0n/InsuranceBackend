using InsuranceBackend.Models;

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
        public IEnumerable<Agent> GetAllAgent()
        {
            return _context.Agents.ToList();
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
    }
}
