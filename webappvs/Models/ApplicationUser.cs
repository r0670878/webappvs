using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace webappvs.Models
{
    public class ApplicationUser
    {
        [PersonalData]
        public Klant Klant { get; set; }
    }
}
