using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using S19L1.Settings;
using S19L1.Models;
using Microsoft.Extensions.Options;
using S19L1.DTOs.Account;
using Microsoft.AspNetCore.Identity.Data;
using System.Security.Claims;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using System.IdentityModel.Tokens.Jwt;

namespace S19L1.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        private readonly Jwt _jwtSettings;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly RoleManager<ApplicationRole> _roleManager;

        public AccountController(IOptions<Jwt> jwtOptions,
            UserManager<ApplicationUser> userManager,
            SignInManager<ApplicationUser> signInManager,
            RoleManager<ApplicationRole> roleManager)
        {
            _jwtSettings = jwtOptions.Value;
            _userManager = userManager;
            _signInManager = signInManager;
            _roleManager = roleManager;
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterRequestDto registerRequest)
        {
            try
            {
                var newUser = new ApplicationUser()
                {
                    Email = registerRequest.Email,
                    UserName = registerRequest.Email,
                    FirstName = registerRequest.FirstName,
                    LastName = registerRequest.LastName
                };

                var result = await _userManager.CreateAsync(newUser, registerRequest.Password);

                if (!result.Succeeded)
                {
                    return BadRequest(new { message = "Something went wrong!" });
                }

                await _userManager.AddToRoleAsync(newUser, "Admin");

                return Ok(new { message = "Account successfully registered!" });
            }
            catch
            {
                return StatusCode(500, new { message = "An error occurred while processing the request" });
            }
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login(LoginRequestDto loginRequest)
        {
            try
            {
                var user = await _userManager.FindByEmailAsync(loginRequest.Email);

                if (user == null)
                {
                    return Unauthorized(new { message = "Invalid email or password" });
                }

                var result = await _signInManager.PasswordSignInAsync(user, loginRequest.Password, false, false);

                if (!result.Succeeded) 
                {
                    return Unauthorized(new { message = "Invalid email or password" });
                }

                var Roles = await _signInManager.UserManager.GetRolesAsync(user);

                List<Claim> claims = new List<Claim>();

                claims.Add(new Claim(ClaimTypes.Email, user.Email));
                claims.Add(new Claim(ClaimTypes.Name, $"{user.FirstName} {user.LastName}"));
                foreach (var role in Roles) 
                {
                    claims.Add(new Claim(ClaimTypes.Role, role));
                }

                var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtSettings.SecurityKey));
                var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
                var expiry = DateTime.Now.AddMinutes(_jwtSettings.ExpiresInMinutes);
                var token = new JwtSecurityToken(_jwtSettings.Issuer, _jwtSettings.Audience, claims, expires:  expiry, signingCredentials: creds);
                string tokenString = new JwtSecurityTokenHandler().WriteToken(token);

                return Ok(new { token = tokenString, Expires = expiry });
            }
            catch
            {
                return StatusCode(500, new { message = "An error occurred while processing the request" });
            }
        }
    }
}
