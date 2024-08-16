using BackEnd.Data;
using BackEnd.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;

namespace BackEnd.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ConfigController : Controller
    {

        private readonly CrdpCurriculumMsContext _context;
        private readonly IConfiguration _configuration;

        public ConfigController(CrdpCurriculumMsContext context, IConfiguration configuration)
        {
            _context = context;
            _configuration = configuration;

        }

        // GET: api/ConfigDomainFields
        [HttpGet("GetConfigDomainFields")]
        public async Task<ActionResult<IEnumerable<ConfigDomainField>>> GetConfigDomainFields()
        {
            return await _context.ConfigDomainFields.ToListAsync();
        }

        // GET: api/ConfigDomainFields/5
        [HttpGet("GetConfigDomainFieldsyId/{id}")]
        public async Task<IActionResult> GetConfigDomainFieldsById(int id)
        {
            var ConfigDomainField = await _context.ConfigDomainFields.FindAsync(id);
            if (ConfigDomainField == null)
            {
                return NotFound();
            }
            return Ok(ConfigDomainField);
        }

        [HttpPut("UpdateConfigDomainFields")]
        ////[Consumes("application/json")]
        [Consumes("application/x-www-form-urlencoded")]
        public async Task<IActionResult> UpdateConfigDomainFields([FromForm] IFormCollection form)
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

                var ConfigDomainField = await _context.ConfigDomainFields.FindAsync(id); // Use the converted int here

                // Check if the code exists
                if (ConfigDomainField == null)
                {
                    return NotFound("Code not found.");
                }

                // Update code values
                JsonConvert.PopulateObject(values, ConfigDomainField);

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

        // POST: api/ConfigDomainFields
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost("InsertConfigDomainFields")]
        [Consumes("application/x-www-form-urlencoded")]
        public async Task<ActionResult<ConfigDomainField>> InsertConfigDomainFields([FromForm] IFormCollection form)
        {
            var ConfigDomainField = new ConfigDomainField();
            try
            {
                var key = form["key"];
                var values = form["values"];

                JsonConvert.PopulateObject(values, ConfigDomainField);


                _context.ConfigDomainFields.Add(ConfigDomainField);
                await _context.SaveChangesAsync();
            }
            catch (Exception)
            {


            }


            return CreatedAtAction("GetConfigDomainFields", new { id = ConfigDomainField.Id }, ConfigDomainField);
        }

        // DELETE: api/ConfigDomainFields/5
        [HttpDelete("DeleteConfigDomainFields")]
        [Consumes("application/x-www-form-urlencoded")]
        public async Task<IActionResult> DeleteConfigDomainFields([FromForm] IFormCollection form)
        {
            var key = form["key"];

            // Convert the key to int before using it to find the entity
            if (!int.TryParse(key, out int id))
            {
                return BadRequest("Invalid key format.");
            }

            var ConfigDomainField = await _context.ConfigDomainFields.FindAsync(id); // Use the converted int here

            if (ConfigDomainField == null)
            {
                return NotFound();
            }

            _context.ConfigDomainFields.Remove(ConfigDomainField);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool ConfigDomainFieldExists(int id)
        {
            return _context.ConfigDomainFields.Any(e => e.Id == id);
        }


        // GET: api/ConfigDomainFields
        [HttpGet("GetConfigCycleClasses")]
        public async Task<ActionResult<IEnumerable<ConfigCycleClass>>> GetConfigCycleClasses()
        {
            return await _context.ConfigCycleClasses.ToListAsync();
        }

        // GET: api/ConfigDomainFields/5
        [HttpGet("GetConfigCycleClassesyId/{id}")]
        public async Task<IActionResult> GetConfigCycleClassesById(int id)
        {
            var code = await _context.ConfigCycleClasses.FindAsync(id);
            if (code == null)
            {
                return NotFound();
            }
            return Ok(code);
        }

        [HttpPut("UpdateConfigCycleClasses")]
        ////[Consumes("application/json")]
        [Consumes("application/x-www-form-urlencoded")]
        public async Task<IActionResult> UpdateConfigCycleClasses([FromForm] IFormCollection form)
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

                var ConfigCycleClass = await _context.ConfigCycleClasses.FindAsync(id); // Use the converted int here

                // ConfigCycleClass if the code exists
                if (ConfigCycleClass == null)
                {
                    return NotFound("Code not found.");
                }

                // Update code values
                JsonConvert.PopulateObject(values, ConfigCycleClass);

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

        // POST: api/ConfigDomainFields
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost("InsertConfigCycleClasses")]
        [Consumes("application/x-www-form-urlencoded")]
        public async Task<ActionResult<ConfigDomainField>> InsertConfigCycleClasses([FromForm] IFormCollection form)
        {
            var ConfigCycleClass = new ConfigCycleClass();
            try
            {
                var key = form["key"];
                var values = form["values"];

                JsonConvert.PopulateObject(values, ConfigCycleClass);


                _context.ConfigCycleClasses.Add(ConfigCycleClass);
                await _context.SaveChangesAsync();
            }
            catch (Exception)
            {


            }


            return CreatedAtAction("GetConfigCycleClasses", new { id = ConfigCycleClass.Id }, ConfigCycleClass);
        }

        // DELETE: api/ConfigDomainFields/5
        [HttpDelete("DeleteConfigCycleClasses")]
        [Consumes("application/x-www-form-urlencoded")]
        public async Task<IActionResult> DeleteConfigCycleClasses([FromForm] IFormCollection form)
        {
            var key = form["key"];

            // Convert the key to int before using it to find the entity
            if (!int.TryParse(key, out int id))
            {
                return BadRequest("Invalid key format.");
            }

            var ConfigCycleClass = await _context.ConfigCycleClasses.FindAsync(id); // Use the converted int here

            if (ConfigCycleClass == null)
            {
                return NotFound();
            }

            _context.ConfigCycleClasses.Remove(ConfigCycleClass);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool ConfigCycleClassesExists(int id)
        {
            return _context.ConfigCycleClasses.Any(e => e.Id == id);
        }


        // GET: api/ConfigConceptTreeLevels
        [HttpGet("GetConfigConceptTreeLevels")]
        public async Task<ActionResult<IEnumerable<ConfigConceptTreeLevel>>> GetConfigConceptTreeLevels()
        {
            return await _context.ConfigConceptTreeLevels.ToListAsync();
        }

        // GET: api/ConfigConceptTreeLevels/5
        [HttpGet("GetConfigConceptTreeLevelsyId/{id}")]
        public async Task<IActionResult> GetConfigConceptTreeLevelsById(int id)
        {
            var ConfigConceptTreeLevel = await _context.ConfigConceptTreeLevels.FindAsync(id);
            if (ConfigConceptTreeLevel == null)
            {
                return NotFound();
            }
            return Ok(ConfigConceptTreeLevel);
        }

        // GET: api/GetConfigConceptTreeLevelsNextlevelById/5
        [HttpGet("GetConfigNextlevelByConceptTreeLevel/{id}")]
        public async Task<IActionResult> GetConfigConceptTreeLevelsNextlevelById(int id)
        {
            var ConfigConceptTreeLevel = await _context.ConfigConceptTreeLevels.FirstOrDefaultAsync(e => e.ConceptTreeLevel == id);
            if (ConfigConceptTreeLevel == null)
            {
                return NotFound();
            }
            return Ok(ConfigConceptTreeLevel);
        }

        [HttpPut("UpdateConfigConceptTreeLevels")]
        ////[Consumes("application/json")]
        [Consumes("application/x-www-form-urlencoded")]
        public async Task<IActionResult> UpdateConfigConceptTreeLevels([FromForm] IFormCollection form)
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

                var ConfigConceptTreeLevel = await _context.ConfigConceptTreeLevels.FindAsync(id); // Use the converted int here

                // Check if the code exists
                if (ConfigConceptTreeLevel == null)
                {
                    return NotFound("Code not found.");
                }

                // Update code values
                JsonConvert.PopulateObject(values, ConfigConceptTreeLevel);

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

        // POST: api/ConfigConceptTreeLevels
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost("InsertConfigConceptTreeLevels")]
        [Consumes("application/x-www-form-urlencoded")]
        public async Task<ActionResult<ConfigConceptTreeLevel>> InsertConfigConceptTreeLevels([FromForm] IFormCollection form)
        {
            var ConfigConceptTreeLevel = new ConfigConceptTreeLevel();
            try
            {
                var key = form["key"];
                var values = form["values"];

                JsonConvert.PopulateObject(values, ConfigConceptTreeLevel);


                _context.ConfigConceptTreeLevels.Add(ConfigConceptTreeLevel);
                await _context.SaveChangesAsync();
            }
            catch (Exception)
            {

            }

            return CreatedAtAction("GetConfigConceptTreeLevels", new { id = ConfigConceptTreeLevel.Id }, ConfigConceptTreeLevel);
        }

        // DELETE: api/ConfigConceptTreeLevels/5
        [HttpDelete("DeleteConfigConceptTreeLevels")]
        [Consumes("application/x-www-form-urlencoded")]
        public async Task<IActionResult> DeleteConfigConceptTreeLevels([FromForm] IFormCollection form)
        {
            var key = form["key"];

            // Convert the key to int before using it to find the entity
            if (!int.TryParse(key, out int id))
            {
                return BadRequest("Invalid key format.");
            }

            var ConfigConceptTreeLevel = await _context.ConfigConceptTreeLevels.FindAsync(id); // Use the converted int here

            if (ConfigConceptTreeLevel == null)
            {
                return NotFound();
            }

            _context.ConfigConceptTreeLevels.Remove(ConfigConceptTreeLevel);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool ConfigConceptTreeLevelExists(int id)
        {
            return _context.ConfigConceptTreeLevels.Any(e => e.Id == id);
        }



        [HttpGet("GetConfigCompetenciesLevel")]
        public async Task<IActionResult> GetConfigCompetenciesLevel()
        {
            var ConfigCompetenciesLevel = await _context.ConfigCompetenciesLevels.ToListAsync();
            if (ConfigCompetenciesLevel == null)
            {
                return NotFound();
            }
            return Ok(ConfigCompetenciesLevel);
        }

        [HttpPut("UpdateConfigCompetenciesLevel")]
        ////[Consumes("application/json")]
        [Consumes("application/x-www-form-urlencoded")]
        public async Task<IActionResult> UpdateConfigCompetenciesLevel([FromForm] IFormCollection form)
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

                var ConfigCompetenciesLevel = await _context.ConfigCompetenciesLevels.FindAsync(id); // Use the converted int here

                // Check if the code exists
                if (ConfigCompetenciesLevel == null)
                {
                    return NotFound("Code not found.");
                }

                // Update code values
                JsonConvert.PopulateObject(values, ConfigCompetenciesLevel);

                // Save changes
                await _context.SaveChangesAsync();

                return Ok(new { success = true, message = "ConfigCompetenciesLevel updated successfully." });
            }
            catch (Exception ex)
            {
                // Return an error response in case of an exception
                return StatusCode(500, new { success = false, message = ex.Message });
            }
        }

        //POST: api/ConfigCompetenciesLevel
        //To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost("InsertConfigCompetenciesLevel")]
        [Consumes("application/x-www-form-urlencoded")]
        public async Task<ActionResult<ConfigCompetenciesLevel>> InsertConfigCompetenciesLevel([FromForm] IFormCollection form)
        {
            var ConfigCompetenciesLevel = new ConfigCompetenciesLevel();
            try
            {
                var key = form["key"];
                var values = form["values"];

                JsonConvert.PopulateObject(values, ConfigCompetenciesLevel);


                _context.ConfigCompetenciesLevels.Add(ConfigCompetenciesLevel);
                await _context.SaveChangesAsync();
            }
            catch (Exception)
            {

            }

            return CreatedAtAction("GetConfigCompetenciesLevel", new { id = ConfigCompetenciesLevel.Id }, ConfigCompetenciesLevel);
        }

        [HttpDelete("DeleteConfigCompetenciesLevel")]
        [Consumes("application/x-www-form-urlencoded")]
        public async Task<IActionResult> DeleteConfigCompetenciesLevel([FromForm] IFormCollection form)
        {
            var key = form["key"];

            // Convert the key to int before using it to find the entity
            if (!int.TryParse(key, out int id))
            {
                return BadRequest("Invalid key format.");
            }

            var ConfigCompetenciesLevel = await _context.ConfigCompetenciesLevels.FindAsync(id); // Use the converted int here

            if (ConfigCompetenciesLevel == null)
            {
                return NotFound();
            }

            _context.ConfigCompetenciesLevels.Remove(ConfigCompetenciesLevel);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool ConfigCompetenciesLevelExists(int id)
        {
            return _context.ConfigCompetenciesLevels.Any(e => e.Id == id);
        }

        // GET: api/GetConfigConceptTreeLevelsNextlevelById/5
        [HttpGet("GetConceptTreeClassByConceptId/{id}")]
        public async Task<IActionResult> GetConceptTreeClassByConceptId(int id)
        {
            var ConceptTreeClasses = await _context.ConceptTreeClasses.FirstOrDefaultAsync(e => e.Ctid == id);
            if (ConceptTreeClasses == null)
            {
                return NotFound();
            }
            return Ok(ConceptTreeClasses);
        }

        //POST: api/ConceptTreeClasses
        //To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost("InsertConceptTreeClasses")]
        [Consumes("application/x-www-form-urlencoded")]
        public async Task<ActionResult<ConceptTreeClass>> InsertConceptTreeClasses([FromForm] IFormCollection form)
        {
            var ConceptTreeClass = new ConceptTreeClass();
            try
            {
                var key = form["key"];
                var values = form["values"];


                // Convert the key to int before using it to find the entity
                if (!int.TryParse(key, out int id))
                {
                    return BadRequest("Invalid key format.");
                }

                ConceptTreeClass = await _context.ConceptTreeClasses.FindAsync(id); // Use the converted int here

                _context.ConceptTreeClasses.Remove(ConceptTreeClass);
                await _context.SaveChangesAsync();

                JsonConvert.PopulateObject(values, ConceptTreeClass);
                _context.ConceptTreeClasses.Add(ConceptTreeClass);
                await _context.SaveChangesAsync();
            }
            catch (Exception)
            {

            }

            return Ok();
        }
    }
}
