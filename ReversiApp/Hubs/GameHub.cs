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

        public GameHub(ApplicationDbContext context, UserManager<IdentityUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }
        public async Task SendMessage(string user, string message)
        {
            await Clients.All.SendAsync("ReceiveMessage", user, message);
        }

        public async Task SendGameData(string speler, string cellId, string result)
        {
            await Clients.All.SendAsync("ReceiveGameData", speler, cellId, result);
        }

        public async Task SendSpelers(string spelers)
        {
            await Clients.All.SendAsync("ReceiveSpelers", spelers);
        }

        public async Task SendPlayState(string state)
        {
            await Clients.All.SendAsync("ReceivePlayState", state);
        }

    }
}
