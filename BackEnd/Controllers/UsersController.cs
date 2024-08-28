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
using BackEnd.Helper;
using Microsoft.CodeAnalysis.Scripting;
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
            RandomNumberGenerator _rng = RandomNumberGenerator.Create();
            return Convert.ToBase64String(HashPasswordV2(password, _rng));

        }
        private byte[] HashPasswordV2(string password, RandomNumberGenerator rng)
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
        private bool VerifyHashedPasswordV2(string passwordHashed, string password)
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
        private bool ByteArraysEqual(byte[] a, byte[] b)
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
        public IActionResult Login([FromBody] Login model)
        {
            var user = _context.Users
                               .Where(e => e.Username.Equals(model.UserName))
                               .FirstOrDefault();

            if (user != null && VerifyHashedPasswordV2(user.Password, model.Password))
            {
                // Create claims including userId
                var claims = new List<Claim>
        {
            new Claim("userId", user.Id.ToString())
        };

                var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:SecretKey"]));
                var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

                var token = new JwtSecurityToken(
                    issuer: _configuration["Jwt:Issuer"],
                    audience: _configuration["Jwt:Audience"],
                    claims: claims,
                    expires: DateTime.Now.AddMinutes(double.Parse(_configuration["Jwt:ExpiryMinutes"])),
                    signingCredentials: creds
                );

                // Prepare the user data to be returned
                var userData = new
                {
                    user.Id,
                    user.Username,
                    user.Fname,
                    user.Mname,
                    user.Lname,
                    user.PhoneNb,
                    user.Email,
                    user.Details,
                    user.UserStatus,
                    user.WorkGroup,
                    Roles = user.UserRoles.Select(ur => new
                    {
                        ur.Role?.Id,
                        ur.Role?.RoleName,
                        ur.Role?.RoleDetails
                    }).ToList()
                };

                return Ok(new
                {
                    token = new JwtSecurityTokenHandler().WriteToken(token),
                    data = userData,
                    status = "success"
                });
            }

            return Unauthorized();
        }


        [HttpGet("grouped-by-workgroup-id")]
        public async Task<ActionResult<GroupedUsersResponse>> GetGroupedUsers(int? groupId = null)
        {
            try
            {
                // Fetch grouped users and join with CodesContent to get the group names
                var groupedUsers = await (from user in _context.Users
                                          join codeContent in _context.CodesContents
                                          on user.WorkGroup equals codeContent.Id
                                          where (user.WorkGroup.HasValue && user.WorkGroup == groupId)
                                          group new { user, codeContent } by codeContent.CodeContentName into g
                                          select new GroupedUsersDto
                                          {
                                              GroupName = g.Key, // Group name from CodesContent
                                              Users = g.Select(x => new UserDto
                                              {
                                                  Id = x.user.Id,
                                                  Username = x.user.Username,
                                                  Fname = x.user.Fname,
                                                  Mname = x.user.Mname,
                                                  Lname = x.user.Lname,
                                                  PhoneNb = x.user.PhoneNb,
                                                  Email = x.user.Email
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




        //By Kamel Nazar
        [HttpGet("grouped-by-workgroup")]
        public async Task<ActionResult<GroupedUsersResponse>> GetGroupedUsers()
        {

            try
            {
                // Fetch grouped users and join with CodesContent to get the group names
                var groupedUsers = await (from user in _context.Users
                                          join codeContent in _context.CodesContents
                                          on user.WorkGroup equals codeContent.Id
                                          where user.WorkGroup.HasValue
                                          group new { user, codeContent } by codeContent.CodeContentName into g
                                          select new GroupedUsersDto
                                          {
                                              GroupName = g.Key, // Group name from CodesContent
                                              Users = g.Select(x => new UserDto
                                              {
                                                  Id = x.user.Id,
                                                  Username = x.user.Username,
                                                  Fname = x.user.Fname,
                                                  Mname = x.user.Mname,
                                                  Lname = x.user.Lname,
                                                  PhoneNb = x.user.PhoneNb,
                                                  Email = x.user.Email
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
        // PUT: api/Users
        [HttpPut("UpdateUser")]
        public async Task<IActionResult> UpdateUser([FromBody] User user)
        {
            try
            {
                var token = Request.Headers["Authorization"].FirstOrDefault();
                if (token == null)
                {
                    return Ok("No token");
                }
                var jwtHelper = new JwtHelper(_context, _configuration);
                if (string.IsNullOrEmpty(token))
                {
                    return Unauthorized("Token is missing.");
                }

                var userInfo = await jwtHelper.GetUserInfoFromJwt(token, Request.Path);
                if (userInfo == null || userInfo.UserId == null)
                {
                    return NotFound("User information not found in the token.");
                }

                // Extract user ID from the token
                int userId = userInfo.UserId.Value;
                if (userId != user.Id)
                {
                    return BadRequest("User ID mismatch.");
                }

                // Find the existing user in the database
                var existingUser = await _context.Users.FindAsync(userId);
                if (existingUser == null)
                {
                    return NotFound("User not found in the database.");
                }

                // Update user fields
                existingUser.Username = user.Username;
                existingUser.Fname = user.Fname;
                existingUser.Mname = user.Mname;
                existingUser.Lname = user.Lname;
                existingUser.PhoneNb = user.PhoneNb;
                existingUser.Email = user.Email;
                existingUser.UserStatus = user.UserStatus;
                existingUser.WorkGroup = user.WorkGroup;

                // Save changes to the database
                await _context.SaveChangesAsync();

                return NoContent();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error in UpdateUser: {ex.Message}");
                return StatusCode(500, "Internal server error.");
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
       

        [HttpPost("ChangeOldPassword")]
        public async Task<ActionResult<ChangePassword>> ChangeOldPassword([FromBody] ChangePasswordRequest request)
        {
            var ChangePasswordRequestResult = new ChangePasswordRequestResult();
            try
            {
                var token = Request.Headers["Authorization"].FirstOrDefault();
                if (token == null)
                {
                    return Ok("No token");
                }
                var jwtHelper = new JwtHelper(_context, _configuration);
                if (string.IsNullOrEmpty(token))
                {
                    return Unauthorized("Token is missing.");
                }

                var userInfo = await jwtHelper.GetUserInfoFromJwt(token, Request.Path);
                if (userInfo == null || userInfo.UserId == null)
                {
                    return NotFound("User information not found in the token.");
                }

                // Extract user ID from the token
                int userId = userInfo.UserId.Value;
                

                var user = await _context.Users.FindAsync(userId);
                if (user == null)
                {
                    return NotFound();
                }
                var previewHashed = GetHashPassword(request.PreviousPassword);
                var newPasswordHashed = GetHashPassword(request.NewPassword);

                // Check if the previous password is correct
                if (!VerifyHashedPasswordV2(user.Password,previewHashed))
                {
                    return BadRequest("Previous password is incorrect.");
                }


                // Update the password
                user.Password = newPasswordHashed;
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
            public string? Password { get; set; }
            public string? PreviousPassword { get; set; }
            public string? NewPassword { get; set; }

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
