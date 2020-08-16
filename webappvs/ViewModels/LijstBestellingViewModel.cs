using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using webappvs.Models;

namespace webappvs.ViewModels
{
    public class LijstBestellingViewModel
    {
        public string BestellingZoeken { get; set; }
        public List<Bestelling> Bestellingen { get; set; }
    }
}
