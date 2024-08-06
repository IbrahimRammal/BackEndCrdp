using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using BackEnd.Models;
using Newtonsoft.Json;
using BackEnd.Data;

namespace BackEnd.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ServicesController : ControllerBase
    {
        private readonly CrdpCurriculumMsContext _context;

        public ServicesController(CrdpCurriculumMsContext context)
        {
            _context = context;
        }

        // GET: api/Services
        [HttpGet("GetServices")]
        public async Task<ActionResult<IEnumerable<Service>>> GetServices()
        {
            var result = from d in _context.Services
                      
                        select new Service
                        {
                           Id = d.Id,
                           Dependencies = d.Dependencies,   
                           HasChildren = d.HasChildren,
                           Parent = d.Parent,
                           ServiceName = d.ServiceName, 
                           Svurl = d.Svurl,
                           Title   = d.Title,
                           Clurl = d.Clurl,
                            ParentId = d.Parent.Equals(d.ServiceName)?0: _context.Services.FirstOrDefault(x => x.ServiceName.Trim().Equals(d.Parent.Trim())).Id
                        };
            return await result.ToListAsync();
        }

        // GET: api/Services/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Service>> GetService(int id)
        {
            var service = await _context.Services.FindAsync(id);

            if (service == null)
            {
                return NotFound();
            }

            return service;
        }

        // PUT: api/Services/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutService(int id, Service service)
        {
            if (id != service.Id)
            {
                return BadRequest();
            }

            _context.Entry(service).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ServiceExists(id))
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

        // POST: api/Services
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Service>> PostService(Service service)
        {
            _context.Services.Add(service);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetService", new { id = service.Id }, service);
        }

        // DELETE: api/Services/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteService(int id)
        {
            var service = await _context.Services.FindAsync(id);
            if (service == null)
            {
                return NotFound();
            }

            _context.Services.Remove(service);
            await _context.SaveChangesAsync();

            return NoContent();
        }
        [HttpPost("InsertServices")]
        [Consumes("application/x-www-form-urlencoded")]
        public async Task<ActionResult<Service>> PostUser([FromForm] IFormCollection form)
        {
            var service = new Service();
            try
            {
                var key = form["key"];
                var values = form["values"];

                JsonConvert.PopulateObject(values, service);

                service.Parent = _context.Services.FirstOrDefault(x => x.Id == service.ParentId)?.Parent;
                _context.Services.Add(service);
                await _context.SaveChangesAsync();
            }
            catch (Exception)
            {


            }


            return CreatedAtAction("GetServices", new { id = service.Id }, service);
        }


        [HttpPut("UpdateServices")]
        [Consumes("application/x-www-form-urlencoded")]
        public async Task<bool> UpdateServices([FromForm] IFormCollection form)
        {
            try
            {
                var key = form["key"];
                var values = form["values"];
                var service = _context.Services.First(o => o.Id.Equals(key.ToString()));

                JsonConvert.PopulateObject(values, service);
                service.Parent = _context.Services.FirstOrDefault(x => x.Id == service.ParentId)?.Parent;
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
        [HttpDelete("DeactivateServices")]
        [Consumes("application/x-www-form-urlencoded")]
        public async Task<IActionResult> DeleteUser([FromForm] IFormCollection form)
        {
            var key = form["key"];
            var service = await _context.Services.FindAsync(Convert.ToInt32(key));
            if (service == null)
            {
                return NotFound();
            }
            _context.Remove(service);
           
            //_context.Users.Remove(user);
            await _context.SaveChangesAsync();

            return NoContent();
        }
        private bool ServiceExists(int id)
        {
            return _context.Services.Any(e => e.Id == id);
        }


    }
}
