using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace webappvs.Models
{
    public class AuthenticateModel
    {
        public string Id { get; set; }
        [Required]
        public string Gebruikersnaam { get; set; }
        [Required]
        public string Wachtwoord { get; set; }
        public string Token  { get; set; }
    }
}
