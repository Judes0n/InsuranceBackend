using InsuranceBackend.Models;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using System.Text;

namespace InsuranceBackend.Services
{
    public class UserService
    {
        InsuranceDbContext _context;

        public UserService()
        {
            _context = new InsuranceDbContext();
        }

        public User GetUserByName(string userName)
        {
            var validUser = _context.Users.FirstOrDefault(u => u.UserName == userName);
            return validUser;
        }

        public User? GetUser(int userID)
        {
            var validUser = _context.Users.FirstOrDefault(u => u.UserId == userID);
            return validUser;
        }

        public User AddUser(User user)
        {
            //user.UserId = -1;
            var con = new SqlConnection(
                "Server=JUDE;Database=InsuranceDB;Trusted_Connection=True;TrustServerCertificate=True;"
            );
            con.Open();
            var cmd = new SqlCommand(
                "INSERT INTO Users(userName,password,type,status) VALUES('"
                    + user.UserName
                    + "','"
                    + user.Password
                    + "','"
                    + (int)user.Type
                    + "',0)",
                con
            );
            cmd.ExecuteNonQuery();
            con.Close();
            //_context.Users.Add(user);
            //_context.SaveChanges();
            return user;
        }

        public void DeleteUser(User user)
        {
            _context.Users.Remove(user);
            _context.SaveChanges();
        }

        public User UpdateUser(User user)
        {
            var dbuser = _context.Users.FirstOrDefault(u => u.UserId == user.UserId);
            if (dbuser != null)
            {
                dbuser.UserName = user.UserName;
                dbuser.Password = user.Password;
                _context.Users.Update(dbuser);
                _context.SaveChanges();
            }
            return new() { UserId = 0 };
        }
    }
}
