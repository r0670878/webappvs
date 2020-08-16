using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using webappvs.Data;
using webappvs.Models;

namespace webappvs.Controllers.api
{
    [Route("api/[controller]")]
    [ApiController]
    public class SpelController : Controller
    {
        private readonly ApplicationDbContext _context;

        public SpelController(ApplicationDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Spel>>> GetSpellen()
        {
            return await _context.Spellen.ToListAsync();
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Spel>> GetSpel(int id)
        {
            var spel = await _context.Spellen.FindAsync(id);

            if (spel == null)
            {
                return NotFound();
            }

            return spel;
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> PutProduct(int id, Spel spel)
        {
            if (id != spel.SpelID)
            {
                return BadRequest();
            }

            _context.Entry(spel).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!SpelExists(id))
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

        // POST: api/Product
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPost]
        public async Task<ActionResult<Spel>> PostSpel(Spel spel)
        {
            _context.Spellen.Add(spel);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetSpel", new { id = spel.SpelID }, spel);
        }

        // DELETE: api/Product/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<Spel>> DeleteSpel(int id)
        {
            var spel = await _context.Spellen.FindAsync(id);
            if (spel == null)
            {
                return NotFound();
            }

            _context.Spellen.Remove(spel);
            await _context.SaveChangesAsync();

            return spel;
        }

        private bool SpelExists(int id)
        {
            return _context.Spellen.Any(e => e.SpelID == id);
        }
    }
}
