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
using BackEnd.Class;
using Microsoft.VisualStudio.Web.CodeGenerators.Mvc.Templates.BlazorIdentity.Pages.Manage;
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
        public async Task<IActionResult> UpdateUser([FromForm] IFormCollection form)
        {
            try
            {
                var key = form["key"];
                var values = form["values"];

                // Convert the key to int before using it to find the entity
                if (!int.TryParse(key, out int id))
                {
                    return BadRequest("Invalid key format.");
                }

                var user = await _context.Users.FindAsync(id); // Use the converted int here

                // Check if the concept exists
                if (user == null)
                {
                    return NotFound("concept not found.");
                }

                // Update concept values
                JsonConvert.PopulateObject(values, user);

                // Save changes
                await _context.SaveChangesAsync();
                await _context.Entry(user).ReloadAsync();
                return Ok(new { success = true, message = "concept updated successfully." });
            }
            catch (Exception ex)
            {
                // Return an error response in case of an exception
                return StatusCode(500, new { success = false, message = ex.Message });
            }
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


        //By Kamel Nazar
        [HttpGet("grouped-by-workgroup")]
        public async Task<ActionResult<GroupedUsersResponse>> GetGroupedUsers()
        {
            try
            {
                // Get the grouped users
                var groupedUsers = await _context.Users
                    .Where(u => u.WorkGroup.HasValue)
                    .GroupBy(u => u.WorkGroup)
                    .Select(g => new GroupedUsersDto
                    {
                        GroupName = $"WorkGroup {g.Key}",
                        Users = g.Select(u => new UserDto
                        {
                            Id = u.Id,
                            Username = u.Username,
                            Fname = u.Fname,
                            Mname = u.Mname,
                            Lname = u.Lname,
                            PhoneNb = u.PhoneNb,
                            Email = u.Email
                        }).ToList()
                    }).ToListAsync();

                // Calculate the total number of workgroups
                int workGroupCount = groupedUsers.Count;

                // Prepare the response
                var response = new GroupedUsersResponse
                {
                    WorkGroupCount = workGroupCount,
                    GroupedUsers = groupedUsers
                };

                return Ok(response);
            }
            catch (Exception ex)
            {
                // Log exception for debugging
                Console.WriteLine($"Exception: {ex.Message}");
                return StatusCode(500, "Internal server error");
            }
        }


        //mohamadbaydoun
        [HttpPost("ChangePassword")]
        public async Task<ActionResult<ChangePassword>> ChangePassword([FromBody] ChangePasswordRequest request)
        {
            var ChangePasswordRequestResult = new ChangePasswordRequestResult();
            try
            {
                var user = await _context.Users.FindAsync(request.Id);
                if (user == null)
                {
                    return NotFound();
                }

                user.Password = GetHashPassword(request.Password);
                await _context.SaveChangesAsync();
                ChangePasswordRequestResult.Success = true;
                return Ok(ChangePasswordRequestResult);
            }
            catch (Exception ex)
            {

                // Return an appropriate HTTP response
                if (ex is ArgumentException)
                {
                    return BadRequest(ex.Message);
                }
                else
                {
                    return StatusCode(StatusCodes.Status500InternalServerError, "An error occurred while changing the password.");
                }
            }
        }
        public class ChangePasswordRequest
        {
            public int Id { get; set; }
            public string Password { get; set; }
        }

        public class ChangePasswordRequestResult
        {
            public bool Success { get; set; }
        }




        //mohamadbaydoun
        [HttpPost("BulkInsertUsers")]
        [Consumes("application/json")]
        public async Task<ActionResult<BulkOperationResult>> BulkInsertUsers([FromBody] List<User> users)
        {
            var bulkOperationResult = new BulkOperationResult();

            try
            {
                foreach (var user in users)
                {
                    var existingUser = await _context.Users.FirstOrDefaultAsync(u => u.Username == user.Username);

                    if (existingUser != null)
                    {
                        // Update existing user
                        existingUser.Fname = user.Fname;
                        existingUser.Mname = user.Mname;
                        existingUser.Lname = user.Lname;
                        existingUser.PhoneNb = user.PhoneNb;
                        existingUser.Email = user.Email;
                        existingUser.Password = GetHashPassword(user.Password);
                        existingUser.Details = user.Details;
                        existingUser.UserStatus = user.UserStatus;
                        existingUser.WorkGroup = user.WorkGroup;

                        _context.Users.Update(existingUser);
                        bulkOperationResult.UsersUpdated++;
                    }
                    else
                    {
                        // Insert new user
                        user.Password = GetHashPassword(user.Password);
                        _context.Users.Add(user);
                        bulkOperationResult.UsersInserted++;
                    }
                }

                await _context.SaveChangesAsync();

                bulkOperationResult.Success = true;
                bulkOperationResult.Message = "تمت العملية بنجاح.";
                return Ok(bulkOperationResult);
            }
            catch (Exception ex)
            {
                bulkOperationResult.Success = false;
                bulkOperationResult.Message = "حدث خطأ أثناء تنفيذ العملية.";
                return BadRequest(bulkOperationResult);
            }
        }

        public class BulkOperationResult
        {
            public bool Success { get; set; }
            public string Message { get; set; }
            public int UsersInserted { get; set; }
            public int UsersUpdated { get; set; }
        }






    }




}
