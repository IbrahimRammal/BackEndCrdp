using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using BackEnd.Models;
using BackEnd.Class;
using System.Collections;
using System.Data;
using NuGet.Packaging;

namespace BackEnd.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RoleServicesController : ControllerBase
    {
        private readonly CrdpCurriculumMsContext _context;

        public RoleServicesController(CrdpCurriculumMsContext context)
        {
            _context = context;
        }

        // GET: api/RoleServices
        [HttpGet]
        public async Task<ActionResult<IEnumerable<RoleService>>> GetRoleServices()
        {
            return await _context.RoleServices.ToListAsync();
        }

        // GET: api/RoleServices/5
        [HttpGet("{id}")]
        public async Task<ActionResult<RoleService>> GetRoleService(int id)
        {
            var roleService = await _context.RoleServices.FindAsync(id);

            if (roleService == null)
            {
                return NotFound();
            }

            return roleService;
        }

        // PUT: api/RoleServices/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutRoleService(int id, RoleService roleService)
        {
            if (id != roleService.Id)
            {
                return BadRequest();
            }

            _context.Entry(roleService).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!RoleServiceExists(id))
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

        // POST: api/RoleServices
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<RoleService>> PostRoleService(RoleService roleService)
        {
            _context.RoleServices.Add(roleService);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetRoleService", new { id = roleService.Id }, roleService);
        }

        // DELETE: api/RoleServices/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteRoleService(int id)
        {
            var roleService = await _context.RoleServices.FindAsync(id);
            if (roleService == null)
            {
                return NotFound();
            }

            _context.RoleServices.Remove(roleService);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool RoleServiceExists(int id)
        {
            return _context.RoleServices.Any(e => e.Id == id);
        }

      
        [HttpPost("AddRole")]
        public  IActionResult Post(RoleModel roleModel)
        {

            try
            {

          
            if (roleModel != null)
            {
                Role role = new Role();
                role.RoleName = roleModel.nameRole;
                _context.Roles.Add(role);
                _context.SaveChanges();
                List<Service> services = _context.Services.ToList();
                List<RoleService> roleServices = new List<RoleService>();
               
                foreach (var row in roleModel.rows)
                {
                    var service= services.FirstOrDefault(x=>x.Id==row);
                    if(service != null)
                    { 
                        if(service.HasChildren==true)
                        {
                            var allChildRoles = services.Where(x => x.Parent.Equals(service.ServiceName))
                           .Select(p => new RoleService()
                           {
                               ServiceId = p.Id,
                               RoleId= role.Id
                           });
                            roleServices.AddRange(allChildRoles.ToList());
                        }
                        else
                        {
                                if (service.Dependencies != null)
                                {
                                    //     var dependencies = services.Where(x => x.ServiceName.Equals(service.Dependencies))
                                    //.Select(p => new RoleService()
                                    //{
                                    //    ServiceId = p.Id,
                                    //    RoleId = role.Id
                                    //});

                                    getRecursiverole(service, role.Id, ref roleServices);
                                }
                                else
                                {
                                    RoleService roleService = new RoleService() { RoleId = role.Id, ServiceId = service.Id };
                                    if (roleServices.FindIndex(x => x.RoleId == roleService.Id && x.ServiceId == roleService.ServiceId) < 0)
                                        roleServices.Add(roleService);
                                }
                        }
                    }
                }
                   
                    _context.RoleServices.AddRange(roleServices);
                    _context.SaveChanges();
                }
            }
            catch (Exception)
            {
                return BadRequest(new { data = "Role failed to add" });
            }
            // Process the received data
            return Ok(new { data = "Role Added Successfully" });
        }
        private List<RoleService> getRecursiverole(Service service, int roleId,ref List<RoleService> roleServices)
        {
            
            if (service == null)
            {
                 //roleServices.Add(new RoleService() { RoleId = roleId, ServiceId = service.Id });
                return roleServices;
            }
            else {
                RoleService roleService = new RoleService() { RoleId = roleId, ServiceId = service.Id };
                if (roleServices.FindIndex(x=>x.RoleId==roleService.Id&&x.ServiceId==roleService.ServiceId)<0)
                roleServices.Add(roleService);
                return getRecursiverole(_context.Services.FirstOrDefault(x => x.ServiceName.Equals(service.Dependencies)), roleId,ref roleServices);
               }
        }
    }
}
