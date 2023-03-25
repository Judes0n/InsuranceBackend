using InsuranceBackend.Models;
using Microsoft.EntityFrameworkCore;
using System.Text;

namespace InsuranceBackend.Services
{
    public class UserService
    {   
        InsuranceDbContext _context;
        public UserService()
        {
            _context=new InsuranceDbContext();
        }


        public User? GetUser(string userName)
        {
            var validUser = _context.Users.Where(u => u.UserName == userName).FirstOrDefault();
            return validUser;
        }

        public User AddUser(User user)
        {   
            _context.Users.Add(user);
            _context.SaveChangesAsync();
            return user;
        }

        public int ConvertFileContentsToInt(IFormFile file)
        {
            using var streamReader = new StreamReader(file.OpenReadStream(), Encoding.UTF8);
            var contents = streamReader.ReadToEnd();

            if (!int.TryParse(contents, out var result))
            {
                throw new ArgumentException("Invalid file contents. Cannot convert to int.");
            }

            return result;
        }
    }
}
