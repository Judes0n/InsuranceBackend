using InsuranceBackend.Models;
using InsuranceBackend.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace InsuranceBackend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AdminController : ControllerBase
    {   
        UserService _userService;
        InsuranceDbContext _dbContext;
        public AdminController()
        {
            _userService = new();
            _dbContext = new();
        }
       
    }
}
