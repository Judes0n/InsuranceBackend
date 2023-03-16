using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using InsuranceBackend.Models;
using InsuranceBackend.Services;
using InsuranceBackend.Enum;
using Microsoft.Extensions.Logging.Abstractions;

namespace Insurance.Controllers
{
    [Route("api/[controller]")]
    [ApiController]

    public class ClientController : ControllerBase
    {
        ClientServices _clientService;
        UserService _loginService;
        public ClientController()
        {
            _clientService = new ClientServices();
            _loginService = new UserService();
        }

        [HttpPost]
        [Route("Login")]

        public async Task<IActionResult> Login([FromBody] User user)
        {
            var logUser = _loginService.GetUser(user.UserName);
            if (logUser==null || logUser.Type != UserTypeEnum.Client)
            {
                return BadRequest("Invalid UserName"); 
            }
            if (logUser.Password==user.Password) 
            {
                return Ok("Client Login Successful");
            }
            return BadRequest("Invalid UserName or Password");
            
        }

        [HttpPost]
        [Route("Register")]

        public async Task<IActionResult> Register([FromBody] User user)
        {
            var logUser = _loginService.GetUser(user.UserName);
            if (logUser == null)
            {
                user.Type = UserTypeEnum.Client;
                user.Status = StatusEnum.Inactive;
                if (_loginService.AddUser(user) != null)
                {
                    return Ok("Client Registered!");
                }
                return BadRequest("Client Registration Failed!!");
               
            }
            else if (logUser.UserName == user.UserName)
            {
                return BadRequest("UserName is Already Used!!");
            }
            return BadRequest("Registration Failed");
        }
        
    }

}
