using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AuthService.Models;
using AuthService.Repositories;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;

namespace AuthService.Provider
{
    public class UserProvider : IUserProvider
    {
        private static readonly log4net.ILog _log = log4net.LogManager.GetLogger(typeof(UserProvider));
        private readonly IConfiguration _config;
        private readonly IUserRepository _userRepo;
        public UserProvider(IUserRepository useRepo,IConfiguration config)
        {
            _userRepo = useRepo;
            _config = config;
        }

        public string Login(User userCred)
        {
            User user = new User();
            UserDto userDto;
            try
            {
                userDto = _userRepo.GetUser(userCred);
                string token=null;
                if(userDto!=null)
                {
                    user.UserName = userDto.UserName;
                    token = GenerateJWT(userCred);
                }
                return token;
            }
            catch (Exception e)
            {
                _log.Error("Error in Provider while getting token - "+e.Message);
                throw;
            }
        }

        public string GenerateJWT(User userCred)
        {
            try
            {
                UserDto user = _userRepo.GetUser(userCred);
                if (user != null)
                {
                    var signingKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["Jwt:Key"]));
                    var credentials = new SigningCredentials(signingKey, SecurityAlgorithms.HmacSha256);
                    var token = new JwtSecurityToken(
                        _config["Jwt:Issuer"],
                        _config["Jwt:Issuer"],
                        null,
                        expires: DateTime.Now.AddMinutes(30),
                        signingCredentials: credentials);
                    return new JwtSecurityTokenHandler().WriteToken(token);
                }
                else
                {
                    return null;
                }
            }
            catch (Exception e)
            {
                _log.Error("Error in Provider while generating token - "+e.Message);
                throw;
            }
        }
    }
}
