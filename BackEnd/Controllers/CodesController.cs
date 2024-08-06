using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using BackEnd.Models;

namespace BackEnd.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CodesController : ControllerBase
    {
        private readonly CrdpCurriculumMsContext _context;

        public CodesController(CrdpCurriculumMsContext context)
        {
            _context = context;
        }

        // GET: api/Codes
        [HttpGet("GetCodes")]
        public async Task<ActionResult> GetCodes()
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
                       .Where(c => new[] { 2, 3 }.Contains(c.Id))
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
                dataClass=codes.Where(o => o.CodeDescription.Equals("Class")).SelectMany(x=>x.CodeContents),
                dataKnowledgeField = codes.Where(o => o.CodeDescription.Equals("Knowledge Field")).SelectMany(x => x.CodeContents)

            });
            
        }

        // GET: api/Codes/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Code>> GetCode(int id)
        {
            var code = await _context.Codes.FindAsync(id);

            if (code == null)
            {
                return NotFound();
            }

            return code;
        }

        // PUT: api/Codes/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutCode(int id, Code code)
        {
            if (id != code.Id)
            {
                return BadRequest();
            }

            _context.Entry(code).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!CodeExists(id))
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

        // POST: api/Codes
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Code>> PostCode(Code code)
        {
            _context.Codes.Add(code);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetCode", new { id = code.Id }, code);
        }

        // DELETE: api/Codes/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteCode(int id)
        {
            var code = await _context.Codes.FindAsync(id);
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
