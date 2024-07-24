using BackEnd.Models;
using DevExtreme.AspNet.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Microsoft.VisualBasic;
using Newtonsoft.Json;
using System.Security.Cryptography;

namespace BackEnd.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CodesContentsController : ControllerBase
    {
        private readonly CrdpCurriculumMsContext _context;

        private readonly IConfiguration _configuration;

        public CodesContentsController(CrdpCurriculumMsContext context, IConfiguration configuration)
        {
            _context = context;
            _configuration = configuration;

        }

        // GET: api/CodesContents
        [HttpGet("GetCodesContents")]
        public async Task<ActionResult<IEnumerable<CodesContent>>> GetCodesContents()
        {
            return await _context.CodesContents.ToListAsync();
        }

        // GET: api/CodesContents/5
        [HttpGet("GetCodesContents{id}")]
        public async Task<ActionResult<CodesContent>> GetCodesContents(int id)
        {
            var codeContents = await _context.CodesContents.FindAsync(id);

            if (codeContents == null)
            {
                return NotFound();
            }

            return codeContents;
        }

        [HttpGet("GetCodesContentByCodeId{id}")]
        public async Task<ActionResult<CodesContent>> GetCodesContentByCodeId(int id)
        {

            var codeContents = await _context.CodesContents
               .Where(o => o.CodeId == id)
               .ToListAsync();

            if (codeContents == null)
            {
                return NotFound();
            }

            return Ok(codeContents);
        }



        // PUT: api/CodesContents/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutCodeContents(int id, CodesContent codeContents)
        {
            if (id != codeContents.Id)
            {
                return BadRequest();
            }

            _context.Entry(codeContents).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!CodeContentsExists(id))
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
     
        [HttpPut("UpdateCodeContents")]
        [Consumes("application/x-www-form-urlencoded")]
        public async Task<bool> UpdateCodesContents([FromForm] IFormCollection form)
        {
            try
            {
                var key = form["key"];
                var values = form["values"];
                var codeContents = _context.CodesContents.First(o => o.Id.Equals(key.ToString()));

                JsonConvert.PopulateObject(values, codeContents);

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
        [HttpPost("InsertCodeContents")]
        [Consumes("application/x-www-form-urlencoded")]
        public async Task<ActionResult<CodesContent>> PostCodeContents([FromForm] IFormCollection form)
        {
            var codeContents = new CodesContent();
            try
            {
                var key = form["key"];
                var values = form["values"];

                JsonConvert.PopulateObject(values, codeContents);

          
                _context.CodesContents.Add(codeContents);
                await _context.SaveChangesAsync();
            }
            catch (Exception)
            {


            }


            return CreatedAtAction("GetCodes", new { id = codeContents.Id }, codeContents);
        }

        // DELETE: api/CodesContents/5
        [HttpDelete("DeleteCodeContents")]
        [Consumes("application/x-www-form-urlencoded")]
        public async Task<IActionResult> DeleteCodeContents([FromForm] IFormCollection form)
        {
            var key = form["key"];
            var codeContents = await _context.CodesContents.FindAsync(key);
            if (codeContents == null)
            {
                return NotFound();
            }
            
            _context.CodesContents.Remove(codeContents);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool CodeContentsExists(int id)
        {
            return _context.CodesContents.Any(e => e.Id == id);
        }


    }
}
