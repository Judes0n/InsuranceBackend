using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using InsuranceBackend.Services;
using InsuranceBackend.Enum;
using Microsoft.Extensions.Logging.Abstractions;
using System.Net;
using System.Reflection;
using System;
using InsuranceBackend.Models;
using Microsoft.Net.Http.Headers;
using System.Numerics;
using Azure.Core;

namespace Insurance.Controllers
{
    [Route("api/[controller]")]
    [ApiController]

    public class UserController : ControllerBase
    {
        ClientServices _clientService;
        UserService _userService;
        CompanyService _companyService;
        AgentService _agentService;
        InsuranceDbContext _dbContext;
        public UserController()
        {
            _clientService = new ClientServices();
            _agentService = new AgentService();
            _companyService = new CompanyService();
            _userService = new UserService();
            _dbContext = new();
        }
        [HttpPost]
        [Route("Register")]

        public async Task<IActionResult> Register()
        {
            User user = new()
            {
                UserId   = int.Parse(Request.Form["UserId"]),
                UserName = Request.Form["UserName"],
                Password = Request.Form["Password"],
                Type     = (UserTypeEnum)Enum.Parse(typeof(UserTypeEnum), Request.Form["Type"].ToString()),
                Status   = StatusEnum.Inactive,
            };
            var logUser = _userService.GetUser(user.UserName);
            if (logUser == null)
            {
                user.Type = UserTypeEnum.Client;
                user.Status = StatusEnum.Inactive;

                var file = Request.Form.Files[0];
                string email = Request.Form["email"].ToString();
                string gender = Request.Form["gender"].ToString();
                var folderName = Path.Combine("Resources", "Images", "Clients");
                switch (user.Type)
                {
                    case UserTypeEnum.Company:
                        {
                            folderName = Path.Combine("Resources", "Images", "Companies");
                            break;
                        }
                    case UserTypeEnum.Agent:
                        {
                            folderName = Path.Combine("Resources", "Images", "Agents");
                            break;
                        }
                    case UserTypeEnum.Client:
                        {
                            folderName = Path.Combine("Resources", "Images", "Clients");
                            break;
                        }
                }

                    var pathToSave = Path.Combine(Directory.GetCurrentDirectory(), folderName);
                    var fileName = ContentDispositionHeaderValue.Parse(file.ContentDisposition).FileName.Trim().ToString();
                    var fullPath = Path.Combine(pathToSave, fileName);
                    var dbPath = Path.Combine(folderName, fileName);
                    using var stream = new FileStream(fullPath, FileMode.Create);
                    file.CopyTo(stream);
                try
                {
                    var res = _userService.AddUser(user);
                    if (res != null)
                    {
                        switch (user.Type)
                        {
                            case UserTypeEnum.Client:
                                {
                                    Client client = new();
                                    var dbuser = _userService.GetUser(user.UserName);
                                    if (dbuser != null)
                                    {
                                        //client.ClientId = -1;
                                        client.UserId = dbuser.UserId;
                                        client.Address = "Address";
                                        client.ClientName = dbuser.UserName;
                                        client.Gender = gender;
                                        client.Dob = "Date of Birth";
                                        client.ProfilePic = dbPath;
                                        client.PhoneNum = 911234567890;
                                        client.Email = email;
                                        client.Status = ActorStatusEnum.Unapproved;
                                        //client.User = dbuser;
                                        _clientService.AddClient(client);
                                    }
                                    break;
                                }
                            case UserTypeEnum.Agent:
                                {
                                    Agent agent = new();
                                    var dbagent = _userService.GetUser(user.UserName);
                                    if (dbagent != null)
                                    {
                                        agent.UserId = dbagent.UserId;
                                        agent.AgentName = dbagent.UserName;
                                        agent.Gender = gender;
                                        agent.PhoneNum = 0000000000;
                                        agent.Dob = "Date of Birth";
                                        agent.Email = email;
                                        agent.Address = "Address";
                                        agent.Grade = 0;
                                        agent.ProfilePic = dbPath;
                                        agent.Status = ActorStatusEnum.Unapproved;
                                        _agentService.AddAgent(agent);
                                    }

                                    break;
                                }
                            case UserTypeEnum.Company:
                                {
                                    Company company = new();
                                    var dbcompany = _userService.GetUser(user.UserName);
                                    if (dbcompany != null)
                                    {
                                        company.UserId = dbcompany.UserId;
                                        company.CompanyName = dbcompany.UserName;
                                        company.Address = "Address";
                                        company.Email = email;
                                        company.PhoneNum = 0;
                                        company.ProfilePic = dbPath;
                                        company.Status = ActorStatusEnum.Unapproved;
                                        _companyService.AddCompany(company);
                                    }
                                    break;
                                }
                        }
                        return Ok("User Registered!");
                    }
                }
                catch (Exception ex)
                {
                    _userService.DeleteUser(user);
                    return BadRequest(ex.Message);
                }
                return BadRequest("User Registration Failed!!");

            }
            else if (logUser.UserName == user.UserName)
            {
                return BadRequest("UserName is Unavailable!!");
            }
            return BadRequest("Registration Failed");
        }

        [HttpPost]
        [Route("Login")]

        public async Task<IActionResult> Login([FromBody] User user)
        {
            var logUser = _userService.GetUser(user.UserName);
            if (logUser != null)
            {
                if (logUser.UserName == user.UserName && logUser.Password== user.Password)
                {
                    return Ok(logUser);
                }
                else 
                    return BadRequest("Invalid Credentials");
            }
            return BadRequest("User Doesn't Exist!");
            
        }

        [HttpGet]
        [Route("GetAllUsers")]
        public async Task<IActionResult> GetAllUsers()
        {
            return Ok(await _dbContext.Users.ToListAsync());
        }

        [HttpPost]
        [Route("Uploads")]

        public IActionResult Upload()
        {
            try
            {
                var file = Request.Form.Files[0];
                int type= int.Parse(Request.Form["type"]!);
                var folderName = Path.Combine("Resources", "Images" , "Clients");
                switch (type)
                {
                    case 1:
                        {
                            folderName = Path.Combine("Resources", "Images" , "Companies");
                            break;
                        }
                    case 2:
                        { folderName = Path.Combine("Resources", "Images", "Agents");
                            break;
                        }
                    case 3:
                        {
                            folderName = Path.Combine("Resources", "Images", "Clients");
                            break;
                        }
                }
              
                var pathToSave = Path.Combine(Directory.GetCurrentDirectory(), folderName);
                if(file.Length > 0)
                {
                    var fileName = ContentDispositionHeaderValue.Parse(file.ContentDisposition).FileName.Trim().ToString();
                    var fullPath = Path.Combine(pathToSave, fileName);
                    var dbPath = Path.Combine(folderName, fileName);
                    using var stream = new FileStream(fullPath, FileMode.Create);
                    file.CopyTo(stream);
                    return Ok(new { dbPath });
                }
                else
                {
                    return BadRequest("File Upload Failed");
                }
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }

}
