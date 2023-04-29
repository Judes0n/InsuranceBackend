using InsuranceBackend.Models;
using Microsoft.Data.SqlClient;
using InsuranceBackend.Database;

namespace InsuranceBackend.Services
{
    public class UserService
    {   
        private InsuranceDbContext _context;

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
            var con = new SqlConnection(DBConnection.ConnectionString);
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
