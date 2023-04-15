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

        [HttpGet]
        [Route("GetClientById")]

        public IActionResult GetClient(int userId)
        {
            var dbclient = _clientService.GetClientById(userId);
            return Ok(dbclient);
        }

        [HttpPost]
        [Route("AddNominee")]

        public IActionResult AddNominee(Nominee nominee)
        {
            _clientService.AddNominee(nominee);
            return Ok(nominee);
        }

        [HttpGet]
        [Route("ViewNominee")]

        public IEnumerable<Nominee> GetNominees(int clientId)
        {
           var nominees = _clientService.ViewClientNominees(clientId);
            return nominees;
        }

    }
}
