using InsuranceBackend.Enum;
using InsuranceBackend.Models;
using InsuranceBackend.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Insurance.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AgentController : ControllerBase
    {
        AgentService _agentServices;
        UserService _userService;
        InsuranceDbContext _dbContext;

        public AgentController()
        {
            _agentServices = new AgentService();
            _userService = new UserService();
            _dbContext = new InsuranceDbContext();
        }

        [HttpGet]
        [Route("GetPolicyTerms")]

        public IActionResult GetTerms(int policytermId)
        {
            var res = _dbContext.PolicyTerms.FirstOrDefault(pt => pt.PolicyTermId == policytermId);
            return Ok(res);
        }

        [HttpGet]
        [Route("GetPolicies")]

        public IEnumerable<Policy> Getpolicies(int companyId)
        {
            var res = _dbContext.Policies.Include(a=>a.CompanyId == companyId).ToList();
            return res;
        }

        [HttpGet]
        [Route("GetClientPolicies")]

        public IActionResult GetClientPolicies(int agentId)
        {
            var res = _dbContext.ClientPolicies.Where(cp=>cp.AgentId == agentId).ToList();
            return Ok(res);
        }

        [HttpGet]
        [Route("GetAgent")]

        public IActionResult GetAgent(int userId)
        {
            var res = _dbContext.Agents.FirstOrDefault(a=>a.UserId == userId);
            return Ok(res);
        }

        [HttpGet]
        [Route("GetPolicy")]

        public IActionResult GetPolicy(int policyId) 
        { 
            var res = _dbContext.Policies.FirstOrDefault(p=>p.PolicyId == policyId);
            return Ok(res);
        }

        [HttpGet]
        [Route("GetAgentsById")]

        public IEnumerable<Agent> GetAgentsByCompany(int companyId)
        {
            var agents = _dbContext.AgentCompanies.Where(ac=>ac.CompanyId == companyId).Select(a=>a.AgentId).ToList();
            List<Agent> result = new List<Agent>();
            foreach (var agentid in agents)
            {
                result.Add(_dbContext.Agents.FirstOrDefault(a => a.AgentId == agentid));
            }
            return result;
        }

        [HttpGet]
        [Route("GetCompanies")]

        public IEnumerable<Company> GetCompaniesby(int agentId) 
        { 
            var companies = _dbContext.AgentCompanies.Where(ac=>ac.AgentId == agentId).Select(a=>a.CompanyId).ToList();
            List<Company> result = new();
            foreach(var companyid in companies)
            {
                result.Add(_dbContext.Companies.FirstOrDefault(c => c.CompanyId == companyid));
            }
            return result;  
        }


        [HttpPost]
        [Route("ApplyCompany")]

        public IActionResult Apply()
        {
            AgentCompany agentCompany = new();
            agentCompany.CompanyId = int.Parse(Request.Form["companyId"]);
            agentCompany.AgentId = int.Parse(Request.Form["agentId"]);
            var logagentCompany = _dbContext.AgentCompanies.FirstOrDefault(a => a.AgentId == agentCompany.AgentId);
            if (logagentCompany != null)
            {
                if (logagentCompany.CompanyId == agentCompany.CompanyId)
                {
                    return BadRequest();

                }

            }
            else
            {
                agentCompany.Status = StatusEnum.Inactive;
                _dbContext.AgentCompanies.Add(agentCompany);
            }
            _dbContext.SaveChanges();
            return Ok();
        }
    }
}
