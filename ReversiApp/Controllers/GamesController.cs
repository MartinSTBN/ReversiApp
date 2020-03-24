using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using ReversiApp.Data;
using ReversiApp.Models;

namespace ReversiApp.Controllers
{
    public class GamesController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<IdentityUser> _userManager;

        public GamesController(ApplicationDbContext context, UserManager<IdentityUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        // GET: Games
        public async Task<IActionResult> Index()
        {
            IdentityUser user = await _userManager.GetUserAsync(User);

            //Checks if user is logged in
            if (user != null)
            {
                var speler = await _context.Speler.FindAsync(user.Id);
                //Checks if user is already in game
                if (speler != null)
                {
                    var game = await _context.Game.FindAsync(speler.GameID);
                    return LocalRedirect("/Game/Reversi/" + game.GameID);
                }
            }
            else
            {
                return LocalRedirect("/Identity/Account/Login");
            }

            //Checks if games are joinable => already contain 2 players
            List<Game> games = await _context.Game.ToListAsync();
            List<Game> joinableGames = new List<Game>();

            foreach (var game in games)
            {
                var playersInGame = await _context.Speler.Where(m => m.GameID == game.GameID).ToListAsync();
                if(playersInGame.Count < 2)
                {
                    joinableGames.Add(game);
                }
            }
            
            return View(joinableGames);
        }

        // GET: Games/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var game = await _context.Game
                .FirstOrDefaultAsync(m => m.GameID == id);
            if (game == null)
            {
                return NotFound();
            }

            return View(game);
        }

        // GET: Games/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Games/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("GameID,Omschrijving,Token,AandeBeurt")] Game game)
        {
            if (ModelState.IsValid)
            {
                game.AandeBeurt = Kleur.Zwart;
                _context.Add(game);
                await _context.SaveChangesAsync();
                Hash hash = new Hash();
                Salt salt = new Salt();
                game.Token = hash.Create(game.GameID.ToString(), salt.Create());


                IdentityUser user = await _userManager.GetUserAsync(User);
                Speler speler = new Speler();
                speler.Id = user.Id;
                speler.Email = user.Email;
                speler.Password = user.PasswordHash;
                speler.Token = game.Token;
                speler.Kleur = Kleur.Wit;
                speler.GameID = game.GameID;

                _context.Add(speler);
                await _context.SaveChangesAsync();

                game.Spelers = new List<Speler>();
                game.Spelers.Add(speler);

                //Create the playing board
                BordArrayValues bordValues = new BordArrayValues();
                for (int i = 0; i < game.Bord.GetUpperBound(0); i++)
                {
                    for (int j = 0; j < game.Bord.GetUpperBound(1); j++)
                    {
                        bordValues.ArrayIndexX = i;
                        bordValues.ArrayIndexY = j;
                        bordValues.Value = Convert.ToInt32(game.AandeBeurt);
                        bordValues.GameID = 50;
                        await _context.AddAsync(bordValues);
                        await _context.SaveChangesAsync();
                    }
                }





                return LocalRedirect("/Game/Reversi/" + game.GameID);
            }
            return View(game);
        }

        // GET: Games/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var game = await _context.Game.FindAsync(id);
            if (game == null)
            {
                return NotFound();
            }
            return View(game);
        }

        // POST: Games/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("GameID,Omschrijving,Token,AandeBeurt")] Game game)
        {
            if (id != game.GameID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(game);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!GameExists(game.GameID))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(game);
        }

        // GET: Games/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var game = await _context.Game
                .FirstOrDefaultAsync(m => m.GameID == id);
            if (game == null)
            {
                return NotFound();
            }

            return View(game);
        }

        // POST: Games/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            
            var game = await _context.Game.FindAsync(id);
            IdentityUser user = await _userManager.GetUserAsync(User);
            var speler = await _context.Speler.FirstOrDefaultAsync(m => m.GameID == game.GameID);


            _context.Speler.Remove(speler);
            _context.Game.Remove(game);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool GameExists(int id)
        {
            return _context.Game.Any(e => e.GameID == id);
        }
    }
}
