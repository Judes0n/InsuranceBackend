using InsuranceBackend.Models;
using InsuranceBackend.Enum;

namespace InsuranceBackend.Services
{
    public class CompanyServices
    {
        InsuranceDbContext _context;

        public CompanyServices()
        {
            _context = new InsuranceDbContext();
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
        //Approvals
        public void ApproveCompany(int _companyID)
        {
            var dbcompany = GetCompany(_companyID);
            dbcompany.Status = CompanyStatusEnum.Approved;
            UpdateCompany(_companyID,dbcompany);
        }
        public void RejectCompany(int _companyID)
        {
            var dbcompany = GetCompany(_companyID);
            dbcompany.Status = CompanyStatusEnum.Unapproved;
            UpdateCompany(_companyID, dbcompany);
        }
        public void BlockCompany(int _companyID)
        {
            var dbcompany = GetCompany(_companyID);
            dbcompany.Status = CompanyStatusEnum.Blocked;
            UpdateCompany(_companyID, dbcompany);
        }
        //

    }
}
