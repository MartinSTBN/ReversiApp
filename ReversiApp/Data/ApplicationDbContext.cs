using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using ReversiApp.Models;

namespace ReversiApp.Data
{
    public class ApplicationDbContext : IdentityDbContext<IdentityUser>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }
        public DbSet<Game> Game { get; set; }
        public DbSet<Speler> Speler { get; set; }
        public DbSet<BordArrayValues> BordArrayValues { get; set; }
        public DbSet<ReversiApp.Models.Highscore> Highscore { get; set; }
    }
}
