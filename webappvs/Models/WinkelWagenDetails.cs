using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace webappvs.Models
{
    public class WinkelWagenDetails
    {
        public int WinkelWagenDetailsID { get; set; }
        public int WinkelWagenID { get; set; }
        public int? Aantal { get; set; }
        public string Spelnaam { get; set; }
        public int SpelID { get; set; }
        public Spel Spel { get; set; }
        public WinkelWagen WinkelWagen { get; set; }
    }
}
