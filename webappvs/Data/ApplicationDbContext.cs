using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using webappvs.Models;

namespace webappvs.Data
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {
        }

        public DbSet<ApplicationUser> ApplicationUsers { get; set; }
        public DbSet<Klant> Klanten { get; set; }
        public DbSet<Bestelling> Bestellingen { get; set; }
        public DbSet<BestellingDetails> BestellingDetails { get; set; }
        public DbSet<Spel> Spellen { get; set; }
        public DbSet<WinkelWagen> WinkelWagens { get; set; }
        public DbSet<WinkelWagenDetails> WinkelWagenDetails { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.Entity<Klant>().ToTable("Klant").HasMany(b => b.Bestellingen).WithOne(k => k.Klant).OnDelete(DeleteBehavior.Cascade);

            builder.Entity<Bestelling>().ToTable("Bestelling").HasMany(d => d.BestellingDetails).WithOne(b => b.Bestelling).OnDelete(DeleteBehavior.Cascade);

            builder.Entity<BestellingDetails>().ToTable("BestellinDetails").HasOne(b => b.Bestelling).WithMany(d => d.BestellingDetails).OnDelete(DeleteBehavior.Cascade);

            builder.Entity<Spel>().ToTable("Spel").HasMany(d => d.BestellingDetails).WithOne(s => s.Spel).OnDelete(DeleteBehavior.Cascade);

            builder.Entity<WinkelWagen>().ToTable("WinkelWagen");

            builder.Entity<WinkelWagenDetails>().ToTable("WinkelWagenDetails");

            builder.Entity<ApplicationUser>().ToTable("ApplicationUser");

        }
    }
}
