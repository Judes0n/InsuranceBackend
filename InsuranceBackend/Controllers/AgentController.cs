using InsuranceBackend.Models;
using InsuranceBackend.Services;
using InsuranceBackend.Enum;
using Microsoft.AspNetCore.Http;
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
    }
}
