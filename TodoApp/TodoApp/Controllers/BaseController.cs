using Microsoft.AspNetCore.Mvc;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using TodoApp.Utiities.Exceptions;

namespace TodoApp.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public abstract class BaseController : Controller
    {

        protected BaseController()
        {

        }

        public string roleName => User.Claims.First(c => c.Type == ClaimTypes.Role).Value;

        public string userId
        {

            set
            {
                Claim? claim = User.Claims.FirstOrDefault(x => x.Type == JwtRegisteredClaimNames.Sid) ?? throw new UnauthorizedException("Not Authorized");
            }
            get
            {
                Claim? claim = User.Claims.FirstOrDefault(x => x.Type == JwtRegisteredClaimNames.Sid) ?? throw new UnauthorizedException("Not Authorized");
                return claim.Value;
            }
        }
    }
}
