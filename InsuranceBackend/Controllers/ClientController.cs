using InsuranceBackend.Models;
using InsuranceBackend.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace InsuranceBackend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ClientController : ControllerBase
    {
        UserService _userService;
        ClientService _clientService;
        public ClientController() 
        {
            _clientService = new();
            _userService = new();
        }

        [HttpGet]
        public IEnumerable<ClientPolicy> ViewClientPolicies(int clientID,int agentID,int policytermID)
        {
            return _clientService.ViewClientPolicies(clientID,agentID,policytermID) ?? throw new NullReferenceException();
        }

        [HttpGet]

        public IEnumerable<Nominee> ViewNominees(int clientID)
        {
            return _clientService.ViewNominees(clientID) ?? throw new NullReferenceException();
        }

        [HttpPost]

        public async Task <IActionResult> AddClientPolicy(ClientPolicy clientPolicy)
        {
            _clientService.AddClientPolicy(clientPolicy);
            return Ok("Client Policy Added!");
        }
    }
}
