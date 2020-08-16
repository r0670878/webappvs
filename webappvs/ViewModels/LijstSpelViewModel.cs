using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using webappvs.Models;

namespace webappvs.ViewModels
{
    public class LijstSpelViewModel
    {
        public string SpelZoeken { get; set; }
        public List<Spel> Spellen { get; set; }
    }
}
