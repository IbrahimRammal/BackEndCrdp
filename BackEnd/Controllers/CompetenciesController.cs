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
        [HttpGet("GetCompetencies{id}")]
        public async Task<ActionResult<Competencies>> GetCompetencies(int id)
        {
            var Competencies = await _context.Competencies.FindAsync(id);

            if (Competencies == null)
            {
                return NotFound();
            }

            return Competencies;
        }



        // PUT: api/Competencies/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("UpdateCompetencies{id}")]
        public async Task<IActionResult> PutCompetencies(int id, Competencies Competencies)
        {
            if (id != Competencies.Id)
            {
                return BadRequest();
            }

            _context.Entry(Competencies).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!CompetenciesExists(id))
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
        [HttpDelete("DeleteCompetencies")]
        [Consumes("application/x-www-form-urlencoded")]
        public async Task<IActionResult> DeleteCompetencies([FromForm] IFormCollection form)
        {
            var key = form["key"];
            var Competencies = await _context.Competencies.FindAsync(key);
            if (Competencies == null)
            {
                return NotFound();
            }
            
            _context.Competencies.Remove(Competencies);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool CompetenciesExists(int id)
        {
            return _context.Competencies.Any(e => e.Id == id);
        }


    }
}
