using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using VirtualMind.NetTest.Arquitetura.Library.Util.Security;

namespace VirtualMind.NetTest.Service.Controllers
{
    public class AuthController : Controller
    {
        [HttpPost("login")]
        public ActionResult<UserResponseToken> login([FromServices] SigningConfigurations signingConfigurations, [FromServices] TokenConfigurations tokenConfigurations)
        {
            Dictionary<string, string> claims = new Dictionary<string, string>();

            claims.Add("userId", "12345");

            return Token.GetToken(claims, signingConfigurations, tokenConfigurations);

        }
    }
}
