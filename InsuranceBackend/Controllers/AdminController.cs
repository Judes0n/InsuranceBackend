using InsuranceBackend.Enum;
using InsuranceBackend.Models;
using InsuranceBackend.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace InsuranceBackend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AdminController : ControllerBase
    {
        private readonly UserService _userService;
        private readonly AdminService _adminService;
        private readonly InsuranceDbContext _dbContext;
        public AdminController()
        {
            _userService = new();
            _adminService = new();
            _dbContext = new();
        }

        [HttpPut]
        [Route("ChangeUserStatus")]

        public IActionResult ChangeStatus(User user)
        {
            _adminService.ChangeUserStatus(user);
            return Ok(user);
        }

        [HttpPost]
        [Route("AddPolicyType")]

        public IActionResult AddType(PolicyType policyType) 
        { 
            return Ok(_adminService.AddPolicytype(policyType));
        }

        [HttpGet]
        [Route("GetAllTypes")]

        public IActionResult GetAllTypes()
        {
            return Ok(_dbContext.PolicyTypes.ToList());
        }
        [HttpGet]
        [Route("GetAllAgents")]

        public IActionResult GetAllAgent()
        {
            return Ok(_adminService.GetAllAgent());
        }

        [HttpGet]
        [Route("GetAllPolicies")]

        public IActionResult GetAll()
        {
            return Ok(_adminService.GetAllPolicies());
        }

        [HttpGet]
        [Route("GetAllMaturities")]

        public IActionResult GetAllMaturities()
        {
            return Ok(_dbContext.Maturities.ToList());
        }

        [HttpGet]
        [Route("GetFeedbacks")]

        public IActionResult GetFeeds()
        {
            return Ok(_dbContext.Feedbacks.ToList());
        }

        [HttpGet]
        [Route("GetPolicyTerms")]

        public IActionResult GetTerms(int policyId)
        {
            var res = _dbContext.PolicyTerms.Where(pt => pt.PolicyId == policyId).ToList();
            return Ok(res);
        }

        [HttpPut]
        [Route("ChangePolicyStatus")]

        public IActionResult UpdatePolicy(Policy policy)
        {
            _adminService.ChangePolicyStatus(policy);
            return Ok("Status Updated");
        }

        [HttpGet]
        [Route("GetPolicy")]

        public IActionResult GetPolicy(int policyId) 
        {
            return Ok(_dbContext.Policies.FirstOrDefault(p => p.PolicyId == policyId));
        }
    }
}
