using InsuranceBackend.Enum;
using InsuranceBackend.Models;
using InsuranceBackend.Services;
using Microsoft.AspNetCore.Mvc;

namespace InsuranceBackend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CompanyController : ControllerBase
    {
        readonly CompanyService _companyService;
        readonly UserService _userService;
        readonly InsuranceDbContext _dbContext;

        public CompanyController()
        {
            _companyService = new CompanyService();
            _userService = new UserService();
            _dbContext = new();
        }

        [HttpGet]
        [Route("GetPolicy")]
        public IActionResult GetPolicy(int policyId)
        {
            Policy policy = _companyService.GetPolicy(policyId);
            return Ok(policy);
        }

        [HttpPost]
        [Route("AddPolicy")]
        public IActionResult AddPolicy()
        {
            Policy policy = new();

            policy.PolicyId = int.Parse(Request.Form["policyId"]);
            policy.CompanyId = int.Parse(Request.Form["companyId"]);
            policy.PolicyName = Request.Form["policyName"];
            policy.PolicytypeId = int.Parse(Request.Form["policytypeId"]);
            policy.PolicyAmount = int.Parse(Request.Form["policyAmount"]);
            policy.TimePeriod = int.Parse(Request.Form["timePeriod"]);
            policy.Status = (int)StatusEnum.Inactive;

            if (policy == null)
                throw new ArgumentNullException(nameof(policy));

            _companyService.AddPolicy(policy);
            return Ok(policy);
        }

        [HttpPost]
        [Route("AddPolicyTerm")]
        public IActionResult AddPolicyTerm()
        {
            PolicyTerm policyterm =
                new()
                {
                    PolicyId = int.Parse(Request.Form["policyId"]),
                    Terms = int.Parse(Request.Form["terms"]),
                    PremiumAmount = int.Parse(Request.Form["premiumAmount"]),
                    Period = int.Parse(Request.Form["period"])
                };

            if (policyterm == null)
                throw new ArgumentNullException(nameof(policyterm));
            _companyService.AddPolicyTerm(policyterm);
            return Ok(policyterm);
        }

        [HttpGet]
        [Route("ViewPolicies")]
        public IEnumerable<Policy> ViewPolicies(int companyID)
        {
            return _companyService.ViewPolicies(companyID);
        }

        [HttpGet]
        [Route("ViewAgents")]
        public IEnumerable<AgentCompany> ViewAgents(int companyId)
        {
            return _companyService.ViewAgents(companyId);
        }

        [HttpGet]
        [Route("GetCompany")]
        public Company GetCompany(int userID)
        {
            return _companyService.GetCompany(userID);
        }

        [HttpGet]
        [Route("GetAllCompany")]
        public IEnumerable<Company> GetAll()
        {
            return _companyService.GetAllCompanies();
        }

        [HttpPost]
        [Route("ChangeAgentCompanyStatus")]
        public IActionResult Change()
        {
            int id = int.Parse(Request.Form["id"]);
            int status = int.Parse(Request.Form["status"]);
            AgentCompany agentCompany = new();
            agentCompany = _dbContext.AgentCompanies.First(ac => ac.Id == id);
            agentCompany.Status = (StatusEnum)status;
            if (status == 0)
            {
                _dbContext.AgentCompanies.Update(agentCompany);
                _dbContext.SaveChanges();
                return Ok(agentCompany);
            }
            else
            {
                agentCompany = _companyService.CreateReferral(agentCompany);
                _dbContext.AgentCompanies.Update(agentCompany);
                _dbContext.SaveChanges();
                return Ok(agentCompany);
            }
        }

        [HttpPut]
        [Route("ChangePolicyStatus")]
        public IActionResult PStatusChange()
        {
            int cpid = int.Parse(Request.Form["policyId"]);
            StatusEnum status = (StatusEnum)int.Parse(Request.Form["status"]);
            var dbpolicy = _companyService.GetPolicy(cpid);
            dbpolicy.Status = status;
            return Ok(_companyService.UpdatePolicy(dbpolicy));
        }

        [HttpGet]
        [Route("GetAgentCompany")]
        public IActionResult GetAgentCompany(int id)
        {
            return Ok(_dbContext.AgentCompanies.FirstOrDefault(ac => ac.Id == id));
        }
    }
}
