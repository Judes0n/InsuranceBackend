using InsuranceBackend.Models;
using InsuranceBackend.Enum;
using System;
using Microsoft.EntityFrameworkCore;

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
            _context.Companies.Add(company);
            _context.SaveChangesAsync();
            return GetCompany(company.CompanyId);
        }

        public Company GetCompany(int companyID)
        {
            var res = _context.Companies.Find(companyID);
            return res == null ? throw new Exception() : res;
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
        public IEnumerable<AgentCompany> ViewAgents(int companyID)
        {
            return _context.AgentCompanies.Include(a=>a.CompanyId==companyID).ToList();
        }
        //Approvals
        public void SetCompanyStatus(int _companyID,ActorStatusEnum e)
        {
            var dbcompany = GetCompany(_companyID);
            if (!ActorStatusEnum.IsDefined(typeof(ActorStatusEnum), e))
            {
                throw new Exception();
            }
            dbcompany.Status = e;
            UpdateCompany(_companyID,dbcompany);
        }
        public void AddPolicy(Policy policy,int companyID)
        {
            ValidatePolicy(policy);
            policy.Status = StatusEnum.Inactive;
            policy.CompanyId = companyID;
            _context.Policies.Add(policy);
            _context.SaveChangesAsync();
        }

        public IEnumerable<Policy> ViewPolicies(int companyID)
        {
            return _context.Policies.Include(p=>p.CompanyId==companyID).ToList();
        }
        //Validations
        private void ValidatePolicy(Policy policy)
        {   
            var policytypeID=_context.PolicyTypes.Select(p=>p.PolicytypeId==policy.PolicytypeId);
            if (policytypeID==null)
                throw new ArgumentNullException(null, nameof(policytypeID));   
        }
    }
}
