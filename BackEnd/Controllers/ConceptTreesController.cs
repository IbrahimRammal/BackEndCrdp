using BackEnd.Class;
using BackEnd.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using System.Data;
using BackEnd.Helper;

namespace BackEnd.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ConceptTreesController : ControllerBase
    {
        private readonly IConfiguration _configuration;
        private readonly CrdpCurriculumMsContext _context;


        public static class GlobalConstants
        {
            public static string PageName { get; set; } = "concept_tree";
        }


        //a function to not repeat code to check Authorization on function Level
        private async Task<bool> IsAuthorized(HttpRequest? request, params string[] permissions)
        {
            var jwtHelper = new JwtHelper(_context, _configuration);

            foreach (var permission in permissions)
            {
                if ((await jwtHelper.CheckPermission(Request, GlobalConstants.PageName, permission)).Success)
                {
                    return true;
                }
            }

            return false;
        }

        //added by mohamad baydoun to apply permissions on the page buttons
        [HttpGet("GetConceptTreesPermissions")]
        public async Task<ActionResult<IEnumerable<Permissions>>> GetConceptTreesPermissions()
        {
            var token = Request.Headers["Authorization"].FirstOrDefault();
            if (token == null)
            {
                return Ok("No Token");
            }
            var jwtHelper = new JwtHelper(_context, _configuration);
            var userInfo = await jwtHelper.GetUserInfoFromJwt(token, GlobalConstants.PageName);

            if (userInfo == null)
            {
                return Ok("userInfo null");
            }

            var permissions = new Permissions();

            if (userInfo.ServiceNames.Any(sn => sn.ToLower().Contains("manage")) || (userInfo.isAdmin ?? false))
            {
                permissions.CanEdit = true;
                permissions.CanAdd = true;
                permissions.CanDelete = true;
                permissions.CanView = true;
            }
            else if (!userInfo.ServiceNames.Any(sn => sn.Contains("view")))
            {
                permissions.CanEdit = false;
                permissions.CanAdd = false;
                permissions.CanDelete = false;
                permissions.CanView = false;
            }
            else
            {
                permissions.CanEdit = userInfo.ServiceNames.Any(sn => sn.Contains("edit"));
                permissions.CanAdd = userInfo.ServiceNames.Any(sn => sn.Contains("add"));
                permissions.CanDelete = userInfo.ServiceNames.Any(sn => sn.Contains("delete"));
                permissions.CanView = userInfo.ServiceNames.Any(sn => sn.Contains("view"));
            }

            return Ok(

                permissions

              );
        }

        public class Permissions
        {
            public bool CanEdit { get; set; }
            public bool CanAdd { get; set; }
            public bool CanDelete { get; set; }
            public bool CanView { get; set; }
        }


        public ConceptTreesController(CrdpCurriculumMsContext context, IConfiguration configuration)
        {
            _context = context;
            _configuration = configuration;

        }

        // DELETE: api/ConceptTrees/5
        [HttpDelete("DeleteConceptTrees")]
        [Consumes("application/x-www-form-urlencoded")]
        public async Task<IActionResult> DeleteConceptTrees([FromForm] IFormCollection form)
        {
            if (!await IsAuthorized(Request, "manage", "delete"))
            {
                return Ok("Not Authorized");
            }
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

            // Find all ConceptTreeClasses where eid equals Ctid (assuming Ctid is key)
            var conceptTreeClasses = _context.ConceptTreeClasses
                .Where(c => c.Ctid == key)
                .ToList();

            if (conceptTreeClasses.Any())
            {
                _context.ConceptTreeClasses.RemoveRange(conceptTreeClasses);
                await _context.SaveChangesAsync();
            }



            _context.ConceptTrees.Remove(concept);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        // By kamel Nazar
        [HttpGet("GetClassesByConceptID")]
        public async Task<ActionResult<List<int>>> GetClassesByConceptID(int conceptId)
        {

            var jwtHelper = new JwtHelper(_context, _configuration);
            var permission1 = (await jwtHelper.CheckPermission(Request, "concept_tree", "manage")).Success;
            var permission2 = (await jwtHelper.CheckPermission(Request, "concept_tree", "edit")).Success;

            //Authorization 
            if (!permission1 && !permission2)
            {
                return Ok("Not Authorized");
            }

            try
            {
                // Retrieve the list of ClassIds associated with the given conceptId
                var classIds = await _context.ConceptTreeClasses
                    .Where(c => c.Ctid == conceptId)
                    .Select(c => c.ClassId.Value) // Select only the ClassId, assuming ClassId is not null
                    .ToListAsync();

                if (classIds == null || !classIds.Any())
                {
                    return NotFound("No classes found for the provided concept ID.");
                }

                return Ok(classIds);
            }
            catch (Exception ex)
            {
                // Log exception for debugging
                Console.WriteLine($"Exception: {ex.Message}");
                return StatusCode(500, "Internal server error");
            }
        }

        [HttpGet("ConceptCountByLevel")]
        public async Task<ActionResult<IEnumerable<ConceptLevelCountDto>>> GetConceptCountByLevel()
        {
            // Perform a join between ConceptTrees and CodesContents to get the level name
            var result = await _context.ConceptTrees
                .Join(_context.CodesContents,
                    concept => concept.ConceptLevel,   // Join on ConceptLevel
                    code => code.Id,                  // Join on Id in CodesContents
                    (concept, code) => new
                    {
                        code.CodeContentName,
                        concept.ConceptLevel
                    })
                .GroupBy(x => new { x.ConceptLevel, x.CodeContentName })
                .Select(g => new ConceptLevelCountDto
                {
                    LevelName = g.Key.CodeContentName, // Use the level name from the join
                    Count = g.Count()
                })
                .ToListAsync();

            return Ok(result);
        }

        // GET: api/Conceptscascade
        [HttpGet("GetConceptscascade")]
        public async Task<ActionResult<IEnumerable<VconceptsCascade>>> GetConceptscascade()
        {
            return await _context.VconceptsCascades.ToListAsync();
        }

        // GET: api/Conceptscascadeclass
        [HttpGet("GetConceptscascadeclass")]
        public async Task<ActionResult<IEnumerable<VconceptsCascadeClass>>> GetConceptscascadeclass()
        {
            return await _context.VconceptsCascadeClasses.ToListAsync();
        }

        [HttpGet("GetConceptTrees")]
        public async Task<ActionResult<IEnumerable<ConceptTree>>> GetConceptTrees()
        {
            return await _context.ConceptTrees.ToListAsync();
        }

        //changed by mohamd baydoun to apply permissions on data level
        [HttpGet("GetConceptTreesbyClasses")]
        public async Task<ActionResult<IEnumerable<ConceptTreeDto>>> GetConceptTreesbyClasses()
        {
            //Authorization
            if (!await IsAuthorized(Request, "manage", "view"))
            {
                return Ok("Not Authorized");
            }

            //getuserdata
            var jwtHelper = new JwtHelper(_context, _configuration);
            var token = Request.Headers["Authorization"].FirstOrDefault();
            if (token == null)
            {
                return Ok("No Token");
            }
            var userInfo = await jwtHelper.GetUserInfoFromJwt(token, GlobalConstants.PageName);
            if (userInfo == null)
            {
                return Ok("No User Founded");
            }

            var userId = userInfo.UserId;
            var isAdmin = userInfo.isAdmin ?? false;

            var conceptTreeWithClasses = from ct in _context.ConceptTrees
                                         join ctc in _context.ConceptTreeClasses
                                         on ct.Id equals ctc.Ctid into ctcGroup
                                         from ctc in ctcGroup.DefaultIfEmpty()
                                         join cc in _context.CodesContents
                                         on ctc.ClassId equals cc.Id into ccGroup
                                         from cc in ccGroup.DefaultIfEmpty()
                                         join urp in _context.UserRolePermissions
                                         on ct.ConceptField equals urp.ConceptFiled
                                         into urpGroup //joined with permissions
                                         from urp in urpGroup.DefaultIfEmpty()
                                         join ur in _context.UserRoles on urp.UserRoleId equals ur.Id into urGroup
                                         from ur in urGroup.DefaultIfEmpty()
                                         where ((isAdmin || ur.UserId == userId || ct.UserCreated == userId) && urp.Class == ctc.ClassId)
                                         group cc.CodeContentName by new
                                         {
                                             ct.Id,
                                             ct.IdNumber,
                                             ct.ConceptName,
                                             ct.ConceptType,
                                             ct.ConceptDomain,
                                             ct.ConceptField,
                                             ct.ConceptDetails,
                                             ct.ConceptParentId,
                                             ct.ConceptActive,
                                             ct.ConceptLevel,
                                             ct.UserCreated,
                                             ct.DateCreated,
                                             ct.UserModified,
                                             ct.DateModified,
                                         } into grouped
                                         select new ConceptTreeDto
                                         {
                                             Id = grouped.Key.Id,
                                             IdNumber = grouped.Key.IdNumber,
                                             ConceptName = grouped.Key.ConceptName,
                                             ConceptType = grouped.Key.ConceptType,
                                             ConceptDomain = grouped.Key.ConceptDomain,
                                             ConceptField = grouped.Key.ConceptField,
                                             ConceptDetails = grouped.Key.ConceptDetails,
                                             ConceptParentId = grouped.Key.ConceptParentId,
                                             ConceptActive = grouped.Key.ConceptActive ?? false, // Handle nullable bool
                                             ConceptLevel = grouped.Key.ConceptLevel,
                                             UserCreated = grouped.Key.UserCreated,
                                             DateCreated = grouped.Key.DateCreated,
                                             UserModified = grouped.Key.UserModified,
                                             DateModified = grouped.Key.DateModified,
                                             ClassNames = grouped.Where(name => name != null).ToList()
                                         };

            var conceptTrees = await conceptTreeWithClasses
                .AsNoTracking()
                .ToListAsync();

            return Ok(conceptTrees);
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



        // By kamel Nazar
        [HttpPost("InsertConceptTreeClasses")]
        [Consumes("application/x-www-form-urlencoded")]
        public async Task<ActionResult> InsertConceptTreeClasses([FromForm] IFormCollection form)
        {
            try
            {
                var key = form["key"];
                var values = form["values"];

                // Log received data for debugging
                Console.WriteLine($"Received key: {key}");
                Console.WriteLine($"Received values: {values}");

                // Convert the key to int before using it to find the entity
                if (!int.TryParse(key, out int id))
                {
                    return BadRequest("Invalid key format.");
                }

                // Deserialize the JSON array of classes
                var selectedClassIds = JsonConvert.DeserializeObject<List<int>>(values);

                if (selectedClassIds == null || !selectedClassIds.Any())
                {
                    return BadRequest("No classes selected.");
                }

                // Retrieve the ConceptTreeClass entities for the given concept ID
                var existingClasses = await _context.ConceptTreeClasses
                    .Where(c => c.Ctid == id)
                    .ToListAsync();

                // Remove existing classes for this concept
                _context.ConceptTreeClasses.RemoveRange(existingClasses);
                await _context.SaveChangesAsync();

                // Create new ConceptTreeClass entities based on the selected class IDs
                foreach (var classId in selectedClassIds)
                {
                    var newClass = new ConceptTreeClass
                    {
                        Ctid = id,
                        ClassId = classId,
                        UserCreated = 1,
                        DateCreated = DateTime.UtcNow
                    };
                    _context.ConceptTreeClasses.Add(newClass);
                }

                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                // Log exception for debugging
                Console.WriteLine($"Exception: {ex.Message}");
                return StatusCode(500, "Internal server error");
            }

            return Ok();
        }
        [HttpPost("InsertConceptTrees")]
        [Consumes("application/x-www-form-urlencoded")]
        public async Task<ActionResult<ConceptTree>> PostConceptTrees([FromForm] IFormCollection form)
        {

            //Authorization
            if (!await IsAuthorized(Request, "manage", "add"))
            {
                return Ok("Not Authorized");
            }
            var ConceptTrees = new ConceptTree();
            try
            {
                var key = form["key"];
                var values = form["values"];

                // Populate the ConceptTree object with the form data
                JsonConvert.PopulateObject(values, ConceptTrees);

                // Add the ConceptTree to the context but don't save changes yet
                _context.ConceptTrees.Add(ConceptTrees);
                await _context.SaveChangesAsync();

                // Now that the Id has been generated, set the GroupID
                if (ConceptTrees.ConceptParentId.HasValue)
                {
                    ConceptTrees.GroupId = ConceptTrees.ConceptParentId.Value;
                }
                else
                {
                    ConceptTrees.GroupId = ConceptTrees.Id;
                }

                // Save the updated GroupID
                await _context.SaveChangesAsync();

                // Reload the entity to get the latest state from the database
                await _context.Entry(ConceptTrees).ReloadAsync();
            }
            catch (Exception ex)
            {
                // Handle exceptions here (e.g., log the error)
                return StatusCode(500, "An error occurred while processing your request.");
            }

            return CreatedAtAction("GetConceptTrees", new { id = ConceptTrees.Id }, ConceptTrees);
        }

        // POST: api/ConceptTrees
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        //[HttpPost("InsertConceptTrees")]
        //[Consumes("application/x-www-form-urlencoded")]
        //public async Task<ActionResult<ConceptTree>> PostConceptTrees([FromForm] IFormCollection form)
        //{
        //    var ConceptTrees = new ConceptTree();
        //    try
        //    {
        //        var key = form["key"];
        //        var values = form["values"];

        //        JsonConvert.PopulateObject(values, ConceptTrees);


        //        _context.ConceptTrees.Add(ConceptTrees);
        //        await _context.SaveChangesAsync();

        //        await _context.Entry(ConceptTrees).ReloadAsync();
        //    }
        //    catch (Exception)
        //    {


        //    }


        //    return CreatedAtAction("GetConceptTrees", new { id = ConceptTrees.Id }, ConceptTrees);
        //}

        [HttpPost("PostCTreeCompetencies")]
        public async Task<ActionResult<IEnumerable<ConceptTreeSecondary>>> PostCTreeCompetencies(SearchRules conceptInfo)
        {

            var dt = new DataTable();
            dt.Columns.Add("Id", typeof(int));

            // Populate the DataTable with your list of integers
            foreach (var id in conceptInfo.classes)
            {
                dt.Rows.Add(id);
            }

            // Set up the parameters including the table-valued parameter
            var parameters = new[]
            {
    new SqlParameter("@ClassesId", SqlDbType.Structured)
    {
        TypeName = "dbo.ClassesId",
        Value = dt
    },  new SqlParameter("@knowldegeField", SqlDbType.Int)
    {

        Value = conceptInfo.knowldegeField
    }
};

            // Execute the stored procedure
            var conceptTreeSecondary = await _context.VconceptsCascadeClasses
                .FromSqlRaw("EXEC dbo.GetConceptTreeFromView @ClassesId ,@knowldegeField", parameters)
                .ToListAsync();

            var CompetencePublicSecondaryData = conceptTreeSecondary.Select(x => new ConceptTreeSecondary
            {
                idC = x.Id1,
                idP = x.Id2,
                idS = x.Id3,
                ClassId = x.ClassId,
                ConceptName = x.ConceptName1,
                ConceptName4 = x.ConceptName4,
                ConceptNameP = x.ConceptName2,
                ConceptNameS = x.ConceptName3,
                cyclename = x.Cyclename,
                classname = x.Classname



            }).ToList();
            //var groupedProducts = competenciesSecondaryPublicC.GroupBy(p => p.GroupId);
            return CompetencePublicSecondaryData;
        }

        // PUT: api/ConceptTrees/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("UpdateConceptTrees")]
        [Consumes("application/x-www-form-urlencoded")]
        public async Task<IActionResult> PutConceptTrees([FromForm] IFormCollection form)
        {
            if (!await IsAuthorized(Request, "manage", "edit"))
            {
                return Ok("Not Authorized");
            }
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
                await _context.Entry(concept).ReloadAsync();
                return Ok(new { success = true, message = "concept updated successfully." });
            }
            catch (Exception ex)
            {
                // Return an error response in case of an exception
                return StatusCode(500, new { success = false, message = ex.Message });
            }

        }
    }
}
