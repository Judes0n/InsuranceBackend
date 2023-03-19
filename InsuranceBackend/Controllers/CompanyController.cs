using InsuranceBackend.Enum;
using InsuranceBackend.Models;
using InsuranceBackend.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace InsuranceBackend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CompanyController : ControllerBase
    {
        CompanyService _companyService; 
        UserService _userService;

        public CompanyController()
        {
            _companyService = new CompanyService();
            _userService = new UserService();
        }

    }
}

