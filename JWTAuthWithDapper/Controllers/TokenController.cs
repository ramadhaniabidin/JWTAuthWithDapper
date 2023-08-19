using JWTAuthWithDapper.Interface;
using JWTAuthWithDapper.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Text.Json;
using static System.Net.WebRequestMethods;

namespace JWTAuthWithDapper.Controllers
{
    [Route("api/[controller]/[action]")]
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
        public IActionResult DocodeToken(string token)
        {
            bool falseCondition = (string.IsNullOrWhiteSpace(token));
            if(!falseCondition)
            {
                TokenValidationParameters validationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidIssuer = "https://localhost:7112",
                    ValidAudience = "https://localhost:7112",
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]))
                };

                var tokenHandler = new JwtSecurityTokenHandler();
                ClaimsPrincipal claimsPrincipal = tokenHandler.ValidateToken(token, validationParameters, out SecurityToken validatedToken);

                var claims = claimsPrincipal.Claims;
                var email = claims.FirstOrDefault(c => c.Type == "Email")?.Value;

                return Ok(claims);
            }

            else 
            { 
                return BadRequest();
            }
        }

        [HttpPost]
        public IActionResult GetToken(string email, string password)
        {
            
            bool falseCondition = (string.IsNullOrWhiteSpace(email) || string.IsNullOrWhiteSpace(password));
            if(!falseCondition)
            {
                var user = userRepository.SelectUser(email, password);
                if (user != null)
                {
                    var claims = new[]
                    {
                        new Claim("Password", password),
                        new Claim("Email", email),
                        new Claim("Role", "Admin")
                    };

                    var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]));
                    //var psw = claims.FirstOrDefault(c => c.Type == "Email")?.Value;

                    var signIn = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
                    var token = new JwtSecurityToken(
                        Issuer,
                        Audience,
                        claims, expires: DateTime.UtcNow.AddMinutes(10), signingCredentials: signIn);
                    var jwtToken = new JwtSecurityTokenHandler().WriteToken(token);

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
