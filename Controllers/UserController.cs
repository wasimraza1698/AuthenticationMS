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
        private static readonly log4net.ILog _log = log4net.LogManager.GetLogger(typeof(UserController));
        private readonly IProvider _userProvider;
        public UserController(IProvider userProvider)
        {
            _userProvider = userProvider;
        }
        [HttpPost]
        public IActionResult GetUser(User userCred)
        {
            _log.Info("Credentials of user : '"+userCred.UserName+"' Received");
            try
            {
                User user = _userProvider.GetUser(userCred);
                if (user != null)
                {
                    _log.Info("User : '"+user.UserName+"' having ID : '"+user.UserID.ToString()+"' found and returned");
                    return new OkObjectResult(user);
                }
                else
                {
                    _log.Info("User : '"+userCred.UserName+"' not found");
                    return NotFound();
                }
            }
            catch (Exception e)
            {
                _log.Error("Error inside Controller while getting User details. - "+e.Message);
                return StatusCode(500);
            }
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
                    return Ok(new {token = token});
                }
                else
                {
                    _log.Info("User : '" + userCred.UserName + "' not found");
                    return NotFound();
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