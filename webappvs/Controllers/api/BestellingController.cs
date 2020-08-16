using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using webappvs.Data;
using webappvs.Models;

namespace webappvs.Controllers.api
{
    [Route("api/[controller]")]
    public class BestellingController : Controller
    {
        private readonly ApplicationDbContext _context;

        public BestellingController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: api/<controller>
        [HttpGet]
        public IEnumerable<Bestelling> Get()
        {

            return _context.Bestellingen.Include(x => x.Klant).Include(d => d.BestellingDetails).ToList();

        }

        // GET api/<controller>/5
        [HttpGet("{id}")]
        public Bestelling Get(int id)
        {
            return _context.Bestellingen.Find(id);
        }

        [HttpGet("orders")]
        public IEnumerable<Bestelling> GetOrders()
        {
            return _context.Bestellingen.ToList();
        }
        // POST api/<controller>
        [HttpPost]
        public ActionResult<Bestelling> PostOrder([FromBody] Bestelling value)
        {
            _context.Bestellingen.Add(value);
            _context.SaveChangesAsync();
            return CreatedAtAction(nameof(Get), new { id = value.BestellingID }, value);
        }

        // PUT api/<controller>/5
        [HttpPut("{id}")]
        public ActionResult PutBestelling(int id, [FromBody] Bestelling value)
        {
            if (id != value.BestellingID)
            {
                return BadRequest();
            }

            var OldBestelling = _context.Bestellingen.Find(id);


            OldBestelling.Korting = value.Korting;
            OldBestelling.prijs = value.prijs;
            OldBestelling.beschrijving = value.beschrijving;

            _context.SaveChangesAsync();

            return NoContent();
        }

        // DELETE api/<controller>/5
        [HttpDelete("{id}")]
        public ActionResult DeleteBestelling(int id)
        {
            var bestelling = _context.Bestellingen.Find(id);

            if (bestelling == null)
            {
                return NotFound();
            }

            _context.Bestellingen.Remove(bestelling);
            _context.SaveChanges();

            return NoContent();

        }
    }
}
