using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using webappvs.Data;

namespace webappvs.Controllers
{
    public class WinkelWagenDetailsController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly IHttpContextAccessor _accessor;


        public WinkelWagenDetailsController(ApplicationDbContext context, IHttpContextAccessor accessor)
        {
            _context = context;
            _accessor = accessor;
        }

        // GET: ShoppingCartDetail
        public async Task<IActionResult> Index()
        {
            var klantID = _context.Klanten
                .Where(x => x.UserID == GetCurrentUserId())
                .FirstOrDefault();

            var winkelWagenID = _context.WinkelWagens.Where(x => x.KlantID == klantID.KlantID).FirstOrDefault();

            if (winkelWagenID == null)
            {
                return RedirectToAction("Index", "Home");
            }

            var applicationDbContext = _context.WinkelWagenDetails.Include(s => s.Spel).Include(s => s.WinkelWagen).Where(x => x.WinkelWagenID == winkelWagenID.WinkelWagenID);

            if (applicationDbContext == null)
            {
                return RedirectToAction("Index", "Home");
            }

            return View(await applicationDbContext.ToListAsync());
        }



        private string GetCurrentUserId()
        {
            return _accessor.HttpContext.User.FindFirst(ClaimTypes.NameIdentifier).Value;
        }



        // GET: ShoppingCartDetail/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var winkelWagenDetails = await _context.WinkelWagenDetails
                .Include(s => s.Spel)
                .Include(s => s.WinkelWagen)
                .FirstOrDefaultAsync(m => m.WinkelWagenDetailsID == id);
            if (winkelWagenDetails == null)
            {
                return NotFound();
            }

            return View(winkelWagenDetails);
        }

        // POST: ShoppingCartDetail/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var winkelWagenDetail = await _context.WinkelWagenDetails.FindAsync(id);
            _context.WinkelWagenDetails.Remove(winkelWagenDetail);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool WinkelWagenDetailExists(int id)
        {
            return _context.WinkelWagenDetails.Any(e => e.WinkelWagenDetailsID == id);
        }
    }
}
