using BackEnd.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using System.Security.Cryptography;

namespace BackEnd.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CodesController : ControllerBase
    {
        private readonly CrdpCurriculumMsContext _context;
    
        private readonly IConfiguration _configuration;

        public CodesController(CrdpCurriculumMsContext context, IConfiguration configuration)
        {
            _context = context;
            _configuration = configuration;

        }

        // GET: api/Codes
        [HttpGet("GetCodes")]
        public async Task<ActionResult<IEnumerable<Code>>> GetCodes()
        {
            return await _context.Codes.ToListAsync();
        }

        // GET: api/Codes/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Code>> GetCodes(int id)
        {
            var code = await _context.Codes.FindAsync(id);

            if (code == null)
            {
                return NotFound();
            }

            return code;
        }

        // PUT: api/Codes/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutCode(int id, Code code)
        {
            if (id != code.Id)
            {
                return BadRequest();
            }

            _context.Entry(code).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!CodeExists(id))
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
        [HttpPut("UpdateCode")]
        [Consumes("application/x-www-form-urlencoded")]
        public async Task<bool> UpdateCode([FromForm] IFormCollection form)
        {
            try
            {
                var key = form["key"];
                var values = form["values"];
                var code = _context.Codes.First(o => o.Id.Equals(key.ToString()));

                JsonConvert.PopulateObject(values, code);

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
        // POST: api/Codes
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost("InsertCode")]
        [Consumes("application/x-www-form-urlencoded")]
        public async Task<ActionResult<Code>> PostCode([FromForm] IFormCollection form)
        {
            var code = new Code();
            try
            {
                var key = form["key"];
                var values = form["values"];

                JsonConvert.PopulateObject(values, code);

          
                _context.Codes.Add(code);
                await _context.SaveChangesAsync();
            }
            catch (Exception)
            {


            }


            return CreatedAtAction("GetCodes", new { id = code.Id }, code);
        }

        // DELETE: api/Codes/5
        [HttpDelete("DeleteCode")]
        [Consumes("application/x-www-form-urlencoded")]
        public async Task<IActionResult> DeleteCode([FromForm] IFormCollection form)
        {
            var key = form["key"];
            var code = await _context.Codes.FindAsync(key);
            if (code == null)
            {
                return NotFound();
            }
            
            _context.Codes.Remove(code);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool CodeExists(int id)
        {
            return _context.Codes.Any(e => e.Id == id);
        }


    }
}
