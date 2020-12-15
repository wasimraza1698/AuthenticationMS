using AuthService.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AuthService.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly log4net.ILog _log = log4net.LogManager.GetLogger(typeof(UserRepository));
        private static readonly List<UserDto> Users = new List<UserDto>()
        {
            new UserDto(){UserID = 1,UserName = "John",Password = "john@123"},
            new UserDto(){UserID = 2,UserName = "Bucky",Password = "bucky@123"}
        };
        public UserDto GetUser(User user)
        {
            try
            {
                UserDto  userDto =  Users.SingleOrDefault(u => u.UserName == user.UserName && u.Password == user.Password);
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
