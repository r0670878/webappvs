using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace webappvs.Models
{
    public class Bestelling
    {
        public int BestellingID { get; set; }
        public int KlantID { get; set; }
        public string beschrijving { get; set; }
        public decimal prijs { get; set; }
        public decimal? Korting { get; set; }
        public Klant Klant { get; set; }
        public ICollection<BestellingDetails> BestellingDetails { get; set; }
    }
}
