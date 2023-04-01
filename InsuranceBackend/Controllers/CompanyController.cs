using InsuranceBackend.Enum;
using InsuranceBackend.Models;
using InsuranceBackend.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

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
        [HttpPost]
        [Route("AddPolicy")]

        public IActionResult AddPolicy(Policy policy)
        {
            if (policy == null)
                throw new ArgumentNullException(nameof(policy));
             _companyService.AddPolicy(policy);
             return Ok(policy);
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

