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

        public async Task<IActionResult> Register([FromBody] User user)
        {
           
            var logUser = _userService.GetUser(user.UserName);
            if (logUser == null)
            {
                user.Type = UserTypeEnum.Client;
                user.Status = StatusEnum.Inactive;
                if (_userService.AddUser(user) != null)
                { 
                    switch(user.Type)
                    {
                        case UserTypeEnum.Client:
                            {
                                Client client = new();
                                var dbuser = _userService.GetUser(user.UserName);
                                if (dbuser != null)
                                {
                                    client.UserId = dbuser.UserId;
                                    client.Address = "Address";
                                    client.ClientName = dbuser.UserName;
                                    client.Gender = "Gender";
                                    client.Dob = "Date of Birth";
                                    client.ProfilePic = "Profile Pic";
                                    client.PhoneNum = 0000000000;
                                    client.Email = "Email";
                                    client.Status = ActorStatusEnum.Unapproved;
                                   _clientService.AddClient(client);
                                }
                                break;
                            }
                        case UserTypeEnum.Agent:
                            {
                                Agent agent = new();
                                var dbagent= _userService.GetUser(user.UserName); 
                                if (dbagent != null) 
                                { 
                                    agent.UserId = dbagent.UserId;
                                    agent.AgentName = dbagent.UserName;
                                    agent.Gender = "Gender";
                                    agent.PhoneNum = 0000000000;
                                    agent.Dob = "Date of Birth";
                                    agent.Email = "Email";
                                    agent.Address = "Address";
                                    agent.Grade = 0;
                                    agent.ProfilePic = "ProfilePic";
                                    agent.Status=ActorStatusEnum.Unapproved;
                                    _agentService.AddAgent(agent);
                                }

                                break;
                            }
                        case UserTypeEnum.Company:
                            {
                                Company company = new();
                                var dbcompany = _userService.GetUser(user.UserName);
                                if(dbcompany != null)
                                {
                                    company.UserId = dbcompany.UserId;
                                    company.CompanyName = dbcompany.UserName;
                                    company.Address = "Address";
                                    company.Email = "Email";
                                    company.PhoneNum = 0;
                                    company.ProfilePic = "Profile Pic";
                                    company.Status = ActorStatusEnum.Unapproved;
                                    _companyService.AddCompany(company);
                                }
                                break;
                            }
                    }
                    return Ok("User Registered!");
                }
                return BadRequest("User Registration Failed!!");
                
            }
            else if (logUser.UserName == user.UserName)
            {
                return BadRequest("UserName is Already Used!!");
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
                    switch (logUser.Type)
                    {
                        case UserTypeEnum.Client:
                            {
                                return Ok("Client");

                            }
                        case UserTypeEnum.Agent:
                            {
                                return Ok("Agent");

                            }
                        case UserTypeEnum.Company:
                            {
                                return Ok("Company");
                            }
                        case UserTypeEnum.Admin:
                            {
                                return Ok("Admin");
                            }
                    }
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

    }

}
