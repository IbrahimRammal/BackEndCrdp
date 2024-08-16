using Microsoft.AspNetCore.Identity;

using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using BackEnd.Models;
using System.Security.Claims;
using System.IdentityModel.Tokens.Jwt;
using Microsoft.IdentityModel.Tokens;
using System.Text;

namespace BackEnd.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountController : ControllerBase
    {

        private readonly UserManager<IdentityUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly IConfiguration _configuration;
        public AccountController(UserManager<IdentityUser> userManager, RoleManager<IdentityRole> roleManager, IConfiguration configuration)
        {
            _configuration = configuration;
            _userManager = userManager;
            _roleManager = roleManager;
        }
        [HttpPost("register")]
        public async Task<IActionResult> Register(Register register)
        {
            var user = new IdentityUser { UserName = register.UserName };
            var result = await _userManager.CreateAsync(user, register.Password);
            if (result.Succeeded)
            {
                return Ok(new { message = "User registered Successfully" });
            }
            return BadRequest(result.Errors);
        }

        [HttpPost("Login")]
        public async Task<IActionResult> Login(Login model)
        {
            var user = await _userManager.FindByNameAsync(model.UserName);
            if (user != null && await _userManager.CheckPasswordAsync(user, model.Password))
            {
                var userRoles = await _userManager.GetRolesAsync(user);
                var authclaims = new List<Claim>
                {
                    new Claim(JwtRegisteredClaimNames.Sub, user.UserName),
                    new Claim(JwtRegisteredClaimNames.Jti, user.Id.ToString())

                };
                var test = _configuration["Jwt:SecretKey"];
                authclaims.AddRange(userRoles.Select(role => new Claim(ClaimTypes.Role, role)));
                var token = new JwtSecurityToken(
                    claims: authclaims,
                    signingCredentials: new SigningCredentials(new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:SecretKey"])), SecurityAlgorithms.HmacSha256),

                 expires: DateTime.Now.AddMinutes(double.Parse(_configuration["jwt:ExpiryMinutes"])),
                 issuer: _configuration["jwt:Issuer"]);

                return Ok(new { accessToken = new JwtSecurityTokenHandler().WriteToken(token), status =
            "ok" });
            }
            return Unauthorized();
        }
        [HttpPost("add-role")]
        public async Task<IActionResult> AddRole(string role)

        {
            if (!await _roleManager.RoleExistsAsync(role))
            {
                var result = await _roleManager.CreateAsync(new IdentityRole(role));
                if (result.Succeeded)
                {
                    return Ok(new { message = "Role Added Successfully" });
                }
                return BadRequest(result.Errors);

            }
            return BadRequest("Role already Exists");

        }

        [HttpPost("Assign-role")]
        public async Task<IActionResult> AssignRole(UserRolePermission userRole)

        {
            var user = await _userManager.FindByIdAsync(userRole.Id.ToString());
            if (user == null)
            {
                return BadRequest("User not Found");

            }
            var result = await _userManager.AddToRoleAsync(user, userRole.UserRole.ToString());
            if (result.Succeeded)
            {
                return Ok(new { message = "Role assigned Successfully" });

            }
            return BadRequest(result.Errors);
        }
    }
}


