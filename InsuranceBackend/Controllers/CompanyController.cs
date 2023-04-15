﻿using InsuranceBackend.Enum;
using InsuranceBackend.Models;
using InsuranceBackend.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Text.Json.Serialization;

namespace InsuranceBackend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CompanyController : ControllerBase
    {
       readonly CompanyService _companyService;
       readonly UserService _userService;
       readonly InsuranceDbContext _dbContext;
        public CompanyController()
        {
            _companyService = new CompanyService();
            _userService = new UserService();
            _dbContext = new();
        }

        [HttpGet]
        [Route("GetPolicy")]

        public IActionResult GetPolicy(int policyId)
        {
           Policy policy = _companyService.GetPolicy(policyId);
            return Ok(policy);
        }

        [HttpPost]
        [Route("AddPolicy")]

        public IActionResult AddPolicy(Policy policy)
        {
            if (policy == null)
                throw new ArgumentNullException(nameof(policy));
             _companyService.AddPolicy(policy);
             return Ok(policy);
        }

        [HttpPost]
        [Route("AddPolicyTerm")]
        public IActionResult AddPolicyTerm(PolicyTerm policyterm)
        {
            if (policyterm == null)
                throw new ArgumentNullException(nameof(policyterm));
            _companyService.AddPolicyTerm(policyterm);
            return Ok(policyterm);
        }

        [HttpGet]
        [Route("ViewPolicies")]

        public IEnumerable<Policy> ViewPolicies(int companyID)
        {
           return _companyService.ViewPolicies(companyID);
        }

        [HttpGet]
        [Route("ViewAgents")]

        public IEnumerable<AgentCompany> ViewAgents(int companyId)
        {
            return _companyService.ViewAgents(companyId);
        }

        [HttpGet]
        [Route("GetCompany")]

        public Company GetCompany(int userID) 
        { 
            return _companyService.GetCompany(userID);
        }


        [HttpGet]
        [Route("GetAllCompany")]

        public IEnumerable<Company> GetAll()
        {
            return _companyService.GetAllCompanies();
        }

        [HttpPost]
        [Route("ChangeAgentCompanyStatus")]

        public IActionResult Change()
        {
            int id = int.Parse(Request.Form["id"]);
            int status = int.Parse(Request.Form["status"]);
            AgentCompany agentCompany = new();
            agentCompany = _dbContext.AgentCompanies.FirstOrDefault(ac => ac.Id == id);
            agentCompany.Status =(StatusEnum)status;
            if (status != 1)
            {
                _dbContext.AgentCompanies.Update(agentCompany);
                _dbContext.SaveChanges();
                return Ok(agentCompany);
            }
            else
            {
                var dbcompany = _dbContext.Companies.FirstOrDefault(c => c.CompanyId == agentCompany.CompanyId);
                var dbagent = _dbContext.Agents.FirstOrDefault(a => a.AgentId == agentCompany.AgentId);
                Random random = new();
            Retry:
                agentCompany.Referral = dbcompany.CompanyName + dbagent.AgentName + random.Next(1, 1000);
                var dbac = _dbContext.AgentCompanies.FirstOrDefault(ac => ac.Referral == agentCompany.Referral);
                if (dbac == null)
                {
                    _dbContext.AgentCompanies.Update(agentCompany);
                }
                else
                    goto Retry;
                _dbContext.SaveChanges();
                return Ok(agentCompany);
            }
        }

        [HttpGet]
        [Route("GetAgentCompany")]
        public IActionResult GetAgentCompany(int id) 
        { 
            return Ok(_dbContext.AgentCompanies.FirstOrDefault(ac=>ac.Id == id));
        }
    } 
}

