﻿using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using InsuranceBackend.Models;
using InsuranceBackend.Services;
using InsuranceBackend.Enum;
using Microsoft.Extensions.Logging.Abstractions;
using System.Net;
using System.Reflection;
using System;

namespace Insurance.Controllers
{
    [Route("api/[controller]")]
    [ApiController]

    public class UserController : ControllerBase
    {
        ClientService _clientService;
        UserService _userService;
        CompanyService _companyService;
        AgentService _agentService;
        public UserController()
        {
            _clientService = new ClientService();
            _agentService = new AgentService();
            _companyService = new CompanyService();
            _userService = new UserService();
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

        [HttpPost]
        [Route("Login")]

        public async Task<IActionResult> Login([FromBody] User user)
        {
            var logUser = _userService.GetUser(user.UserName);
            if (logUser != null)
            {
                if (logUser.UserName == user.UserName || logUser.Password== user.Password)
                {
                    switch (logUser.Type)
                    {
                        case UserTypeEnum.Client:
                            {
                                return Ok("Client Login");

                            }
                        case UserTypeEnum.Agent:
                            {
                                return Ok("Agent Login");

                            }
                        case UserTypeEnum.Company:
                            {
                                return Ok("Company Login");
                            }
                        case UserTypeEnum.Admin:
                            {
                                return Ok("Admin Login");
                            }
                    }
                }
                else 
                    return BadRequest("Invalid Credentials");
            }
            return BadRequest("User Doesn't Exist!");
            
        }
     
    }

}
