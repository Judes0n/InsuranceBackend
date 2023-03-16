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
    }
}
