using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using webappvs.Data;
using webappvs.Models;
using webappvs.ViewModels;

namespace webappvs.Controllers
{
    public class SpelController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly IHttpContextAccessor _accessor;


        public SpelController(ApplicationDbContext context, IHttpContextAccessor accessor)
        {
            _accessor = accessor;

            _context = context;
        }

        // GET: Product
        public async Task<IActionResult> Index()
        {
            var viewModel = new LijstSpelViewModel();


            viewModel.Spellen = await _context.Spellen.ToListAsync();
            return View(viewModel);
        }

        public async Task<IActionResult> Search(LijstSpelViewModel viewModel)
        {
            if (!string.IsNullOrEmpty(viewModel.SpelZoeken))
            {
                viewModel.Spellen = await _context.Spellen
                    .Where(b => b.SpelNaam.StartsWith(viewModel.SpelZoeken)).ToListAsync();
            }
            else
            {
                viewModel.Spellen = await _context.Spellen.ToListAsync();
            }

            return View("Index", viewModel);
        }

        // GET: Product/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var spel = await _context.Spellen
                .FirstOrDefaultAsync(m => m.SpelID == id);
            if (spel == null)
            {
                return NotFound();
            }

            return View(spel);
        }

        // GET: Product/Create
        [Authorize(Roles = "Administrator")]
        public IActionResult Create()
        {
            return View();
        }

        // POST: Product/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("SpelID,Spelnaam,Soort,Console")] Spel spel)
        {
            if (ModelState.IsValid)
            {
                _context.Add(spel);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(spel);
        }

        // GET: Product/Edit/5
        [Authorize(Roles = "Administrator")]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var spel = await _context.Spellen.FindAsync(id);
            if (spel == null)
            {
                return NotFound();
            }
            return View(spel);
        }

        // POST: Product/Edit/5

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("SpelID,Spelnaam,Soort,Console")] Spel spel)
        {
            if (id != spel.SpelID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(spel);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!SpelExists(spel.SpelID))
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
            return View(spel);
        }

        // GET: Product/Delete/5
        [Authorize(Roles = "Administrator")]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var spel = await _context.Spellen
                .FirstOrDefaultAsync(m => m.SpelID == id);
            if (spel == null)
            {
                return NotFound();
            }

            return View(spel);
        }

        // POST: Product/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var spel = await _context.Spellen.FindAsync(id);
            _context.Spellen.Remove(spel);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }



        private bool SpelExists(int id)
        {
            return _context.Spellen.Any(e => e.SpelID == id);
        }

        private string GetCurrentUserId()
        {

            return _accessor.HttpContext.User.FindFirst(ClaimTypes.NameIdentifier).Value;

        }

        [HttpPost, ActionName("AddProduct")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddSpel(int spelID)
        {

            if (User.Identity.IsAuthenticated)
            {
                var klantID = _context.Klanten
                .Where(x => x.UserID == GetCurrentUserId())
                .FirstOrDefault();

                var winkelWagenID = _context.WinkelWagens.Where(x => x.KlantID == klantID.KlantID).FirstOrDefault();

                if (winkelWagenID == null)
                {
                    _context.WinkelWagens.Add(new WinkelWagen() { KlantID = klantID.KlantID });
                    _context.SaveChanges();

                    var spel = _context.Spellen.Where(x => x.SpelID == spelID).FirstOrDefault();

                    var WinkelWagenNLeeg = _context.WinkelWagens.Where(x => x.KlantID == klantID.KlantID).FirstOrDefault();


                    _context.WinkelWagenDetails.Add(new WinkelWagenDetails
                    {
                        WinkelWagenID = WinkelWagenNLeeg.WinkelWagenID,
                        SpelID = spel.SpelID,
                        Spelnaam = spel.SpelNaam,
                        Aantal = 1

                    });

                    _context.SaveChanges();

                }
                else
                {
                    var spel = _context.Spellen.Where(x => x.SpelID == spelID).FirstOrDefault();

                    _context.WinkelWagenDetails.Add(new WinkelWagenDetails
                    {
                        WinkelWagenID = winkelWagenID.WinkelWagenID,
                        SpelID = spel.SpelID,
                        Spelnaam = spel.SpelNaam,
                        Aantal = 1

                    });

                    _context.SaveChanges();

                    return RedirectToAction(nameof(BestellingController.Index), "Bestelling");
                }


            }

            return RedirectToAction(nameof(BestellingController.Index), "Bestelling");

        }
    }
}
