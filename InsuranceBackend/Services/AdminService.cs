using InsuranceBackend.Models;

namespace InsuranceBackend.Services
{
    public class AdminService
    {
        InsuranceDbContext _context;
        public AdminService()
        {
            _context = new InsuranceDbContext();
        }
        public IEnumerable<Agent> GetAllAgent()
        {
            return _context.Agents.ToList();
        }
        public IEnumerable<Company> GetAllCompanies()
        {
            return _context.Companies.ToList();
        }
        public IEnumerable<Client> GetAllClient()
        {
            return _context.Clients.ToList();
        }
    }
}
