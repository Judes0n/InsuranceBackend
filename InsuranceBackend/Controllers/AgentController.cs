using InsuranceBackend.Models;
using InsuranceBackend.Services;
using InsuranceBackend.Enum;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Insurance.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AgentController : ControllerBase
    {
        AgentServices _agentServices;
        UserService _userService;

        public AgentController()
        {
            _agentServices = new AgentServices();
            _userService = new UserService();
        }

        [HttpPost]
        [Route("Login")]

        public async Task<IActionResult> Login([FromBody]User user)
        {
            var logUser = _userService.GetUser(user.UserName);
            if (logUser == null || logUser.Type != UserTypeEnum.Agent) 
            {
                return BadRequest("Invalid Username"); 
            }
            if(logUser.Password == user.Password)
            {
                return Ok("Agent Login Successful");
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
                user.Type = UserTypeEnum.Agent;
                user.Status = UserStatusEnum.Inactive;
                if (_userService.AddUser(user) != null)
                {
                    return Ok("Agent Registered!");
                }
                return BadRequest("Agent Registration Failed!!");

            }
            else if (logUser.UserName == user.UserName)
            {
                return BadRequest("UserName is Already Used!!");
            }
            return BadRequest("Registration Failed");
        }
    }
}
