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

            Game game = _context.Game.Find(Convert.ToInt32(id));

            BordArrayValues bordValues = new BordArrayValues();
            

            var split = cellId.Split();
            var row = Convert.ToInt32(split[0]);
            var column = Convert.ToInt32(split[1]);
            bool zetMogelijk = game.DoeZet(row, column);
            if (zetMogelijk)
            {
                _context.Update(game);
                await _context.SaveChangesAsync();
                await Clients.All.SendAsync("ReceiveGameData", cellId, game.AandeBeurt, zetMogelijk);
            }
            else
            {
                await Clients.All.SendAsync("ReceiveGameData", cellId, game.AandeBeurt, zetMogelijk);
            }
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
