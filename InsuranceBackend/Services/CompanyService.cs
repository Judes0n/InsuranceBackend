using InsuranceBackend.Enum;
using System;
using Microsoft.EntityFrameworkCore;
using InsuranceBackend.Models;
using Microsoft.Data.SqlClient;

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
                var con = new SqlConnection("Server=JUDE;Database=InsuranceDB;Trusted_Connection=True;TrustServerCertificate=True;");
                con.Open();
                var cmd = new SqlCommand("INSERT INTO Companies(userID,companyName,address,email,phoneNum,profilePic,status) VALUES('" + company.UserId + "','" + company.CompanyName + "','Address','" + company.Email + "','" + company.PhoneNum + "','" + company.ProfilePic + "',0)", con);
                cmd.ExecuteNonQuery();
                con.Close();
            }
            catch (Exception) 
            {
                throw new Exception(null);
            }
            return GetCompanyByName(company.CompanyName);
        }

        public Company GetCompany(int userID)
        {
            var res = _context.Companies.FirstOrDefault(c=>c.UserId == userID);
            if(res == null)
            {
                return res = new Company();
            }
            return res;
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

        public Policy UpdatePolicy(Policy policy)
        {
            _context.Policies.Update(policy);
            _context.SaveChanges();
            return _context.Policies.First(p=>p.PolicyId == policy.PolicyId);
        }

        public void AddPolicy(Policy policy)
        {
            ValidatePolicy(policy);
            policy.PolicyId = 0;
            policy.Status = (int)StatusEnum.Inactive;
            var con = new SqlConnection("Server=JUDE;Database=InsuranceDB;Trusted_Connection=True;TrustServerCertificate=True;");
            con.Open();
            var cmd = new SqlCommand("INSERT INTO Policies(companyID,policytypeID,policyName,timePeriod,policyAmount,status) VALUES('" + policy.CompanyId + "','" + policy.PolicytypeId + "','" + policy.PolicyName + "','" + policy.TimePeriod + "','" + policy.PolicyAmount + "','" +(int) policy.Status + "')", con);
            cmd.ExecuteNonQuery();
            var PolicyId = _context.Policies.OrderByDescending(p => p.PolicyId).FirstOrDefault().PolicyId;
            var cmd2 = new SqlCommand("INSERT INTO PolicyTerms(policyID,period,terms,premiumAmount) VALUES('" + PolicyId + "','" + policy.TimePeriod + "',1,'" + (float) policy.PolicyAmount * 0.8 + "')", con);
            cmd2.ExecuteNonQuery();
            con.Close();
            //_context.Policies.Add(policy);
            //_context.SaveChanges();
        }

        public void AddPolicyTerm(PolicyTerm policyTerm)
        {
            var con = new SqlConnection("Server=JUDE;Database=InsuranceDB;Trusted_Connection=True;TrustServerCertificate=True;");
            con.Open();
            var cmd = new SqlCommand("INSERT INTO PolicyTerms(policyID,period,terms,premiumAmount) VALUES('" + policyTerm.PolicyId + "','" + policyTerm.Period + "','" + policyTerm.Terms + "','" + policyTerm.PremiumAmount +"')", con);
            cmd.ExecuteNonQuery();
            con.Close();
            //_context.PolicyTerms.Add(policyTerm);
            //_context.SaveChanges();
        }

        public Policy GetPolicy(int policyId)
        {
            Policy? policy = _context.Policies.FirstOrDefault(p => p.PolicyId == policyId);
            return policy ?? throw new NullReferenceException();

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
            AgentCompany dbreq = _context.AgentCompanies.FirstOrDefault(a => a.AgentId == agentID) ?? throw new ArgumentNullException();
            if (!StatusEnum.IsDefined(typeof(StatusEnum), e))
            {
                throw new Exception();
            }
            dbreq.Status = e;
            _context.AgentCompanies.Update(dbreq);
            _context.SaveChanges();
        }
        
        //Views
        public IEnumerable<AgentCompany> ViewAgents(int companyId)
        {
            return _context.AgentCompanies.Where(a=>a.CompanyId == companyId).ToList();
        }
        
      

        public IEnumerable<Policy> ViewPolicies(int companyID)
        {
            return _context.Policies.Include(p=>p.CompanyId==companyID).ToList();
        }


        public AgentCompany CreateReferral(AgentCompany _agentCompany)
        {
            Random random = new();
            var dbcompany = _context.Companies.First(c => c.CompanyId == _agentCompany.CompanyId);
            var dbagent = _context.Agents.First(a => a.AgentId == _agentCompany.AgentId);
            Retry:
            _agentCompany.Referral = dbcompany.CompanyName + dbagent.AgentName + random.Next(1,1000);
            var dbac = _context.AgentCompanies.FirstOrDefault(ac => ac.Referral == _agentCompany.Referral);
            if (dbac != null) goto Retry;
            return _agentCompany;
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
