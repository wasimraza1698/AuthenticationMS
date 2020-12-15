using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AuthService.Models;
using AuthService.Provider;
using AuthService.Repositories;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;

namespace AuthService.Controllers
{
    [ApiController]
    [Route("[Controller]/[Action]")]
    public class UserController : ControllerBase
    {
        private readonly log4net.ILog _log = log4net.LogManager.GetLogger(typeof(UserController));
        private readonly IUserProvider _userProvider;
        public UserController(IUserProvider userProvider)
        {
            _userProvider = userProvider;
        }
        
        [HttpPost]
        public IActionResult Login(User userCred)
        {
            _log.Info("Credentials of user : '" + userCred.UserName + "' Received");
            try
            {
                string token = _userProvider.Login(userCred);
                if (token != null)
                {
                    _log.Info("User : '"+userCred.UserName+"' is valid and token returned");
                    return Ok(new { token });
                }
                else
                {
                    _log.Info("User : '" + userCred.UserName + "' not found");
                    return NotFound("invalid username/password");
                }
            }
            catch (Exception e)
            {
                _log.Error("Error inside Controller while logging in. - "+e.Message);
                return StatusCode(500);
            }
        }
    }
}
