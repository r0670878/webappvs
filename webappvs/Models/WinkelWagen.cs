using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace webappvs.Models
{
    public class WinkelWagen
    {
        public int WinkelWagenID { get; set; }
        public int KlantID { get; set; }
        public ICollection<WinkelWagenDetails> WinkelWagenDetails { get; set; }
        public Klant Klant { get; set; }
    }
}
