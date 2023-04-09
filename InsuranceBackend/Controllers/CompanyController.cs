using InsuranceBackend.Enum;
using InsuranceBackend.Models;
using InsuranceBackend.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Text.Json.Serialization;

namespace InsuranceBackend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CompanyController : ControllerBase
    {
        CompanyService _companyService;
        UserService _userService;

        public CompanyController()
        {
            _companyService = new CompanyService();
            _userService = new UserService();
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

        public IActionResult AddPolicy(Policy policy)
        {
            if (policy == null)
                throw new ArgumentNullException(nameof(policy));
             _companyService.AddPolicy(policy);
             return Ok(policy);
        }

        [HttpPost]
        [Route("AddPolicyTerm")]
        public IActionResult AddPolicyTerm(PolicyTerm policyterm)
        {
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

        public IEnumerable<AgentCompany> ViewAgents(int companyID)
        {
            return _companyService.ViewAgents(companyID);
        }

        [HttpGet]
        [Route("GetCompany")]

        public Company GetCompany(int userID) 
        { 
            return _companyService.GetCompany(userID);
        }
    } 
}

