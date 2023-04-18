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

        public IActionResult AddNominee()
        {
            Nominee nominee = new Nominee()
            {
                NomineeName = Request.Form["nomineeName"],
                PhoneNum = Request.Form["phoneNum"],
                Relation = Request.Form["Relation"],
                ClientId = int.Parse(Request.Form["clientId"]),
                Address = Request.Form["address"],
            };

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

        [HttpGet]
        [Route("ViewPolicies")]

        public IEnumerable<Policy> GetPolicies(int policytypeId = 0,int agentId=0,int order=0)
        {
             
            if (policytypeId != 0 && order !=0) {
               return _clientService.GetPolicies(typeId:policytypeId,order:1);
            }
            else if(agentId != 0 && order !=0)
            {
               return _clientService.GetPolicies(agentId: agentId,order:1);
            }
            else if(order !=0)
            {
                return _clientService.GetPolicies(order:order);
            }
            else
                return _clientService.GetPolicies();
            
        }
        //ClientPolicies
    }
}
