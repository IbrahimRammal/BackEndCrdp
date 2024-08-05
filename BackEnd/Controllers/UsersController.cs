using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using BackEnd.Models;
using Microsoft.AspNetCore.Cryptography.KeyDerivation;
using System.Security.Cryptography;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Newtonsoft.Json;
using BackEnd.Data;
namespace BackEnd.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly CrdpCurriculumMsContext _context;
        private readonly IConfiguration _configuration;
        
        public UsersController(CrdpCurriculumMsContext context, IConfiguration configuration)
        {
            _context = context;
            _configuration = configuration;
           
        }

        // GET: api/Users
        [HttpGet("GetUsers")]
        public async Task<ActionResult<IEnumerable<User>>> GetUsers()
        {
            return await _context.Users.ToListAsync();
        }

        // GET: api/Users/5
        [HttpGet("{id}")]
        public async Task<ActionResult<User>> GetUser(int id)
        {
            var user = await _context.Users.FindAsync(id);

            if (user == null)
            {
                return NotFound();
            }

            return user;
        }

        // PUT: api/Users/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutUser(int id, User user)
        {
            if (id != user.Id)
            {
                return BadRequest();
            }

            _context.Entry(user).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!UserExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }
        [HttpPut("UpdateUser")]
        [Consumes("application/x-www-form-urlencoded")]
        public async Task<bool> UpdateUser([FromForm] IFormCollection form)
        {
            try
            {
                var key = form["key"];
                var values = form["values"];
                var user = _context.Users.First(o => o.Id.Equals(key.ToString()));

                JsonConvert.PopulateObject(values, user);

                //Validate(order);
                //if (!ModelState.IsValid)
                //    return Request.CreateErrorResponse(HttpStatusCode.BadRequest, ModelState.GetFullErrorMessage());

                await _context.SaveChangesAsync();
            }
            catch (Exception)
            {

                return false;
            }
           

            return true;
        }
        // POST: api/Users
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost("InsertUser")]
        [Consumes("application/x-www-form-urlencoded")]
        public async Task<ActionResult<User>> PostUser([FromForm] IFormCollection form)
        {
            var user = new User();
            try
            {
                var key = form["key"];
                var values = form["values"];
               
                JsonConvert.PopulateObject(values, user);

                user.Password = GetHashPassword(user.Password);
                _context.Users.Add(user);
                await _context.SaveChangesAsync();
            }
            catch (Exception)
            {

               
            }
            

            return CreatedAtAction("GetUser", new { id = user.Id }, user);
        }

        // DELETE: api/Users/5
        [HttpDelete("DeactivateUser")]
        [Consumes("application/x-www-form-urlencoded")]
        public async Task<IActionResult> DeleteUser([FromForm] IFormCollection form)
        {
            var key = form["key"];
            var user = await _context.Users.FindAsync(key);
            if (user == null)
            {
                return NotFound();
            }
            user.UserStatus = true;
            //_context.Users.Remove(user);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool UserExists(int id)
        {
            return _context.Users.Any(e => e.Id == id);
        }
        private string GetHashPassword(string password)
        {
            IdentityUser applicationUser = new IdentityUser();
            RandomNumberGenerator _rng =  RandomNumberGenerator.Create();
            return Convert.ToBase64String(HashPasswordV2(password, _rng));
           
        }
        private  byte[] HashPasswordV2(string password, RandomNumberGenerator rng)
        {
            const KeyDerivationPrf Pbkdf2Prf = KeyDerivationPrf.HMACSHA1; // default for Rfc2898DeriveBytes
            const int Pbkdf2IterCount = 1000; // default for Rfc2898DeriveBytes
            const int Pbkdf2SubkeyLength = 256 / 8; // 256 bits
            const int SaltSize = 128 / 8; // 128 bits

            // Produce a version 2 (see comment above) text hash.
            byte[] salt = new byte[SaltSize];
            rng.GetBytes(salt);
            byte[] subkey = KeyDerivation.Pbkdf2(password, salt, Pbkdf2Prf, Pbkdf2IterCount, Pbkdf2SubkeyLength);

            var outputBytes = new byte[1 + SaltSize + Pbkdf2SubkeyLength];
            outputBytes[0] = 0x00; // format marker
            Buffer.BlockCopy(salt, 0, outputBytes, 1, SaltSize);
            Buffer.BlockCopy(subkey, 0, outputBytes, 1 + SaltSize, Pbkdf2SubkeyLength);
            return outputBytes;
        }
        private  bool VerifyHashedPasswordV2(string passwordHashed, string password)
        {
            byte[] hashedPassword = Convert.FromBase64String(passwordHashed);
            const KeyDerivationPrf Pbkdf2Prf = KeyDerivationPrf.HMACSHA1; // default for Rfc2898DeriveBytes
            const int Pbkdf2IterCount = 1000; // default for Rfc2898DeriveBytes
            const int Pbkdf2SubkeyLength = 256 / 8; // 256 bits
            const int SaltSize = 128 / 8; // 128 bits

            // We know ahead of time the exact length of a valid hashed password payload.
            if (hashedPassword.Length != 1 + SaltSize + Pbkdf2SubkeyLength)
            {
                return false; // bad size
            }

            byte[] salt = new byte[SaltSize];
            Buffer.BlockCopy(hashedPassword, 1, salt, 0, salt.Length);

            byte[] expectedSubkey = new byte[Pbkdf2SubkeyLength];
            Buffer.BlockCopy(hashedPassword, 1 + salt.Length, expectedSubkey, 0, expectedSubkey.Length);

            // Hash the incoming password and verify it
            byte[] actualSubkey = KeyDerivation.Pbkdf2(password, salt, Pbkdf2Prf, Pbkdf2IterCount, Pbkdf2SubkeyLength);
            return ByteArraysEqual(actualSubkey, expectedSubkey);
        }
        private  bool ByteArraysEqual(byte[] a, byte[] b)
        {
            if (a == null && b == null)
            {
                return true;
            }
            if (a == null || b == null || a.Length != b.Length)
            {
                return false;
            }
            var areSame = true;
            for (var i = 0; i < a.Length; i++)
            {
                areSame &= (a[i] == b[i]);
            }
            return areSame;
        }
        [HttpPost("Login")]
        public IActionResult Login(Login model)
        {
  
            var user = _context.Users.Where(e => e.Username.Equals(model.UserName)).ToList();

            if (user!=null &&user.Count()>0)
            {
                if (VerifyHashedPasswordV2(user.First().Password, model.Password))
                {
                    var test = _configuration["Jwt:SecretKey"];

                    var token = new JwtSecurityToken(

                        signingCredentials: new SigningCredentials(new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:SecretKey"])), SecurityAlgorithms.HmacSha256),

                     expires: DateTime.Now.AddMinutes(double.Parse(_configuration["jwt:ExpiryMinutes"])),
                     issuer: _configuration["jwt:Issuer"]);

                    return Ok(new
                    {
                        token = new JwtSecurityTokenHandler().WriteToken(token),
                        data= user.First().Username,
                        status = "success"
                    });
                }
            }
            return Unauthorized();
        }
    }
}
