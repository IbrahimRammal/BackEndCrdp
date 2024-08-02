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
    }
}
