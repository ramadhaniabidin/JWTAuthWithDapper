using JWTAuthWithDapper.Interface;
using JWTAuthWithDapper.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace JWTAuthWithDapper.Controllers
{
    [Route("api/token")]
    [ApiController]
    public class TokenController : Controller
    {
        //For the sake of Jwt
        private protected string Issuer = "https://localhost:7112";
        private protected string Audience = "https://localhost:7112";


        private readonly IUserRepository userRepository;
        private readonly IConfiguration _configuration;
        public TokenController(IUserRepository _userRepository, IConfiguration configuration)
        {
            userRepository = _userRepository;
            _configuration = configuration;
        }

        [HttpPost]
        public IActionResult Post(string email, string password)
        {
            
            bool falseCondition = (string.IsNullOrWhiteSpace(email) || string.IsNullOrWhiteSpace(password));
            if(!falseCondition)
            {
                var user = userRepository.SelectUser(email, password);
                if (user != null)
                {
                    var claims = new[]
                    {
                        //new Claim(JwtRegisteredClaimNames.Sub, _configuration["Jwt:Subject"]),
                        //new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                        //new Claim(JwtRegisteredClaimNames.Iat, DateTime.UtcNow.ToString()),
                        //new Claim("UserId", user.UserId.ToString()),
                        //new Claim("DisplayName", user.DisplayName),
                        new Claim("Password", password),
                        new Claim("Email", email),
                        new Claim("Role", "Admin")
                    };

                    var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]));
                    var signIn = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
                    var token = new JwtSecurityToken(
                        Issuer,
                        Audience,
                        claims, expires: DateTime.UtcNow.AddMinutes(10), signingCredentials: signIn);

                    return Ok(new JwtSecurityTokenHandler().WriteToken(token));
                }

                else
                {
                    return BadRequest("Invalid Credentials");
                }
            }

            else 
            {
                return BadRequest();
            }
        }
    }
}
