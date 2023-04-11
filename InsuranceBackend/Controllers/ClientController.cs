using InsuranceBackend.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace InsuranceBackend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ClientController : ControllerBase
    {
        ClientServices _clientService;
        UserService _userService;

        public ClientController()
        {
            _clientService = new ClientServices();
            _userService = new UserService();
        }

        [HttpGet]
        [Route("GetClient")]

        public IActionResult GetClientById(int clientId)
        {
           var dbclient  = _clientService.GetClient(clientId);
            return Ok(dbclient);
        }


    }
}
