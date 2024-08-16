using BackEnd.Data;
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
        [HttpGet("GetCodesContentsById/{id}")]
        public async Task<ActionResult<CodesContent>> GetCodesContentsById(int id)
        {
            var codeContents = await _context.CodesContents.FindAsync(id);

            if (codeContents == null)
            {
                return NotFound();
            }

            return codeContents;
        }

        [HttpGet("GetCodesContentsByCodeId/{id}")]
        public async Task<ActionResult<CodesContent>> GetCodesContentsByCodeId(int id)
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
        public async Task<IActionResult> PutCodesContents(int id, CodesContent codeContents)
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
     
        [HttpPut("UpdateCodesContents")]
        [Consumes("application/x-www-form-urlencoded")]
        public async Task<IActionResult> UpdateCodesContents([FromForm] IFormCollection form)
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

                var code = await _context.CodesContents.FindAsync(id); // Use the converted int here

                // Check if the code exists
                if (code == null)
                {
                    return NotFound("Code not found.");
                }

                // Update code values
                JsonConvert.PopulateObject(values, code);

                // Save changes
                await _context.SaveChangesAsync();

                return Ok(new { success = true, message = "Code updated successfully." });
            }
            catch (Exception ex)
            {
                // Return an error response in case of an exception
                return StatusCode(500, new { success = false, message = ex.Message });
            }
        }

        // POST: api/Codes
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost("InsertCodesContents")]
        [Consumes("application/x-www-form-urlencoded")]
        public async Task<ActionResult<CodesContent>> InsertCodesContents([FromForm] IFormCollection form)
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
            catch (Exception ex)
            {
                return StatusCode(500, new { success = false, message = ex.Message });

            }


            return CreatedAtAction("GetCodesContents", new { id = codeContents.Id }, codeContents);
        }

        // DELETE: api/CodesContents/5
        [HttpDelete("DeleteCodesContents")]
        [Consumes("application/x-www-form-urlencoded")]
        public async Task<IActionResult> DeleteCodesContents([FromForm] IFormCollection form)
        {
            var keyString = form["key"];

            // Parse the key as an integer
            if (!int.TryParse(keyString, out int key))
            {
                // If parsing fails, return a BadRequest response
                return BadRequest("Invalid key format.");
            }



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
