using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using ReversiApp.Data;
using ReversiApp.Models;

namespace ReversiApp.Controllers
{
    public class GameController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<IdentityUser> _userManager;

        public GameController(ApplicationDbContext context, UserManager<IdentityUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        [HttpGet("/Game/Reversi/{id}")]
        public async Task<IActionResult> Reversi(int id)
        {
            Game game = new Game();
            IdentityUser user = await _userManager.GetUserAsync(User);
            if (user != null)
            {
                game = await _context.Game.FirstOrDefaultAsync(m => m.GameID == id);
                game.Spelers = new List<Speler>();
                var spelerExists = await _context.Speler.FirstOrDefaultAsync(m => m.Id == user.Id);
                if (spelerExists == null)
                {
                    Speler speler = new Speler();
                    speler.Id = user.Id;
                    speler.Email = user.Email;
                    speler.Password = user.PasswordHash;
                    speler.Token = game.Token;
                    speler.Kleur = Kleur.Zwart;
                    speler.GameID = game.GameID;

                    _context.Add(speler);
                    await _context.SaveChangesAsync();
                }
                var spelers = await _context.Speler.Where(m => m.GameID == game.GameID).ToListAsync();
            }
            else
            {
                return LocalRedirect("/Identity/Account/Login");
            }
            return View(game);
        }

        /*[HttpPost]
        public JsonResult Reversi(string kleur)
        {
            return Json(kleur);
        }*/
    }
}