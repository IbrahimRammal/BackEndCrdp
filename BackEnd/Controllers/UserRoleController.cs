using BackEnd.Class;
using BackEnd.Models;
using BackEnd.Data;
//using DevExtreme.AspNet.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using System.Linq;
using System.Net;
using System.Net.Http.Formatting;


// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace BackEnd.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserRoleController : ControllerBase
    {
        private readonly CrdpCurriculumMsContext _context;
        private readonly IConfiguration _configuration;

        public UserRoleController(CrdpCurriculumMsContext context, IConfiguration configuration)
        {
            _context = context;
            _configuration = configuration;

        }

        [HttpGet("GetAllUserRole")]   
      public async Task<ActionResult<IEnumerable<UserRole>>> etAllUserRole()
        {
            return await _context.UserRoles.ToListAsync();
        }




        [HttpGet("GetAllUserRoleDetailed")]
        public async Task<ActionResult<IEnumerable<dynamic>>> GetAllUserRoleDetailed()
        {
            var result = await (from ur in _context.UserRoles
                                join u in _context.Users on ur.UserId equals u.Id
                                join r in _context.Roles on ur.RoleId equals r.Id into roleGroup
                                from r in roleGroup.DefaultIfEmpty()
                                join urp in _context.UserRolePermissions on ur.Id equals urp.UserRoleId into permissionGroup
                                from urp in permissionGroup.DefaultIfEmpty()
                                join cc in _context.CodesContents on urp.Class equals cc.Id into classGroup
                                from cc in classGroup.DefaultIfEmpty()
                                join cf in _context.CodesContents on urp.ConceptFiled equals cf.Id into conceptGroup
                                from cf in conceptGroup.DefaultIfEmpty()
                                select new
                                {
                                    Id = u.Username,
                                    UserId = u.Id,
                                    Username = $"{u.Username}-{u.Fname} {u.Mname} {u.Lname}",
                                    RoleName = r.RoleName,
                                    Permissions = $"{cc.CodeContentName} : {cf.CodeContentName}"
                                })
                                .ToListAsync();

            // Get all the unique user IDs from the result
            var allUserIds = await _context.Users.Select(u => u.Id).ToListAsync();

            // Check if any users are missing from the result and add them with an empty role and permissions
            foreach (var userId in allUserIds)
            {
                if (!result.Any(x => x.UserId == userId))
                {
                    var user = await _context.Users.FindAsync(userId);
                    result.Add(new
                    {
                        Id = user.Username,
                        UserId = user.Id,
                        Username = $"{user.Username}-{user.Fname} {user.Mname} {user.Lname}",
                        RoleName = "",
                        Permissions = ""
                    });
                }
            }

            return Ok(result);
        }







        // GET api/<UserRoleController>/5
        [HttpGet("GetUserRole/{id}")]
        public async Task<ActionResult<UserRole>> GetUserRole(int id)
        {
            var user = await _context.UserRoles
                .Where(o => o.UserId == id)
                .ToListAsync();

            if (user == null)
            {
                return NotFound();
            }

            return Ok(user);
        }

        [HttpPut("UpdateUserRole")]
        [Consumes("application/x-www-form-urlencoded")]
        public async Task<IActionResult> UpdateUserRole([FromForm] IFormCollection form)
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

                var user = await  _context.UserRoles.FindAsync(id); // Use the converted int here

                // Check if the user exists
                if (user == null)
                {
                    return NotFound("user not found.");
                }

                // Update user values
                JsonConvert.PopulateObject(values, user);

                // Save changes
                await _context.SaveChangesAsync();

                return Ok(new { success = true, message = "user updated successfully." });
            }
            catch (Exception ex)
            {
                // Return an error response in case of an exception
                return StatusCode(500, new { success = false, message = ex.Message });
            }
        }

        [HttpPost("InsertUserRole")]
        [Consumes("application/json")]
        public async Task<ActionResult<UserRole>> InsertUserRole([FromBody] UserRole userRoleDto)
        {
            var user = new UserRole
            {
                UserId = userRoleDto.UserId,
                RoleId = userRoleDto.RoleId
            };

            try
            {
                _context.UserRoles.Add(user);
                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { success = false, message = ex.Message });
            }

            return Ok(user);
        }


        [HttpDelete("DeleteUserRole")]
        [Consumes("application/x-www-form-urlencoded")]
        public async Task<IActionResult> DeleteUserRole([FromForm] IFormCollection form)
        {
            var keyString = form["key"];

            // Parse the key as an integer
            if (!int.TryParse(keyString, out int key))
            {
                // If parsing fails, return a BadRequest response
                return BadRequest("Invalid key format.");
            }

            // Find the UserRole
            var userRole = await _context.UserRoles.FindAsync(key);
            if (userRole == null)
            {
                return NotFound();
            }

            // Delete all associated UserRolePermissions
            var userRolePermissions = await _context.UserRolePermissions.Where(p => p.UserRoleId == key).ToListAsync();
            _context.UserRolePermissions.RemoveRange(userRolePermissions);
            await _context.SaveChangesAsync();

            // Delete the UserRole
            _context.UserRoles.Remove(userRole);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool UserroleExists(int id)
        {
            return _context.UserRoles.Any(e => e.Id == id);
        }


    }
}
