using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using webappvs.Data;
using webappvs.Models;
using webappvs.ViewModels;

namespace webappvs.Controllers
{
    public class BestellingController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly IHttpContextAccessor _accessor;

        public BestellingController(ApplicationDbContext context, IHttpContextAccessor accessor)
        {
            _accessor = accessor;
            _context = context;
        }

        // GET: Order
        public async Task<IActionResult> Index()
        {
            var viewModel = new LijstBestellingViewModel();

            viewModel.Bestellingen = await _context.Bestellingen.Include(k => k.Klant).ToListAsync();

            return View(viewModel);
        }

        public async Task<IActionResult> Search(LijstBestellingViewModel viewModel)
        {
            if (!string.IsNullOrEmpty(viewModel.BestellingZoeken))
            {
                viewModel.Bestellingen = await _context.Bestellingen.Include(k => k.Klant)
                    .Where(b => b.beschrijving.StartsWith(viewModel.BestellingZoeken)).ToListAsync();
            }
            else
            {
                viewModel.Bestellingen = await _context.Bestellingen.Include(k => k.Klant).ToListAsync();
            }

            return View("Index", viewModel);
        }


        public async Task<IActionResult> KlantBestellingen()
        {
            var klantID = _context.Klanten
                .Where(x => x.UserID == GetCurrentUserId())
                .FirstOrDefault();

            var klantBestellingen = _context.Bestellingen.Where(x => x.KlantID == klantID.KlantID);

            return View(await klantBestellingen.ToListAsync());
        }

        // GET: Order/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var klantID = _context.Klanten
                .Where(x => x.UserID == GetCurrentUserId())
                .FirstOrDefault();

            var bestellingID = _context.Bestellingen.Where(b => b.BestellingID == id).FirstOrDefault();

            var bestellingdetail = _context.BestellingDetails
                .Include(b => b.Bestelling)
                .Include(s => s.Spel)
                .Where(m => m.BestellingID == bestellingID.BestellingID);

            if (bestellingID == null)
            {
                return NotFound();
            }

            return View(await bestellingdetail.ToListAsync());
        }


        // GET: Order/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var bestelling = await _context.Bestellingen.FindAsync(id);
            if (bestelling == null)
            {
                return NotFound();
            }
            ViewData["KlantID"] = new SelectList(_context.Klanten, "KlantID", "KlantID", bestelling.KlantID);
            return View(bestelling);
        }

        // POST: Order/Edit/5

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("BestellingID,KlantID,Beschrijving,Prijs,Korting")] Bestelling bestelling)
        {
            if (id != bestelling.BestellingID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(bestelling);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!BestellingBestaat(bestelling.BestellingID))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            ViewData["KlantID"] = new SelectList(_context.Klanten, "KlantID", "KlantID", bestelling.KlantID);
            return View(bestelling);
        }

        // GET: Order/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var bestelling = await _context.Bestellingen
                .Include(k => k.Klant)
                .FirstOrDefaultAsync(k => k.KlantID == id);
            if (bestelling == null)
            {
                return NotFound();
            }

            return View(bestelling);
        }

        // POST: Order/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var bestelling = await _context.Bestellingen.FindAsync(id);
            _context.Bestellingen.Remove(bestelling);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool BestellingBestaat(int id)
        {
            return _context.Bestellingen.Any(b => b.BestellingID == id);
        }

        private string GetCurrentUserId()
        {
            return _accessor.HttpContext.User.FindFirst(ClaimTypes.NameIdentifier).Value;
        }
    }
}
