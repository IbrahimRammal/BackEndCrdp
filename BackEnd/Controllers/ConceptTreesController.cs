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
    public class ConceptTreesController : ControllerBase
    {
        private readonly CrdpCurriculumMsContext _context;

        private readonly IConfiguration _configuration;

        public ConceptTreesController(CrdpCurriculumMsContext context, IConfiguration configuration)
        {
            _context = context;
            _configuration = configuration;

        }

        // GET: api/ConceptTrees
        [HttpGet("GetConceptTrees")]
        public async Task<ActionResult<IEnumerable<ConceptTree>>> GetConceptTrees()
        {
            return await _context.ConceptTrees.ToListAsync();
        }

        // GET: api/ConceptTrees/5
        [HttpGet("GetConceptTreesById{id}")]
        public async Task<ActionResult<ConceptTree>> GetConceptTreesById(int id)
        {
            var ConceptTrees = await _context.ConceptTrees.FindAsync(id);

            if (ConceptTrees == null)
            {
                return NotFound();
            }

            return ConceptTrees;
        }



        // PUT: api/ConceptTrees/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("UpdateConceptTrees")]
        [Consumes("application/x-www-form-urlencoded")]
        public async Task<IActionResult> PutConceptTrees([FromForm] IFormCollection form)
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

                var concept = await _context.ConceptTrees.FindAsync(id); // Use the converted int here

                // Check if the concept exists
                if (concept == null)
                {
                    return NotFound("concept not found.");
                }

                // Update concept values
                JsonConvert.PopulateObject(values, concept);

                // Save changes
                await _context.SaveChangesAsync();

                return Ok(new { success = true, message = "concept updated successfully." });
            }
            catch (Exception ex)
            {
                // Return an error response in case of an exception
                return StatusCode(500, new { success = false, message = ex.Message });
            }
       
        }
     

        // POST: api/ConceptTrees
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost("InsertConceptTrees")]
        [Consumes("application/x-www-form-urlencoded")]
        public async Task<ActionResult<ConceptTree>> PostConceptTrees([FromForm] IFormCollection form)
        {
            var ConceptTrees = new ConceptTree();
            try
            {
                var key = form["key"];
                var values = form["values"];

                JsonConvert.PopulateObject(values, ConceptTrees);

          
                _context.ConceptTrees.Add(ConceptTrees);
                await _context.SaveChangesAsync();
            }
            catch (Exception)
            {


            }


            return CreatedAtAction("GetConceptTrees", new { id = ConceptTrees.Id }, ConceptTrees);
        }
        // DELETE: api/ConceptTrees/5
        [HttpDelete("DeleteConceptTrees")]
        [Consumes("application/x-www-form-urlencoded")]
        public async Task<IActionResult> DeleteConceptTrees([FromForm] IFormCollection form)
        {
            var keyString = form["key"];

            // Parse the key as an integer
            if (!int.TryParse(keyString, out int key))
            {
                // If parsing fails, return a BadRequest response
                return BadRequest("Invalid key format.");
            }



            var concept = await _context.ConceptTrees.FindAsync(key);
            if (concept == null)
            {
                return NotFound();
            }

            _context.ConceptTrees.Remove(concept);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool ConceptTreesExists(int id)
        {
            return _context.ConceptTrees.Any(e => e.Id == id);
        }


    }
}
