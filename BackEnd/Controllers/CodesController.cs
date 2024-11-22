﻿using BackEnd.Data;
using BackEnd.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using System.Security.Cryptography;

namespace BackEnd.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CodesController : ControllerBase

    {
        private readonly CrdpCurriculumMsContext _context;
        private readonly IConfiguration _configuration;



        public CodesController(CrdpCurriculumMsContext context, IConfiguration configuration)
        {
            _context = context;
            _configuration = configuration;

        }

        [HttpGet("GetCodesCompetences")]
        public async Task<ActionResult> GetCodesCompetences()
        {
            //var codes= await _context.Codes
            //           .Where(c => new[] { 2, 3 }.Contains(c.Id))
            //           .Select(c => new
            //           {
            //               c.Id,
            //               c.CodeDescription,
            //               c.CodeName,
            //               CodeContents = c.CodesContents.Select(c0 => new
            //               {
            //                   c0.Id,
            //                   c0.CodeContentDescription,
            //                   c0.CodeContentName,
            //                   c0.CodeId
            //               }).ToList()
            //           })
            //           .OrderBy(c => c.Id)
            //           .ToListAsync();
            var codes = await _context.Codes
                       .Where(c => new[] { 2, 3, 9 }.Contains(c.Id))
                       .Select(c => new
                       {
                           c.Id,
                           c.CodeDescription,
                           c.CodeName,
                           CodeContents = c.CodesContents.Select(c0 => new
                           {
                               c0.Id,
                               c0.CodeContentDescription,
                               c0.CodeContentName,
                               c0.CodeId
                           }).ToList()
                       })
                       .OrderBy(c => c.Id)
                       .ToListAsync();
            return Ok(new
            {
                dataClass = codes.Where(o => o.CodeDescription.Equals("Class")).SelectMany(x => x.CodeContents),
                dataKnowledgeField = codes.Where(o => o.CodeDescription.Equals("Knowledge Field")).SelectMany(x => x.CodeContents),
                dataCompetenceType = codes.Where(o => o.CodeDescription.Trim().Equals("Competence Type")).SelectMany(x => x.CodeContents)
            });

        }
        // GET: api/Codes
        [HttpGet("GetCodes")]
        public async Task<ActionResult<IEnumerable<Code>>> GetCodes()
        {
            return await _context.Codes.ToListAsync();
        }

        // GET: api/Codes/5
        [HttpGet("GetCodesById/{id}")]
        public async Task<IActionResult> GetCodesById(int id)
        {
            var code = await _context.Codes.FindAsync(id);
            if (code == null)
            {
                return NotFound();
            }
            return Ok(code);
        }

        // PUT: api/Codes/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("UpdateCode00")]
        [Consumes("application/x-www-form-urlencoded")]
        public async Task<IActionResult> UpdateCode([FromForm] IFormCollection form)
        {
            try
            {
                var key = form["key"];
                var values = form["values"];

                // تحقق من صحة الـ key
                if (!int.TryParse(key, out int id))
                {
                    return BadRequest("Invalid key format.");
                }

                var code = _context.Codes.FirstOrDefault(o => o.Id == id);

                // تحقق من وجود الكود
                if (code == null)
                {
                    return NotFound("Code not found.");
                }

                // تحديث بيانات الكود
                JsonConvert.PopulateObject(values, code);

                // حفظ التغييرات
                await _context.SaveChangesAsync();

                return Ok(new { success = true, message = "Code updated successfully." });
            }
            catch (Exception ex)
            {
                // إعادة استجابة خطأ في حالة حدوث استثناء
                return StatusCode(500, new { success = false, message = ex.Message });
            }
        }
        [HttpPut("UpdateCode")]
        ////[Consumes("application/json")]
        [Consumes("application/x-www-form-urlencoded")]
        public async Task<IActionResult> UpdateCodes([FromForm] IFormCollection form)
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

                var code = await _context.Codes.FindAsync(id); // Use the converted int here

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
        [HttpPost("InsertCode")]
        [Consumes("application/x-www-form-urlencoded")]
        public async Task<ActionResult<Code>> PostCode([FromForm] IFormCollection form)
        {
            var code = new Code();
            try
            {
                var key = form["key"];
                var values = form["values"];

                JsonConvert.PopulateObject(values, code);

          
                _context.Codes.Add(code);
                await _context.SaveChangesAsync();
            }
            catch (Exception)
            {


            }


            return CreatedAtAction("GetCodes", new { id = code.Id }, code);
        }

        // DELETE: api/Codes/5
        [HttpDelete("DeleteCode")]
        [Consumes("application/x-www-form-urlencoded")]
        public async Task<IActionResult> DeleteCode([FromForm] IFormCollection form)
        {
            var key = form["key"];

            // Convert the key to int before using it to find the entity
            if (!int.TryParse(key, out int id))
            {
                return BadRequest("Invalid key format.");
            }

            var code = await _context.Codes.FindAsync(id); // Use the converted int here

            if (code == null)
            {
                return NotFound();
            }

            _context.Codes.Remove(code);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool CodeExists(int id)
        {
            return _context.Codes.Any(e => e.Id == id);
        }


    }
}
