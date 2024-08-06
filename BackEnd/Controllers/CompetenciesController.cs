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
    public class CompetenciesController : ControllerBase
    {
        private readonly CrdpCurriculumMsContext _context;

        private readonly IConfiguration _configuration;

        public CompetenciesController(CrdpCurriculumMsContext context, IConfiguration configuration)
        {
            _context = context;
            _configuration = configuration;

        }

        // GET: api/Competencies
        [HttpGet("GetCompetencies")]
        public async Task<ActionResult<IEnumerable<Competencies>>> GetCompetencies()
        {
            return await _context.Competencies.ToListAsync();
        }

        // GET: api/Competencies/5
        [HttpGet("GetCompetenciesById{id}")]
        public async Task<ActionResult<Competencies>> GetCompetenciesById(int id)
        {
            var Competencies = await _context.Competencies.FindAsync(id);

            if (Competencies == null)
            {
                return NotFound();
            }

            return Competencies;
        }



        // PUT: api/Competencies/5
        [HttpPut("UpdateCompetencies")]
        [Consumes("application/x-www-form-urlencoded")]
        public async Task<IActionResult> PutCompetencies([FromForm] IFormCollection form)
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

                var competence = await _context.Competencies.FindAsync(id); // Use the converted int here

                // Check if the Competencies exists
                if (competence == null)
                {
                    return NotFound("Competencies not found.");
                }

                // Update Competencies values
                JsonConvert.PopulateObject(values, competence);

                // Save changes
                await _context.SaveChangesAsync();

                return Ok(new { success = true, message = "Competencies updated successfully." });
            }
            catch (Exception ex)
            {
                // Return an error response in case of an exception
                return StatusCode(500, new { success = false, message = ex.Message });
            }
        }
     

        // POST: api/Competencies
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost("InsertCompetencies")]
        [Consumes("application/x-www-form-urlencoded")]
        public async Task<ActionResult<Competencies>> PostCompetencies([FromForm] IFormCollection form)
        {
            var Competencies = new Competencies();
            try
            {
                var key = form["key"];
                var values = form["values"];

                JsonConvert.PopulateObject(values, Competencies);

          
                _context.Competencies.Add(Competencies);
                await _context.SaveChangesAsync();
            }
            catch (Exception)
            {


            }


            return CreatedAtAction("GetCompetencies", new { id = Competencies.Id }, Competencies);
        }

        // DELETE: api/Competencies/5
        //[HttpDelete("DeleteCompetencies")]
        //[Consumes("application/x-www-form-urlencoded")]
        //public async Task<IActionResult> DeleteCompetencies([FromForm] IFormCollection form)
        //{
        //    var key = form["key"];
        //    var Competencies = await _context.Competencies.FindAsync(key);
        //    if (Competencies == null)
        //    {
        //        return NotFound();
        //    }

        //    _context.Competencies.Remove(Competencies);
        //    await _context.SaveChangesAsync();

        //    return NoContent();
        //}


        // DELETE: api/Competencies/5
        [HttpDelete("DeleteCompetencies")]
        [Consumes("application/x-www-form-urlencoded")]
        public async Task<IActionResult> DeleteCompetencies([FromForm] IFormCollection form)
        {
            var keyString = form["key"];
            // Parse the key as an integer
            if (!int.TryParse(keyString, out int key))
            {
                // If parsing fails, return a BadRequest response
                return BadRequest("Invalid key format.");
            }



            var competence = await _context.Competencies.FindAsync(key);
            if (competence == null)
            {
                return NotFound();
            }

            _context.Competencies.Remove(competence);
            await _context.SaveChangesAsync();

            return NoContent();
        }


        private bool CompetenciesExists(int id)
        {
            return _context.Competencies.Any(e => e.Id == id);
        }


    }
}
