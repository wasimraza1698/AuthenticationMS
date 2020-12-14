using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AuthService.Models;

namespace AuthService.Provider
{
    public interface IProvider
    {
        public string Login(User userCred);
        public string GenerateJWT(User userCred);

    }
}
