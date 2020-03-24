using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
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
            //Check if user is logged in
            if (user != null)
            {
                game = await _context.Game.FirstOrDefaultAsync(m => m.GameID == id);
                //Check if game exists, else NotFound
                if(game != null)
                {
                    var playersInGame = await _context.Speler.Where(m => m.GameID == game.GameID).ToListAsync();
                    
                    game.Spelers = new List<Speler>();
                    var userExists = await _context.Speler.FirstOrDefaultAsync(m => m.Id == user.Id);

                    //Check if game is already full or if the player is in this game
                    if (playersInGame.Count < 2 || userExists != null)
                    {
                        //Check if user is already a player and add them to the game
                        if (userExists == null)
                        {
                            Speler speler = new Speler();
                            speler.Id = user.Id;
                            speler.Email = user.Email;
                            speler.Password = user.PasswordHash;
                            speler.Token = game.Token;
                            speler.Kleur = Kleur.Zwart;
                            speler.GameID = game.GameID;

                            var waitingPlayer = await _context.Speler.FirstOrDefaultAsync(m => m.GameID == game.GameID);
                            _context.Add(speler);
                            _context.Add(waitingPlayer);
                            await _context.SaveChangesAsync();
                        }
                        //Adds the players to the game
                        var spelers = await _context.Speler.Where(m => m.GameID == game.GameID).ToListAsync();
                        foreach (var speler in spelers)
                        {
                            game.Spelers.Add(speler);
                        }
                    }
                    else
                    {
                        return RedirectToAction("Index", "Games");
                    } 
                }
                else
                {
                    return NotFound();
                } 
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