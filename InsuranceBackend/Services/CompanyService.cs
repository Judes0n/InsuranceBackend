using InsuranceBackend.Enum;
using System;
using Microsoft.EntityFrameworkCore;
using InsuranceBackend.Models;

namespace InsuranceBackend.Services
{
    public class CompanyService
    {
        InsuranceDbContext _context;

        public CompanyService()
        {
            _context = new InsuranceDbContext();
        }
        public Company AddCompany(Company company)
        {
            try
            {
                _context.Database.ExecuteSqlRaw("SET IDENTITY_INSERT Users ON");
                _context.Companies.Add(company);
                _context.SaveChangesAsync();
                _context.Database.ExecuteSqlRaw("SET IDENTITY_INSERT Users OFF");
            }
            catch (Exception) 
            {
                throw new Exception(null);
            }
            return GetCompanyByName(company.CompanyName);
        }

        public Company GetCompany(int companyID)
        {
            var res = _context.Companies.Find(companyID);
            return res ?? throw new Exception();
        }

        public Company GetCompanyByName(string companyName)
        {
            var res = _context.Companies.Find(companyName);
            return res ?? throw new Exception();
        }
        public IEnumerable<Company> GetAllCompanies()
        {
            return _context.Companies.ToList();
        }

        public void DeleteCompany(int companyID)
        {
            var dbcompany = GetCompany(companyID);
            _context.Companies.Remove(dbcompany);
        }

        public Company UpdateCompany(int companyID, Company company)
        {
            if (GetCompany(companyID) == null)
            {
                throw new Exception();
            }
            _context.Companies.Update(company);
            _context.SaveChanges();
            return GetCompany(companyID);
        }

        public void AddPolicy(Policy policy, int companyID)
        {
            ValidatePolicy(policy);
            policy.Status = StatusEnum.Inactive;
            policy.CompanyId = companyID;
            _context.Policies.Add(policy);
            _context.SaveChangesAsync();
        }

        //Status
        public void SetCompanyStatus(int _companyID, ActorStatusEnum e)
        {
            var dbcompany = GetCompany(_companyID);
            if (!ActorStatusEnum.IsDefined(typeof(ActorStatusEnum), e))
            {
                throw new Exception();
            }
            dbcompany.Status = e;
            UpdateCompany(_companyID, dbcompany);
        }

        public void ChangeAgentRequest(int agentID, StatusEnum e)
        {
            ValidateAgentRequest(agentID);
            var dbreq = _context.AgentCompanies.FirstOrDefault(a => a.AgentId == agentID) ?? throw new ArgumentNullException();
            if (!StatusEnum.IsDefined(typeof(StatusEnum), e))
            {
                throw new Exception();
            }
            dbreq.Status = e;
            _context.AgentCompanies.Update(dbreq);
            _context.SaveChanges();
        }
        
        //Views
        public IEnumerable<AgentCompany> ViewAgents(int companyID)
        {
            return _context.AgentCompanies.Include(a=>a.CompanyId==companyID).ToList();
        }
        
      

        public IEnumerable<Policy> ViewPolicies(int companyID)
        {
            return _context.Policies.Include(p=>p.CompanyId==companyID).ToList();
        }


        //Validations
        private void ValidatePolicy(Policy policy)
        {   
            var policytypeID=_context.PolicyTypes.Select(p=>p.PolicytypeId==policy.PolicytypeId) ?? throw new ArgumentNullException(null,nameof(policy));

        }

        private void ValidateAgentRequest(int agentID)
        {
            var dbreq = _context.AgentCompanies.FirstOrDefault(s => s.AgentId == agentID) ?? throw new NullReferenceException();
        }
    }
}
