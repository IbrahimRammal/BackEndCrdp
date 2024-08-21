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
    public class UserRolePermissionController : ControllerBase
    {
        private readonly CrdpCurriculumMsContext _context;
        private readonly IConfiguration _configuration;

        public UserRolePermissionController(CrdpCurriculumMsContext context, IConfiguration configuration)
        {
            _context = context;
            _configuration = configuration;

        }

        [HttpGet("GetAllUserRolePermissions")]   
      public async Task<ActionResult<IEnumerable<UserRolePermission>>> etAllUserRolePermissions()
        {
            return await _context.UserRolePermissions.ToListAsync();
        }


        // GET api/<UserRoleController>/5
        [HttpGet("GetUserRolePermission/{id}")]
        public async Task<ActionResult<UserRolePermission>> GetUserRolePermission(int id)
        {
            var userRolePermission = await _context.UserRolePermissions
                .Where(o => o.UserRoleId == id)
                .ToListAsync();

            if (userRolePermission == null)
            {
                return NotFound();
            }

            return Ok(userRolePermission);
        }

        [HttpPut("UpdateUserRolePermission")]
        [Consumes("application/x-www-form-urlencoded")]
        public async Task<IActionResult> UpdateUserRolePermission([FromForm] IFormCollection form)
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

                var user = await  _context.UserRolePermissions.FindAsync(id); // Use the converted int here

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

        [HttpPost("InsertUserRolePermission")]
        [Consumes("application/json")]
        public async Task<ActionResult<IEnumerable<UserRolePermission>>> InsertUserRole([FromBody] List<UserRolePermission> userRoleDtos)
        {
            var userRolePermissions = new List<UserRolePermission>();
            try
            {
                foreach (var userRoleDto in userRoleDtos)
                {
                    // Check if the UserRolePermission already exists
                    var existingUserRolePermission = await _context.UserRolePermissions.FirstOrDefaultAsync(
                        urp => urp.UserRoleId == userRoleDto.UserRoleId
                            && urp.ConceptFiled == userRoleDto.ConceptFiled
                            && urp.Class == userRoleDto.Class
                    );

                    if (existingUserRolePermission == null)
                    {
                        var user = new UserRolePermission
                        {
                            UserRoleId = userRoleDto.UserRoleId,
                            ConceptFiled = userRoleDto.ConceptFiled,
                            Class = userRoleDto.Class
                        };

                        _context.UserRolePermissions.Add(user);
                        userRolePermissions.Add(user);
                    }
                    else
                    {
                        userRolePermissions.Add(existingUserRolePermission);
                    }
                }

                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { success = false, message = ex.Message });
            }

            return Ok(userRolePermissions);
        }



        [HttpPost("InsertUserRolePermissionBulk")]
        [Consumes("application/json")]
        public async Task<ActionResult> InsertUserRolePermissionBulk(InsertUserRolePermissionBulkRequest request)
        {
            try
            {

                foreach (var userId in request.SelectedUsers)
                {
                    // Check if the user-role association exists
                    var userRole = await _context.UserRoles.FirstOrDefaultAsync(ur => ur.UserId == userId && ur.RoleId == request.SelectedRole);
                    if (userRole == null)
                    {
                        // Insert the user-role association
                        userRole = new UserRole
                        {
                            UserId = userId,
                            RoleId = request.SelectedRole
                        };
                        _context.UserRoles.Add(userRole);
                        await _context.SaveChangesAsync();

                    }

                    // Loop through the selected permissions and insert them if they don't exist
                    foreach (var permission in request.SelectedPermissions)
                    {
                        var userRolePermission = await _context.UserRolePermissions.FirstOrDefaultAsync(
                            urp => urp.UserRoleId == userRole.Id && urp.ConceptFiled == permission.ConceptFiled && urp.Class == permission.Class);
                        if (userRolePermission == null)
                        {
                            userRolePermission = new UserRolePermission
                            {
                                UserRoleId = userRole.Id,
                                ConceptFiled = permission.ConceptFiled,
                                Class = permission.Class
                            };
                            _context.UserRolePermissions.Add(userRolePermission);
                        }
                    }
                }

                await _context.SaveChangesAsync();
                return StatusCode(200, new { success = true, message = "تم تعيين الدور بنجاح." });
            }
            catch (Exception ex)
            {
                return StatusCode(200, new { success = false, message = ex.Message });
            }
        }
        public class InsertUserRolePermissionBulkRequest
        {
            public List<PermissionDto> SelectedPermissions { get; set; }
            public int SelectedRole { get; set; }
            public List<int> SelectedUsers { get; set; }
        }
        public class PermissionDto
        {
            public int Class { get; set; }
            public int ConceptFiled { get; set; }
        }




        [HttpDelete("DeleteUserRolePermission")]
        [Consumes("application/x-www-form-urlencoded")]
        public async Task<IActionResult> DeleteUserRolePermission([FromForm] IFormCollection form)
        {
            var keyString = form["key"];

            // Parse the key as an integer
            if (!int.TryParse(keyString, out int key))
            {
                // If parsing fails, return a BadRequest response
                return BadRequest("Invalid key format.");
            }

            var user = await _context.UserRolePermissions.FindAsync(key);
            if (user == null)
            {
                return NotFound();
            }

            _context.UserRolePermissions.Remove(user);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool UserroleExists(int id)
        {
            return _context.UserRolePermissions.Any(e => e.Id == id);
        }




        [HttpPost("BulkDeleteUserRolePermission")]
        public async Task<IActionResult> BulkDeleteUserRolePermission([FromBody] IEnumerable<int> keys)
        {
            try
            {
                if (keys == null || !keys.Any())
                {
                    return BadRequest("No keys provided.");
                }

                // Fetch the user role permissions based on the provided keys
                var userRolePermissions = await _context.UserRolePermissions.Where(p => keys.Contains(p.Id)).ToListAsync();

                // Remove the user role permissions
                _context.UserRolePermissions.RemoveRange(userRolePermissions);
                await _context.SaveChangesAsync();

                return Ok(new { success = true, message = $"{userRolePermissions.Count} user role permissions deleted successfully." });
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new { success = false, message = "An error occurred while deleting user role permissions." });
            }
        }



    }
}
