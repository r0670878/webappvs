using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace webappvs.Models
{
    public class BestellingDetails
    {
        public BestellingDetails BestellingDetailsID { get; set; }
        public int BestellingID { get; set; }
        public int? Aantal { get; set; }
        public string SpelNaam { get; set; }
        public int SpelID { get; set; }
        public Bestelling Bestelling { get; set; }
        public Spel Spel { get; set; }
    }
}
