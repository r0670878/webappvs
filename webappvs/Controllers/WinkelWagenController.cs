using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using webappvs.Data;
using webappvs.Models;
using webappvs.ViewModels;

namespace webappvs.Controllers
{
    public class WinkelWagenController : Controller
    {

        private readonly ApplicationDbContext _context;
        private readonly IHttpContextAccessor _accessor;

        public WinkelWagenController(ApplicationDbContext context, IHttpContextAccessor accessor)
        {
            _accessor = accessor;
            _context = context;
        }

        private bool ShoppingCartExists(int id)
        {
            return _context.WinkelWagens.Any(e => e.WinkelWagenID == id);
        }

        private string GetCurrentUserId()
        {
            return _accessor.HttpContext.User.FindFirst(ClaimTypes.NameIdentifier).Value;
        }

        [HttpPost, ActionName("SendBestellling")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> SendBestelling()
        {
            var winkelWagenDetails = _context.WinkelWagenDetails.ToList();


            InsertBestelling(winkelWagenDetails);
            DeleteCurrentItems();

            return RedirectToAction("Index", "WinkelWagenDetails");
        }

        public void DeleteCurrentItems()
        {
            var klantID = _context.Klanten
                .Where(x => x.UserID == GetCurrentUserId())
                .FirstOrDefault();

            var winkelWagenID = _context.WinkelWagens.Where(x => x.KlantID == klantID.KlantID).FirstOrDefault();

            _context.WinkelWagens.Remove(winkelWagenID);
            _context.SaveChanges();
        }

        public void InsertBestelling(List<WinkelWagenDetails> _winkelWagen)
        {
            var klantID = _context.Klanten
                .Where(x => x.UserID == GetCurrentUserId())
                .FirstOrDefault();

            var winkelwagenID = _context.WinkelWagens.Where(x => x.KlantID == klantID.KlantID).FirstOrDefault();

            var winkelWagenDetails = _context.WinkelWagenDetails
                .Where(x => x.WinkelWagenID == winkelwagenID.WinkelWagenID).ToList();

            Bestelling newBestelling = new Bestelling();
            newBestelling.KlantID = klantID.KlantID;
            newBestelling.prijs = newBestelling.prijs;
            newBestelling.Korting = 0;
            newBestelling.beschrijving = "game";
            _context.Bestellingen.Add(newBestelling);
            _context.SaveChanges();


            foreach (WinkelWagenDetails item in winkelWagenDetails)
            {
                BestellingDetails newBestellingDetail = new BestellingDetails();
                newBestellingDetail.BestellingID = newBestelling.BestellingID;
                newBestellingDetail.SpelNaam = item.Spelnaam;
                newBestellingDetail.SpelID = item.SpelID;
                newBestellingDetail.Aantal = item.Aantal;
                _context.BestellingDetails.Add(newBestellingDetail);
                _context.SaveChanges();

            }
        }
    }
}
