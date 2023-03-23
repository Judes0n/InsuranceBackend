﻿using InsuranceBackend.Models;
using Microsoft.EntityFrameworkCore;

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
    }
}
