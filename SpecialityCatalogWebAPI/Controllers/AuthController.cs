using FluentValidation;
using FluentValidation.Results;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using SpecialityCatalogWebAPI.Classes;
using SpecialityCatalogWebAPI.Data;
using SpecialityCatalogWebAPI.Utils;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace SpecialityCatalogWebAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private IValidator<User> _validator;
        private readonly NewsDbContext _context;
        public AuthController(NewsDbContext context, IValidator<User> validator)
        {
            _context = context;
            _validator = validator;
        }

        /// <summary>
        /// Регистрация пользователя
        /// </summary>
        /// <param name="user">Объект User</param>
        /// <returns></returns>
        [HttpPost("signup")]
        public async Task<IActionResult> PostAsync(User user)
        {
            ValidationResult validationResult = await _validator.ValidateAsync(user);

            if (!validationResult.IsValid)
                return BadRequest(validationResult.Errors[0].ErrorMessage);

            var existUser = _context.Users.FirstOrDefault(u => u.Email == user.Email);
            if (existUser != null)
                return BadRequest("Пользователь с таким Email уже зарегистрирован");

            user.Email = user.Email.ToLower();

            // TODO: Remove BCrypt.Net
            string password = BCrypt.Net.BCrypt.HashPassword(user.Password);
            user.Password = password;

            user.LastLoginIp = HttpContext.Request.Host.ToString();

            try
            {
                _context.Users.Add(user);
                _context.SaveChanges();
            }
            catch (Exception ex)
            {
                return Problem("Возникла ошибка при регистрации пользователя");
            }

            return Ok("Пользователь успешно зарегистрирован");
        }

        /// <summary>
        /// Вход в аккаунт
        /// </summary>
        /// <param name="login">Объект Login</param>
        /// <returns></returns>
        [HttpPost("login")]
        public IActionResult Post(Login login)
        {
            var email = login.Email;
            var password = login.Password;

            if (email == null)
                return BadRequest("Email яляется обязательным");

            if (password == null)
                return BadRequest("Password яляется обязательным");

            var existUser = _context.Users.FirstOrDefault(u => u.Email == email.ToLower());
            if (existUser is null)
                return NotFound("Пользователь с таким Email не найден");

            bool passwordComparison = BCrypt.Net.BCrypt.Verify(password, existUser.Password);
            if (!passwordComparison) return BadRequest("Неверный пароль");

            var claims = new List<Claim> { new Claim("email", email) };

            var timeNow = DateTime.UtcNow;

            var jwt = new JwtSecurityToken(
                issuer: AuthOptions.ISSUER,
                audience: AuthOptions.AUDIENCE,
                notBefore: timeNow,
                claims: claims,
                expires: timeNow.Add(TimeSpan.FromDays(AuthOptions.LIFETIMEDAYS)),
                signingCredentials: new SigningCredentials(AuthOptions.GetSymmetricSecurityKey(), SecurityAlgorithms.HmacSha256));

            var encodedJwt = new JwtSecurityTokenHandler().WriteToken(jwt);

            return Ok(encodedJwt);
        }

        /// <summary>
        /// Получение информации о пользователе
        /// </summary>
        /// <returns></returns>
        [Authorize]
        [HttpGet("user")]
        public IActionResult Get()
        {
            var headers = Request.Headers;
            headers.TryGetValue("Authorization", out var authHeader);
            string token = authHeader.ToString().Split(' ').Skip(1).FirstOrDefault();

            string email = Validate.ValidateToken(token);

            var existUser = new User();

            if (email != null)
            {
                existUser = _context.Users.FirstOrDefault(u => u.Email == email.ToLower());
                if (existUser is null)
                    return NotFound("Пользователь с таким Email не найден");
            }
            else
                return BadRequest("Некорректный токен");

            return Ok(new UserDTO
            {
                Id = existUser.Id,
                FirstName = existUser.FirstName,
                LastName = existUser.LastName,
                Email = existUser.Email,
                LastLoginDate = existUser.LastLoginDate,
                LastLoginIP = existUser.LastLoginIp
            });
        }
    }
}
