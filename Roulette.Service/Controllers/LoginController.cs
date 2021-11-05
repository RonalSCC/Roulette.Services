using Microsoft.AspNetCore.Mvc;
using Roulette.Entity;
using Roulette.Service.Security;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Roulette.Service.Controllers
{
    public class LoginController : Controller
    {
        public string Authentication(EntityBetUser user)
        {
            try
            {
                var token = TokenGenerator.GenerateTokenJwt(user.UserName);
                return token;
            }
            catch (Exception ex)
            {
                return string.Empty;
            }
        }
    }
}
