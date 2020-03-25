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
            bool zetMogelijk = game.DoeZet(row, column);

            if (zetMogelijk)
            {
                //Update bord value in database
                var value = _context.BordArrayValues.FirstOrDefault(item => item.GameID == game.GameID && item.ArrayIndexX == row && item.ArrayIndexY == column);

                if (color == "Wit")
                {
                    value.Value = 1;
                }
                else
                {
                    value.Value = 2;      
                }
                _context.Update(value);
                _context.Update(game);
                await _context.SaveChangesAsync();
                await Clients.All.SendAsync("ReceiveGameData", cellId, game.AandeBeurt, zetMogelijk);
            }
            else
            {
                await Clients.All.SendAsync("ReceiveGameData", cellId, game.AandeBeurt, zetMogelijk);
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
