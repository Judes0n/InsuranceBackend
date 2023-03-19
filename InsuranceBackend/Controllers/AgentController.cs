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
        AgentService _agentServices;
        UserService _userService;

        public AgentController()
        {
            _agentServices = new AgentService();
            _userService = new UserService();
        }

    }
}
