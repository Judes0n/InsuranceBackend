using InsuranceBackend.Enum;
using InsuranceBackend.Models;
using InsuranceBackend.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace InsuranceBackend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ReportController : ControllerBase
    {
        private readonly InsuranceDbContext _dbContext;

        public ReportController()
        {
            _dbContext = new InsuranceDbContext();
        }

        [HttpGet]
        [Route("Client-Payments")]
        public IEnumerable<Payment> GetPayments(int clientId)
        {
            var clientpolicies = _dbContext.ClientPolicies
                .Where(cp => cp.ClientId == clientId)
                .ToList();
            var payments = new List<Payment>();
            foreach (var clientpolicy in clientpolicies)
            {
                var a = _dbContext.Payments
                    .Where(cp => cp.ClientPolicyId == clientpolicy.ClientPolicyId)
                    .ToList();
                foreach (var b in a)
                    payments.Add(b);
            }
            return payments;
        }

        [HttpGet]
        [Route("Agent-ClientPolicies")]
        public IEnumerable<ClientPolicy> GetClientPolices(int agentId)
        {
            var clientpolicies = _dbContext.ClientPolicies
                .Where(cp => cp.AgentId == agentId)
                .ToList();
            return clientpolicies;
        }

        [HttpGet]
        [Route("Agent-Payments")]
        public IEnumerable<Payment> GetPayment(int agentId)
        {
            var clientpolicies = _dbContext.ClientPolicies
                .Where(cp => cp.AgentId == agentId)
                .ToList();
            var payments = new List<Payment>();
            foreach (var clientpolicy in clientpolicies)
            {
                var a = _dbContext.Payments
                    .Where(cp => cp.ClientPolicyId == clientpolicy.ClientPolicyId)
                    .ToList();
                foreach (var b in a)
                    payments.Add(b);
            }
            return payments;
        }

        [HttpGet]
        [Route("Company-Policies")]
        public IEnumerable<Policy> GetCompanyPolicies(int companyId)
        {
            return _dbContext.Policies.Where(p => p.CompanyId == companyId).ToList();
        }

        [HttpGet]
        [Route("Company-Agents")]
        public IEnumerable<Agent> GetAgents(int companyId)
        {
            var agents = _dbContext.AgentCompanies.Where(ac => ac.CompanyId == companyId).ToList();
            var Agents = new List<Agent>();
            foreach (var agent in agents)
            {
                Agents.Add(_dbContext.Agents.First(a => a.AgentId == agent.AgentId));
            }
            return Agents;
        }

        [HttpGet]
        [Route("Company-ClientPolicies")]
        public IEnumerable<ClientPolicy> GetClientPolicies(int companyId)
        {
            var policies = _dbContext.Policies.Where(p => p.CompanyId == companyId).ToList();
            var policyterms = new List<PolicyTerm>();
            foreach (var policy in policies)
            {
                policyterms.Add(_dbContext.PolicyTerms.First(pt => pt.PolicyId == policy.PolicyId));
            }
            var clientpolicies = new List<ClientPolicy>();
            foreach (var pt in policyterms)
            {
                var c = _dbContext.ClientPolicies.FirstOrDefault(
                    cp => cp.PolicyTermId == pt.PolicyTermId
                );
                if (c != null)
                {
                    clientpolicies.Add(c);
                }
            }
            return clientpolicies;
        }

        [HttpGet]
        [Route("Admin-Policies")]
        public IEnumerable<Policy> GetPolicies()
        {
            return _dbContext.Policies.ToList();
        }

        [HttpGet]
        [Route("Admin-Users")]
        public IEnumerable<User> GetUsers()
        {
            var allusers = _dbContext.Users.ToList();
            var users = new List<User>();
            foreach (var user in allusers)
            {
                if (user.Type != Enum.UserTypeEnum.Admin)
                    users.Add(user);
            }
            return users;
        }

        [HttpGet]
        [Route("Admin-Actors")]
        public IActionResult GetActors(UserTypeEnum userType)
        {
            switch (userType)
            {
                case UserTypeEnum.Company:
                {
                    return Ok(_dbContext.Companies.ToList());
                }
                case UserTypeEnum.Agent:
                {
                    return Ok(_dbContext.Agents.ToList());
                }
                case UserTypeEnum.Client:
                {
                    return Ok(_dbContext.Clients.ToList());
                }
                default:
                {
                    return Ok(new User() { UserId = 0 });
                }
            }
        }

        [HttpGet]
        [Route("Admin-Maturities")]
        public IEnumerable<Maturity> GetMaturities()
        {
            return _dbContext.Maturities.ToList();
        }

        [HttpGet]
        [Route("Admin-ClientDeaths")]
        public IEnumerable<ClientDeath> GetClientDeaths()
        {
            return _dbContext.ClientDeaths.ToList();
        }

        [HttpGet]
        [Route("Admin-Feedbacks")]
        public IEnumerable<Feedback> GetFeeds()
        {
            return _dbContext.Feedbacks.ToList();
        }
    }
}
