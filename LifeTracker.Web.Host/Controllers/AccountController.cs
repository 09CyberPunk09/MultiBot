using Application.Services.Users;
using Common.Entites;
using LifeTracker.Web.Host.Models.IncomeModels.Account;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace LifeTracker.Web.Host.Controllers
{
    [ApiController]
    [Route("api/[controller]/[action]")]
    public class AccountController : ControllerBase
    {
        private readonly UserAppService _service;

        public AccountController(UserAppService service)
        {
            _service = service;
        }

        [HttpPost]
        public async Task<ActionResult>  SignUp([FromBody] SignUpDto dto)
        {
            //TODO Add SignUpDto to service method parameters ad pass there a dto
            await _service.SignUp(dto.Name,dto.EmailAddress,dto.Password);
            return Ok();
        }

        [HttpPost]
        public ActionResult SignIn([FromBody] LoginIncomeModel model)
        {
            var (user, identity) = GetIdentity(model.EmailAddress, model.Password);
            if (identity == null)
            {
                return BadRequest(new { errorText = "Invalid username or password." });
            }

            var now = DateTime.UtcNow;
            var jwt = new JwtSecurityToken(
                    issuer: AuthOptions.ISSUER,
                    audience: AuthOptions.AUDIENCE,
                    notBefore: now,
                    claims: identity.Claims,
                    expires: now.Add(TimeSpan.FromMinutes(AuthOptions.LIFETIME)),
                    signingCredentials: new SigningCredentials(AuthOptions.GetSymmetricSecurityKey(), SecurityAlgorithms.HmacSha256));
            var encodedJwt = new JwtSecurityTokenHandler().WriteToken(jwt);

            var response = new
            {
                access_token = encodedJwt,
            };
            return Ok(response);
        }

        private (User, ClaimsIdentity) GetIdentity(string email, string password)
        {
            //TODO: Make database-side checking, not taking all users from db
            User person = _service.GetAll().FirstOrDefault(x => x.EmailAddress == email && x.Password == password);
            if (person != null)
            {
                var claims = new List<Claim>
                {
                    new Claim(ClaimsIdentity.DefaultNameClaimType, person.EmailAddress),
                    new Claim("UserId", person.Id.ToString())
                };
                ClaimsIdentity claimsIdentity =
                new ClaimsIdentity(claims, "Token", ClaimsIdentity.DefaultNameClaimType,
                    ClaimsIdentity.DefaultRoleClaimType);
                return (person, claimsIdentity);
            }

            return (null, null);
        }
    }
}