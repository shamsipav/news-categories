using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using SpecialityCatalogWebAPI.Classes;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace SpecialityCatalogWebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        [HttpPost, Route("login")]
        public IActionResult Login(AuthDTO authDTO)
        {
            try
            {
                if (string.IsNullOrEmpty(authDTO.UserName) || string.IsNullOrEmpty(authDTO.Password))
                {
                    return BadRequest("Некорректное имя пользователя и (или) пароль");
                }

                if (authDTO.UserName.Equals("pavel") && authDTO.Password.Equals("pavel123"))
                {
                    var signinCredentials = new SigningCredentials(AuthOptions.GetSymmetricSecurityKey(), SecurityAlgorithms.HmacSha256);

                    var claims = new List<Claim> { new Claim(ClaimTypes.Name, authDTO.UserName) };

                    var jwt = new JwtSecurityToken(
                        issuer: AuthOptions.ISSUER,
                        audience: AuthOptions.AUDIENCE,
                        claims: claims,
                        expires: DateTime.Now.AddMinutes(5),
                        signingCredentials: signinCredentials
                    );

                    return Ok(new JwtSecurityTokenHandler().WriteToken(jwt));
                }
            }
            catch
            {
                return BadRequest("Возникла ошибка при генерации токена");
            }
            return Unauthorized();
        }
    }
}
