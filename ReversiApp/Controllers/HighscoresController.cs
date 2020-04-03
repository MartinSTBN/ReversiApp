using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using ReversiApp.Data;
using ReversiApp.Models;

namespace ReversiApp.Controllers
{
    
    public class HighscoresController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<IdentityUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;

        public HighscoresController(ApplicationDbContext context, UserManager<IdentityUser> userManager, RoleManager<IdentityRole> roleManager)
        {
            _context = context;
            _userManager = userManager;
            _roleManager = roleManager;
        }

        // GET: Highscores
        public IActionResult Index()
        {
            var users =  _userManager.Users.ToList();
            var highScores = new List<Highscore>();
            foreach (var user in users)
            {
                var scores = _context.Highscore.Where(r => r.UserID == user.Id).ToList();

                
                foreach (var score in scores)
                {
                    float winLossRatio = 0;
                    if (score.Losses > 0)
                    {
                        winLossRatio = (float)score.Wins / (float)score.Losses;
                    }

                    var hs = new Highscore()
                    {
                        ID = score.ID,
                        UserID = user.Id,
                        UserName = user.UserName,
                        Wins = score.Wins,
                        Losses = score.Losses,
                        WinLossRatio = winLossRatio
                    };
                    highScores.Add(hs);
                }

                
            }
            List<Highscore> highScoresOrdered = highScores.OrderByDescending(o => o.WinLossRatio).ToList();
            return View(highScoresOrdered);
        }


        [Authorize(Roles = "Administrator")]
        // GET: Highscores/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var highscore = await _context.Highscore.FindAsync(id);
            if (highscore == null)
            {
                return NotFound();
            }
            return View(highscore);
        }

        // POST: Highscores/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Administrator")]
        public async Task<IActionResult> Edit(int id, [Bind("ID,Wins,Losses,UserID")] Highscore highscore)
        {
            if (id != highscore.ID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                _context.Update(highscore);
                await _context.SaveChangesAsync();
                
                return RedirectToAction(nameof(Index));
            }
            return View(highscore);
        }

       
    }
}
