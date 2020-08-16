using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;

namespace webappvs.Models
{
    public class Spel
    {
        public int SpelID { get; set; }
        public string SpelNaam { get; set; }
        public string Soort { get; set; }
        public string Console { get; set; }
        public ICollection<BestellingDetails> BestellingDetails { get; set; }
        public ICollection<WinkelWagenDetails> WinkelWagenDetails { get; set; }
    }
}
