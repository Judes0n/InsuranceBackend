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
        [Route("Login")]

        public async Task<IActionResult> Login([FromBody] User user)
        {
            var logUser = _userService.GetUser(user.UserName);
            if (logUser == null || logUser.Type != UserTypeEnum.Company)
            {
                return BadRequest("Invalid Username");
            }
            if (logUser.Password == user.Password)
            {
                return Ok("Company Login Successful");
            }
            return BadRequest("Invalid Username or Password");
        }

        [HttpPost]
        [Route("Register")]

        public async Task<IActionResult> Register([FromBody] User user)
        {
            var logUser = _userService.GetUser(user.UserName);
            if (logUser == null)
            {
                user.Type = UserTypeEnum.Company;
                user.Status = StatusEnum.Inactive;
                if (_userService.AddUser(user) != null)
                {
                    return Ok("Company Registered!");
                }
                return BadRequest("Company Registration Failed!!");

            }
            else if (logUser.UserName == user.UserName)
            {
                return BadRequest("UserName is Already Used!!");
            }
            return BadRequest("Registration Failed");
        }
    }
}

