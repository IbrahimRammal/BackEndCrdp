using BackEnd.Class;
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

        [HttpGet("count-by-type")]
        public async Task<ActionResult<List<CompetenceTypeCountDto>>> GetCompetenciesCountByType()
        {
            try
            {
                // Fetch all level codes
                var levelCodes = await _context.CodesContents
                    .Where(c => c.CodeId == 5) // Adjust this if needed
                    .ToDictionaryAsync(c => c.Id, c => c.CodeContentName); // Create a dictionary for easy lookup
                                                                           // Group by CompetenceType

                var competenceTypeCodes = await _context.CodesContents
                    .Where(c => c.CodeId == 9) // Fetch competence type names
                    .ToDictionaryAsync(c => c.Id, c => c.CodeContentName); // Dictionary for easy lookup

                var counts = await _context.Competencies
                    .GroupBy(c => c.CompetenceType)
                    .Select(g => new CompetenceTypeCountDto
                    {
                        CompetenceType = g.Key ?? 0,
                        CompetenceTypeName = competenceTypeCodes.GetValueOrDefault(g.Key ?? 0), // CompetenceType name
                        Count = g.Count(),
                        Levels = g.GroupBy(c => c.CompetenceLevel)
                                  .Select(lg => new CompetenceLevelDto
                                  {
                                      CompetenceLevelId = lg.Key ?? 0,
                                      CompetenceLevelName = levelCodes.GetValueOrDefault(lg.Key ?? 0), // Lookup name
                                      Count = lg.Count(),
                                      Competencies = lg.Select(c => new CompetenciesDto
                                      {
                                          Id = c.Id,
                                          CompetenceName = c.CompetenceName,
                                          CompetenceDetails = c.CompetenceDetails
                                          // Map other fields as necessary
                                      }).ToList()
                                  }).ToList()
                    })
                    .ToListAsync();

                return Ok(counts);
            }
            catch (Exception ex)
            {
                // Log exception for debugging
                Console.WriteLine($"Exception: {ex.Message}");
                return StatusCode(500, "Internal server error");
            }
        }

        // GET: api/Competencies
        [HttpGet("GetCompetencies")]
        public async Task<ActionResult<IEnumerable<Competencies>>> GetCompetencies()
        {
            return await _context.Competencies.ToListAsync();
        }

        // GET: api/Competenciescascade
        [HttpGet("GetCompetenciescascade")]
        public async Task<ActionResult<IEnumerable<VCompetenciesCascade>>> GetCompetenciescascade()
        {
            return await _context.VCompetenciesCascades.ToListAsync();
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

        // By kamel Nazar
        [HttpGet("count-by-type-ID")]
        public async Task<ActionResult<int>> GetCompetenciesCountByType([FromQuery] int competenceType)
        {
            try
            {
                // Count the number of competencies based on the competenceType
                int count = await _context.Competencies
                    .Where(c => c.CompetenceType == competenceType)
                    .CountAsync();

                return Ok(count);
            }
            catch (Exception ex)
            {
                // Log exception for debugging
                Console.WriteLine($"Exception: {ex.Message}");
                return StatusCode(500, "Internal server error");
            }
        }

        //// By kamel Nazar
        //[HttpGet("count-by-type")]
        //public async Task<ActionResult<List<CompetenceTypeCountDto>>> GetCompetenciesCountByType()
        //{
        //    try
        //    {
        //        // Group by CompetenceType and count each group
        //        var counts = await _context.Competencies
        //            .GroupBy(c => c.CompetenceType)
        //            .Select(g => new CompetenceTypeCountDto
        //            {
        //                CompetenceType = g.Key ?? 0,  // Handle null CompetenceType
        //                Count = g.Count()
        //            })
        //            .ToListAsync();

        //        return Ok(counts);
        //    }
        //    catch (Exception ex)
        //    {
        //        // Log exception for debugging
        //        Console.WriteLine($"Exception: {ex.Message}");
        //        return StatusCode(500, "Internal server error");
        //    }
        //}

        [HttpPost("AddCompetences")]
        public IActionResult AddCompetences(CompetenciesInfo competenciesInfo)
        {
            try
            {
                var Competencies = new Competencies();
                Competencies.CompetenceName = competenciesInfo.competencieName;
                Competencies.CompetenceParentId = 0;
                Competencies.CompetenceActive = true;
                Competencies.CompetenceType = competenciesInfo.CompetenceTypeValue;
                _context.Competencies.Add(Competencies);
                _context.SaveChanges();
                int idcompetence = Competencies.Id;
                for (int i = 0; i < competenciesInfo.valuePrimayName.Count(); i++)
                {
                    var CompetenciesP = new Competencies();
                    CompetenciesP.CompetenceName = competenciesInfo.valuePrimayName[i];
                    CompetenciesP.CompetenceParentId = idcompetence;
                    CompetenciesP.CompetenceActive = true;
                    CompetenciesP.CompetenceType = competenciesInfo.CompetenceTypeValue;
                    _context.Competencies.Add(CompetenciesP);
                    _context.SaveChanges();
                    int idcompetenceP = CompetenciesP.Id;
                    for (int j = 0; j < competenciesInfo.SecondarySelectedClasses[i].Count(); j++)
                    {
                        var CompetenciesS = new Competencies();
                        CompetenciesS.CompetenceName = competenciesInfo.selectedNameSecondary[i][j];
                        CompetenciesS.CompetenceParentId = idcompetenceP;
                        CompetenciesS.CompetenceActive = true;
                        CompetenciesS.CompetenceType = competenciesInfo.CompetenceTypeValue;
                        _context.Competencies.Add(CompetenciesS);
                        _context.SaveChanges();
                        int idcompetenceS = CompetenciesS.Id;
                        for (int iclass = 0; iclass < competenciesInfo.SecondarySelectedClasses[i][j].Count(); iclass++)
                        {
                            var competenciesClass = new CompetenciesClass();
                            competenciesClass.Cid = idcompetenceS;
                            competenciesClass.ClassId = competenciesInfo.SecondarySelectedClasses[i][j][iclass];
                            _context.CompetenciesClasses.Add(competenciesClass);
                            _context.SaveChanges();
                        }
                        //for (int iConceptTree = 0; iConceptTree < competenciesInfo.SecondarySelectedConceptTree[i][j].Count(); iConceptTree++)
                        //{
                        //    var competenciesConceptTree = new CompetenciesConceptTree();
                        //    competenciesConceptTree.Cid = idcompetenceS;
                        //    competenciesConceptTree.ConceptTreeId = competenciesInfo.SecondarySelectedClasses[i][j][iConceptTree];
                        //    _context.CompetenciesConceptTrees.Add(competenciesConceptTree);
                        //    _context.SaveChanges();

                        //}
                        if (competenciesInfo.DetailsSecondary.Count() > 0)
                        {
                            for (int idetail = 0; idetail < competenciesInfo.DetailsSecondary[i][j].Count(); idetail++)
                            {
                                var competenciesdetail = new Competencies();
                                competenciesdetail.CompetenceName = competenciesInfo.DetailsSecondary[i][j][idetail];
                                competenciesdetail.CompetenceDetails = competenciesInfo.DetailsSecondary[i][j][idetail];
                                competenciesdetail.CompetenceParentId = idcompetenceS;
                                competenciesdetail.CompetenceActive = true;
                                competenciesdetail.CompetenceType = competenciesInfo.CompetenceTypeValue;
                                _context.Competencies.Add(competenciesdetail);
                                _context.SaveChanges();
                            }
                        }
                    }
                }
            }
            catch (Exception)
            {

                return BadRequest();
            }

            // Process the received data here
            // For demonstration, we're just returning it back
            return Ok(competenciesInfo);
        }



    }
}