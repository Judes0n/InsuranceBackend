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
        readonly ClientServices _clientService;
        readonly UserService _userService;
        readonly CompanyService _companyService;
        readonly AgentService _agentService;
        readonly InsuranceDbContext _dbContext;

        public UserController()
        {
            _clientService = new ClientServices();
            _agentService = new AgentService();
            _companyService = new CompanyService();
            _userService = new UserService();
            _dbContext = new();
        }

        //Register
        [HttpPost]
        [Route("Register")]
        public IActionResult Register()
        {
            User user =
                new()
                {                   
                    UserName = Request.Form["UserName"],
                    Password = Request.Form["Password"],
                    Type = (UserTypeEnum)Enum.Parse(typeof(UserTypeEnum), Request.Form["Type"]),
                    Status = StatusEnum.Inactive,
                };
            var logUser = _userService.GetUserByName(userName: user.UserName);
            if (logUser == null)
            {
                var file = Request.Form.Files[0];

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
                var fileName = ContentDispositionHeaderValue
                    .Parse(file.ContentDisposition)
                    .FileName.Trim()
                    .ToString();
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
                                var dbuser = _userService.GetUserByName(user.UserName);
                                if (dbuser != null)
                                {
                                    //client.ClientId = -1;
                                    client.UserId = dbuser.UserId;
                                    client.Address = Request.Form["address"];
                                    client.ClientName = Request.Form["clientName"];
                                    client.Gender = Request.Form["gender"];
                                    client.Dob = Request.Form["dob"];
                                    client.ProfilePic = dbPath;
                                    client.PhoneNum = Request.Form["phoneNum"];
                                    client.Email = Request.Form["email"];
                                    client.Status = (int)ActorStatusEnum.Unapproved;
                                    //client.User = dbuser;
                                    _clientService.AddClient(client);
                                }
                                return Ok(_dbContext.Clients.OrderBy(c => c.ClientId).Last());
                            }
                            case UserTypeEnum.Agent:
                            {
                                Agent agent = new();
                                var dbagent = _userService.GetUserByName(user.UserName);
                                if (dbagent != null)
                                {
                                    agent.UserId = dbagent.UserId;
                                    agent.AgentName = Request.Form["agentName"];
                                    agent.Gender = Request.Form["gender"];
                                    agent.PhoneNum = Request.Form["phoneNum"];
                                    agent.Dob = Request.Form["dob"];
                                    agent.Email = Request.Form["email"];
                                    agent.Address = Request.Form["address"];
                                    agent.Grade = 0;
                                    agent.ProfilePic = dbPath;
                                    agent.Status = (int)ActorStatusEnum.Unapproved;
                                    _agentService.AddAgent(agent);
                                }

                                return Ok(_dbContext.Agents.OrderBy(a=>a.AgentId).Last());
                            }
                            case UserTypeEnum.Company:
                            {
                                Company company = new();
                                var dbcompany = _userService.GetUserByName(user.UserName);
                                if (dbcompany != null)
                                {
                                    company.UserId = dbcompany.UserId;
                                    company.CompanyName = Request.Form["companyName"];
                                    company.Address = Request.Form["address"];
                                    company.Email = Request.Form["email"];
                                    company.PhoneNum = Request.Form["phoneNum"];
                                    company.ProfilePic = dbPath;
                                    company.Status = ActorStatusEnum.Unapproved;
                                    _companyService.AddCompany(company);
                                }
                               return Ok(_dbContext.Companies.OrderBy(c=>c.CompanyId).Last());
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    return BadRequest(ex.Message);
                }
                return BadRequest();
            }
            else if (logUser.UserName == user.UserName)
            {
                return BadRequest(new User() { UserId = -1 });
            }
            return BadRequest();
        }

        [HttpPost]
        [Route("Login")]
        public IActionResult Login([FromBody] User user)
        {
            var logUser = _userService.GetUserByName(user.UserName);
            if (logUser != null)
            {
                if (logUser.UserName == user.UserName && logUser.Password == user.Password)
                {
                    return Ok(logUser);
                }
                else
                    return BadRequest();
            }
            return BadRequest();
        }

        [HttpGet]
        [Route("GetAllUsers")]
        public async Task<IActionResult> GetAllUsers()
        {
            return Ok(await _dbContext.Users.Where(u => u.Type != 0).ToListAsync());
        }

        [HttpPost]
        [Route("Uploads")]
        public IActionResult Upload()
        {
            try
            {
                var file = Request.Form.Files[0];
                int type = int.Parse(Request.Form["type"]!);
                var folderName = Path.Combine("Resources", "Images", "Clients");
                switch (type)
                {
                    case 1:
                    {
                        folderName = Path.Combine("Resources", "Images", "Companies");
                        break;
                    }
                    case 2:
                    {
                        folderName = Path.Combine("Resources", "Images", "Agents");
                        break;
                    }
                    case 3:
                    {
                        folderName = Path.Combine("Resources", "Images", "Clients");
                        break;
                    }
                }

                var pathToSave = Path.Combine(Directory.GetCurrentDirectory(), folderName);
                if (file.Length > 0)
                {
                    var fileName = ContentDispositionHeaderValue
                        .Parse(file.ContentDisposition)
                        .FileName.Trim()
                        .ToString();
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

        [HttpGet]
        [Route("GetUserByName")]
        public IActionResult GetUserByName(string username)
        {
            return Ok(_userService.GetUserByName(username))
                ?? throw new Exception("Username not Found");
        }

        [HttpGet]
        [Route("GetUser")]
        public IActionResult GetUser(int userId)
        {
            return Ok(_userService.GetUser(userId)) ?? throw new Exception("Username not Found");
        }

        [HttpPost]
        [Route("Feed")]
        public IActionResult FeedPost(Feedback feedback)
        {
            _dbContext.Feedbacks.Add(feedback);
            _dbContext.SaveChanges();
            return Ok(feedback);
        }

        [HttpGet]
        [Route("Images/{*loc}")]
        public IActionResult GetImage([FromRoute] string loc)
        {
            if (!System.IO.File.Exists(loc))
            {
                return NotFound(); // Return a 404 Not Found response if the file doesn't exist
            }

            var fileContent = System.IO.File.ReadAllBytes(loc);
            var contentType = Path.GetExtension(loc);
            //var contentTypes = new[] { "image/jpeg", "image/png", "image/jpg" };
            return File(fileContent, $"image/{contentType}");
        }

        [HttpPut]
        [Route("UpdateUser")]
        public IActionResult UpdateUser()
        {
            User dbuser =
                new()
                {
                    UserName = Request.Form["userName"],
                    Password = Request.Form["password"],
                    UserId = int.Parse(Request.Form["userId"]),
                    Status = (StatusEnum)int.Parse(Request.Form["status"]),
                    Type = (UserTypeEnum)int.Parse(Request.Form["type"])
                };
            return Ok(_userService.UpdateUser(dbuser));
        }
    }
}
