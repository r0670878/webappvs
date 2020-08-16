using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace webappvs.Models
{
    public class Klant
    {
        [ForeignKey("ApplicationUser")]
        public string UserID { get; set; }
        public int KlantID { get; set; }
        [PersonalData]
        [DisplayName("Achternaam")]
        public string Naam { get; set; }
        [PersonalData]
        [DisplayName("Voornaam")]
        public string Voornaam { get; set; }
        public string Adres { get; set; }
        public int MyProperty { get; set; }
        public ApplicationUser ApplicationUser { get; set; }
        public ICollection<Bestelling> Bestellingen { get; set; }

    }
}
