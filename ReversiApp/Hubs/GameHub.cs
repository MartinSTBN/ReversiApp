using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.SignalR;
using ReversiApp.Data;
using ReversiApp.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ReversiApp
{
    public class GameHub : Hub
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<IdentityUser> _userManager;
        private Game game;

        public GameHub(ApplicationDbContext context, UserManager<IdentityUser> userManager)
        {
            _context = context;
            _userManager = userManager;
            game = new Game();
        }
        public async Task SendMessage(string user, string message)
        {
            await Clients.All.SendAsync("ReceiveMessage", user, message);
        }

        public async Task SendGameData(string cellId, string color, string id)
        {
            var gameId = Convert.ToInt32(id);
            Game game = _context.Game.Find(Convert.ToInt32(id));
            var split = cellId.Split();
            var row = Convert.ToInt32(split[0]);
            var column = Convert.ToInt32(split[1]);

            //Check if move is possible on new game situation
            var values =  _context.BordArrayValues.Where(item => item.GameID == game.GameID).ToList();
            foreach (var item in values)
            {
                game.Bord[item.Row, item.Column] = (Kleur)item.Value;
            }
            bool zetMogelijk = game.DoeZet(row, column);

            if (zetMogelijk)
            {
                //Update bord stukken te slaan
                foreach(var item in game.stukkenTeSlaan)
                {
                    var splitTeSlaan = item.Split(",");
                    var rowTeSlaan = Convert.ToInt32(splitTeSlaan[0]);
                    var columnTeSlaan = Convert.ToInt32(splitTeSlaan[1]);
                    var value = _context.BordArrayValues.FirstOrDefault(item => item.GameID == game.GameID && item.Row == rowTeSlaan && item.Column == columnTeSlaan);

                    if (color == "Wit") { value.Value = 1; } else if (color == "Zwart") { value.Value = 2; }
                    _context.Update(value);
                    await _context.SaveChangesAsync();
                }
                
                //Update bord stuk gezet
                var gezet = _context.BordArrayValues.FirstOrDefault(item => item.GameID == game.GameID && item.Row == row && item.Column == column);
                if (color == "Wit") { gezet.Value = 1; } else if (color == "Zwart") { gezet.Value = 2; }
                _context.Update(gezet);
                await _context.SaveChangesAsync();

                _context.Update(game);
                await _context.SaveChangesAsync();
                await Clients.All.SendAsync("ReceiveGameData", cellId, game.AandeBeurt, zetMogelijk, game.stukkenTeSlaan);
            }
            else
            {
                await Clients.All.SendAsync("ReceiveGameData", cellId, game.AandeBeurt, zetMogelijk, game.stukkenTeSlaan);
            }
        }

        public async Task GetBordData(string id)
        {
            Game game = _context.Game.Find(Convert.ToInt32(id));
            

            var values = new List<BordArrayValues>();
            var bordData =  _context.BordArrayValues.Where(m => m.GameID == game.GameID).ToList();
            foreach (var value in bordData)
            {
                values.Add(value);
            }

            await Clients.All.SendAsync("ReceiveBordData", values, game.AandeBeurt);
        }

        public async Task Test(string cellId, string color, string id)
        {
            Game game = _context.Game.Find(Convert.ToInt32(id));
            var split = cellId.Split();
            var row = Convert.ToInt32(split[0]);
            var column = Convert.ToInt32(split[1]);
            bool zetMogelijk = game.DoeZet(row, column);

            await Clients.All.SendAsync("Test", row, column, zetMogelijk);
        }
    }
}
