using AuthService.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AuthService.Repositories
{
    public class UserRepo : IRepository
    {
        private readonly log4net.ILog _log = log4net.LogManager.GetLogger(typeof(UserRepo));
        private static List<User> users = new List<User>()
        {
            new User(){UserID = 1,UserName = "John",Password = "john@123"},
            new User(){UserID = 2,UserName = "Bucky",Password = "bucky@123"}
        };
        public User GetUser(User user)
        {
            try
            {
                User userDto =  users.SingleOrDefault(u => u.UserName == user.UserName && u.Password == user.Password);
                return userDto;
            }
            catch (Exception e)
            {
                _log.Error("Error in Repository while getting User - "+e.Message);
                throw;
            }
            
        }
    }
}
