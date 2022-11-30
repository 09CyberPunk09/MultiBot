using Application.Services;
using LifeTracker.Web.Host.Models.IncomeModels.Account;
using LifeTracker.Web.Host.Models.IncomeModels.Users;
using Microsoft.AspNetCore.Authorization;
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

        private record Person(string Login, string Password, string Role);

        //TODO: Add real user getting instead of mock
        private List<Person> people = new List<Person>
        {
            new Person("admin@gmail.com","12345","admin" ),
            new Person("string","string","admin"),
        };

        [HttpPost]
        [AllowAnonymous]
        public IActionResult Login(LoginIncomeModel model)
        {
            #region
            //try
            //{
            //    if (string.IsNullOrEmpty(loginDTO.UserName) ||
            //    string.IsNullOrEmpty(loginDTO.Password))
            //        return BadRequest("Username and/or Password not specified");
            //    if (loginDTO.UserName.Equals("string") &&
            //    loginDTO.Password.Equals("string"))
            //    {
            //        var secretKey = new SymmetricSecurityKey
            //        (Encoding.ASCII.GetBytes(_configuration["LifeTracker.Web.Host:Authentification:SecurityKey"]));

            //        var signinCredentials = new SigningCredentials
            //       (secretKey, SecurityAlgorithms.HmacSha256);

            //        var jwtSecurityToken = new JwtSecurityToken(
            //            issuer: _configuration["LifeTracker.Web.Host:Authentification:Issuer"],
            //            audience: _configuration["LifeTracker.Web.Host:Authentification:Audience"]
            //           // claims: new List<Claim>(),
            //         //   expires: DateTime.Now.AddMinutes(10)

            //            //,signingCredentials: signinCredentials
            //        );
            //        return Ok(new JwtSecurityTokenHandler().
            //        WriteToken(jwtSecurityToken));
            //    }
            //}
            //catch
            //{
            //    return BadRequest
            //    ("An error occurred in generating the token");
            //}
            //return Unauthorized();
            #endregion
            var identity = GetIdentity(model.EmailAddress, model.Password);
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
                    expires: now.Add(TimeSpan.FromHours(AuthOptions.LIFETIME)),
                    signingCredentials: new SigningCredentials(AuthOptions.GetSymmetricSecurityKey(), SecurityAlgorithms.HmacSha256));
            var encodedJwt = new JwtSecurityTokenHandler().WriteToken(jwt);

            var response = new
            {
                access_token = encodedJwt
            };

            return Ok(response);
        }

        private ClaimsIdentity GetIdentity(string emailAddress, string password)
        {
            var person = _service.GetUserByEmail(emailAddress);

            if (person != null && person.Password.Equals(password))
            {
                var claims = new List<Claim>
                {
                    new Claim(ClaimsIdentity.DefaultNameClaimType, person.EmailAddress),
                    new Claim("UserId", person.Id.ToString())
                };

                ClaimsIdentity claimsIdentity =
                new ClaimsIdentity(claims, "Token", ClaimsIdentity.DefaultNameClaimType,
                    ClaimsIdentity.DefaultRoleClaimType);
                return claimsIdentity;
            }

            return null;
        }

        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> SignUp(SignUpIncomeModel model)
        {
            await _service.SignUp(model.Name, model.Password, model.Email);
            return Ok();
        }
    }
}