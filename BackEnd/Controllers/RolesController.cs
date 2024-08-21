using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using BackEnd.Data;
using BackEnd.Models;

namespace BackEnd.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RolesController : ControllerBase
    {
        private readonly CrdpCurriculumMsContext _context;

        public RolesController(CrdpCurriculumMsContext context)
        {
            _context = context;
        }

        // GET: api/Roles
        [HttpGet("GetRoles")]
        public async Task<ActionResult<IEnumerable<dynamic>>> GetRoles()
        {


            var user = HttpContext.User;
            var userIdClaim = user.Claims.FirstOrDefault(c => c.Type == "userId");
            var roles = await _context.Roles
                .Select(r => new
                {
                    r.Id,
                    r.RoleName,
                    r.RoleDetails,
                    r.DateModified,
                    r.UserModified,
                    r.DateCreated,
                    r.UserCreated,
                    ServiceNames = string.Join("\n", r.RoleServices.Select(rs => rs.Service.ServiceName)),
                    ServiceKeys = r.RoleServices.Select(rs => rs.ServiceId).ToArray()

                })
                .ToListAsync();

            return roles;
        }


        // GET: api/Roles/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Role>> GetRole(int id)
        {
            var role = await _context.Roles.FindAsync(id);

            if (role == null)
            {
                return NotFound();
            }

            return role;
        }

        // PUT: api/Roles/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutRole(int id, Role role)
        {
            if (id != role.Id)
            {
                return BadRequest();
            }

            _context.Entry(role).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!RoleExists(id))
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

        // POST: api/Roles
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Role>> PostRole(Role role)
        {
            _context.Roles.Add(role);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetRole", new { id = role.Id }, role);
        }


        // DELETE: api/Roles/5 [FromForm] IFormCollection form   ;
        [HttpDelete]
        public async Task<IActionResult> DeleteRole([FromForm] IFormCollection form)
        {
            var key = form["key"];
            if (!int.TryParse(key, out int id))
            {
                return BadRequest("Invalid key format.");
            }

            var role = await _context.Roles.FindAsync(id);
            if (role == null)
            {
                return NotFound();
            }

            // Check if there are any UserRoles associated with this Role
            var userRoles = await _context.UserRoles.Where(ur => ur.RoleId == id).ToListAsync();
            if (userRoles.Any())
            {
                var userCount = userRoles.Count;
                var message = $"لا يمكن حذف هذا الدور. هناك {userCount} مستخدم/مستخدمين مرتبطين بهذا الدور.";
                return StatusCode(StatusCodes.Status409Conflict, message);
            }

            // Delete all associated RoleServices
            var roleServices = await _context.RoleServices.Where(rs => rs.RoleId == id).ToListAsync();
            _context.RoleServices.RemoveRange(roleServices);
            await _context.SaveChangesAsync();

            // Delete the Role
            _context.Roles.Remove(role);
            await _context.SaveChangesAsync();

            return NoContent();
        }









        [HttpPost("EditRole")]
        public async Task<IActionResult> EditRole([FromBody] EditRoleRequest editRoleRequest)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var role = await _context.Roles.FindAsync(editRoleRequest.Key);
            if (role == null)
            {
                return StatusCode(200, new { success = false, message = "حدث خطأ" });

            }

            role.RoleName = editRoleRequest.NameRole;

            // Remove existing RoleServices
            var existingRoleServices = _context.RoleServices.Where(rs => rs.RoleId == role.Id);
            _context.RoleServices.RemoveRange(existingRoleServices);

            // Add new RoleServices based on the provided "rows"
            var newRoleServices = editRoleRequest.Rows.Select(serviceId => new RoleService
            {
                RoleId = role.Id,
                ServiceId = serviceId,
                CanView = true,
                CanEdit = true,
                CanDelete = true,
                DateCreated = DateTime.UtcNow,
                UserCreated = 1 // Replace with the actual user ID
            }).ToList();

            _context.RoleServices.AddRange(newRoleServices);

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!RoleExists(editRoleRequest.Key))
                {
                    return StatusCode(200, new { success = false, message = "حدث خطأ " });
                }
                else
                {
                    throw;
                }
            }

            return StatusCode(200, new { success = true, message = "تم تعديل الدور بنجاح" });
        }
        public class EditRoleRequest
        {
            public int Key { get; set; }
            public string NameRole { get; set; }
            public List<int> Rows { get; set; }
        }








        private bool RoleExists(int id)
        {
            return _context.Roles.Any(e => e.Id == id);
        }
    }
}
