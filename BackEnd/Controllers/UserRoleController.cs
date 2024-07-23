using BackEnd.Class;
using BackEnd.Models;

//using DevExtreme.AspNet.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using System.Linq;
using System.Net;
using System.Net.Http.Formatting;


// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace BackEnd.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserRoleController : ControllerBase
    {

        private readonly UserManager<IdentityUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly IConfiguration _configuration;
        public UserRoleController(UserManager<IdentityUser> userManager, RoleManager<IdentityRole> roleManager, IConfiguration configuration)
        {
            _configuration = configuration;
            _userManager = userManager;
            _roleManager = roleManager;
        }

        /// <summary>
        /// return all users
        /// </summary>
        /// <returns></returns>
        // GET: api/<UserRoleController>

        [HttpGet("GetAllUsers")]
        //,int take,bool requireTotalCount
        //[FromQuery] RequestInfo requestIInfo
        public async Task<IActionResult> GetAllUsers()
        {
          
            var users = await _userManager.Users.ToListAsync();
            var totalCount = await _userManager.Users.CountAsync();
            return Ok(users);
            //return Ok(new PaginationModel
            //{
            //    users= users,
            //    totalCount= totalCount
            //});
        }
        [HttpPut("UpdateUser")]
        [Consumes("application/x-www-form-urlencoded")]
        public  async Task<bool> UpdateUser([FromForm] IFormCollection form)
        {
            var key = form["key"];
            var values = form["values"];
            var user = _userManager.Users.First(o => o.Id == key.ToString());

            JsonConvert.PopulateObject(values, user);

            //Validate(order);
            //if (!ModelState.IsValid)
            //    return Request.CreateErrorResponse(HttpStatusCode.BadRequest, ModelState.GetFullErrorMessage());

            var result  =await _userManager.UpdateAsync(user);

            return result.Succeeded;
        }

        [HttpPost("insertUser")]
        [Consumes("application/x-www-form-urlencoded")]
        public async Task<IActionResult> insertUser([FromForm] IFormCollection form)
        {
            var key = form["key"];
            var values = form["values"];
            var user = new IdentityUser ();
            JsonConvert.PopulateObject(values, user);
            Register register = new Register();
            if (values.Count > 0)
            {
               
                JsonConvert.PopulateObject(values[0], register);
            }
            var result = await _userManager.CreateAsync(user, register.Password);
            if (result.Succeeded)
            {
                return Ok(new { message = "User registered Successfully" });
            }
            return BadRequest(result.Errors);
         
        }
        //[HttpGet("GetAllUsers")]
        ////,int take,bool requireTotalCount
        //public async Task<IActionResult> GetAllUsers()
        //{
        //    var users =  _userManager.Users.AsQueryable();
        //    return Ok(DataSourceLoader.Load(users,null));

        //    //return Ok(new PaginationModel
        //    //{
        //    //    users= users,
        //    //    totalCount= totalCount
        //    //});
        //}

        // GET api/<UserRoleController>/5
        [HttpGet("{id}")]
        public string Get(int id)
        {
            return "value";
        }

        // POST api/<UserRoleController>
        [HttpPost]
        public void Post([FromBody] string value)
        {
        }

        // PUT api/<UserRoleController>/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {
        }

        // DELETE api/<UserRoleController>/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
