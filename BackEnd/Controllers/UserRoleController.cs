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



            var user = await _context.UserRoles.FindAsync(key);
            if (user == null)
            {
                return NotFound();
            }

            _context.UserRoles.Remove(user);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool UserroleExists(int id)
        {
            return _context.UserRoles.Any(e => e.Id == id);
        }


    }
}
