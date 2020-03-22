using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using ReversiApp.Models;

namespace ReversiApp.Controllers
{
    public class GameController : Controller
    {
        public IActionResult Reversi()
        {
            Game game = new Game();
            game.GameID = 0;
            game.Omschrijving = "Reversi";
            game.Token = "";
            game.Spelers = new List<Speler>();
            game.AandeBeurt = Kleur.Geen;

            return View(game);
        }

        [HttpPost]
        public JsonResult Reversi(string kleur)
        {
            return Json(kleur);
        }
    }
}