using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using ReversiApp.Data;
using ReversiApp.Models;

namespace ReversiApp.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SpelController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<IdentityUser> _userManager;
        private Game game;

        public SpelController(ApplicationDbContext context, UserManager<IdentityUser> userManager)
        {
            _context = context;
            _userManager = userManager;
            game = new Game();
        }

        [HttpPost]
        [Route("joinstatus/{id}")]
        public async Task<string> SetAcceptStatus(int id, [FromForm]string status)
        {

            var game = await _context.Game.FindAsync(id);

            game.JoinAccepteerStatus = "Akkoord";

            _context.Update(game);
            await _context.SaveChangesAsync();

            string json = JsonConvert.SerializeObject(game.JoinAccepteerStatus, Formatting.Indented);

            return json;
        }

        [HttpGet]
        [Route("joinstatus/{id}")]
        public async Task<string> GetAcceptStatus(int id)
        {

            var game = await _context.Game.FindAsync(id);

            string json = JsonConvert.SerializeObject(game.JoinAccepteerStatus, Formatting.Indented);

            return json;
        }

        [HttpPost]
        [Route("join/{id}")]
        public async Task<string> JoinGame(int id, [FromForm]string email)
        {

            var game = await _context.Game.FindAsync(id);
            var user = await _userManager.Users.FirstOrDefaultAsync(r => r.Email == email);
            game.SpelerDieWiltJoinen = user.UserName;

            _context.Update(game);
            await _context.SaveChangesAsync();

            string json = JsonConvert.SerializeObject(game.SpelerDieWiltJoinen, Formatting.Indented);

            return json;
        }

        [HttpGet]
        [Route("join/{id}")]
        public async Task<string> GetJoinStatus(int id)
        {

            var game = await _context.Game.FindAsync(id);

            string json = JsonConvert.SerializeObject(game.SpelerDieWiltJoinen, Formatting.Indented);

            return json;
        }

        [HttpGet]
        [Route("beurt/{id}")]
        public async Task<string> Beurt(int id)
        {
            var game = await _context.Game.FindAsync(id);
            var beurt = game.AandeBeurt;
            string json = JsonConvert.SerializeObject(beurt, Formatting.Indented);

            return json;
        }

        [HttpGet]
        [Route("aantalbezet/{id}")]
        public async Task<string> AantalBezet(int id)
        {
            Game game = await _context.Game.FindAsync(id);

            var values = _context.BordArrayValues.Where(item => item.GameID == game.GameID).ToList();
            var aantalLeeg = 0;
            var aantalWit = 0;
            var aantalZwart = 0;
            foreach (var item in values)
            {
                if(item.Row == 0 || item.Column == 0 || item.Row == 9 || item.Column == 9) { } else
                {
                    if (item.Value == 0) { aantalLeeg++; }
                    if (item.Value == 1) { aantalWit++; }
                    if (item.Value == 2) { aantalZwart++; }
                }   
            }
            int[] aantallen = new int[3];
            aantallen[0] = aantalLeeg;
            aantallen[1] = aantalZwart;
            aantallen[2] = aantalWit;
            string json = JsonConvert.SerializeObject(aantallen, Formatting.Indented);

            return json;
        }

        [HttpGet]
        [Route("aantalgeslagen/{id}")]
        public async Task<string> AantalGeslagen(int id)
        {
            Game game = await _context.Game.FindAsync(id);

            int[] values = new int[2];
            values[0] = game.aantalGeslagenDoorWit;
            values[1] = game.aantalGeslagenDoorZwart;
    
            string json = JsonConvert.SerializeObject(values, Formatting.Indented);

            return json;
        }

        [HttpGet]
        [Route("spelers/{id}")]
        public async Task<string> Spelers(int id)
        {
            Game game = await _context.Game.FindAsync(id);
            var spelers = await _context.Speler.Where(m => m.GameID == game.GameID).ToListAsync();
            var dict = new Dictionary<string, List<string>>();

            var count = 0;
            foreach (var speler in spelers)
            {
                List<string> values = new List<string>();
                values.Add(speler.Email);
                values.Add(Convert.ToInt32(speler.Kleur).ToString());
                dict.Add(count.ToString(), values);
                count++;
            }
            string json = JsonConvert.SerializeObject(dict, Formatting.Indented);

            return json;
        }

        [HttpGet]
        [Route("doezet/{id}/{speler}/{cellId}")]
        public async Task<string> Zet(int id, string speler, int cellId)
        {
            Game game = await _context.Game.FindAsync(id);
            int row = cellId / 10;
            int column = cellId % 10;

            //Check if move is possible on new game situation
            var values = _context.BordArrayValues.Where(item => item.GameID == game.GameID).ToList();
            foreach (var item in values)
            {
                game.Bord[item.Row, item.Column] = (Kleur)item.Value;
            }
            bool zetMogelijk = game.DoeZet(row, column);

            if (zetMogelijk)
            {
                //Update bord stukken te slaan
                foreach (var item in game.stukkenTeSlaan)
                {
                    var splitTeSlaan = item.Split(",");
                    var rowTeSlaan = Convert.ToInt32(splitTeSlaan[0]);
                    var columnTeSlaan = Convert.ToInt32(splitTeSlaan[1]);
                    var value = _context.BordArrayValues.FirstOrDefault(item => item.GameID == game.GameID && item.Row == rowTeSlaan && item.Column == columnTeSlaan);

                    if (speler == "Wit") { value.Value = 1; } else if (speler == "Zwart") { value.Value = 2; }
                    _context.Update(value);
                    await _context.SaveChangesAsync();
                }

                //Update bord stuk gezet
                var gezet = _context.BordArrayValues.FirstOrDefault(item => item.GameID == game.GameID && item.Row == row && item.Column == column);
                if (speler == "Wit") { gezet.Value = 1; } else if (speler == "Zwart") { gezet.Value = 2; }
                _context.Update(gezet);
                await _context.SaveChangesAsync();

                if (speler == "Wit") { game.aantalGeslagenDoorWit = game.stukkenTeSlaan.Count + game.aantalGeslagenDoorWit; } 
                else if (speler == "Zwart") { game.aantalGeslagenDoorZwart = game.stukkenTeSlaan.Count + game.aantalGeslagenDoorZwart; }

                _context.Update(game);
                await _context.SaveChangesAsync();
                string json = JsonConvert.SerializeObject(game.stukkenTeSlaan, Formatting.Indented);
                return json;
            }
            string empty = JsonConvert.SerializeObject(zetMogelijk, Formatting.Indented);
            return empty;

        }


        [HttpGet]
        [Route("state/{id}")]
        public async Task<string> State(int id)
        {
            var game = _context.Game.Find(id);
            var values = await _context.BordArrayValues.Where(item => item.GameID == game.GameID).ToListAsync();

            var dict = new Dictionary<string, List<Template>>();
            Template t = new Template();
            dict.Add("0", t.CreateList(values, 11, 19));
            dict.Add("1", t.CreateList(values, 21, 29));
            dict.Add("2", t.CreateList(values, 31, 39));
            dict.Add("3", t.CreateList(values, 41, 49));
            dict.Add("4", t.CreateList(values, 51, 59));
            dict.Add("5", t.CreateList(values, 61, 69));
            dict.Add("6", t.CreateList(values, 71, 79));
            dict.Add("7", t.CreateList(values, 81, 89));

            string json = JsonConvert.SerializeObject(dict, Formatting.Indented);

            return json;
        }

    }
    
    public class Template
    {
        public string row { get; set; }
        public string column { get; set; }
        public string value { get; set; }

        public List<Template> CreateList(List<BordArrayValues> values, int from, int too)
        {
            List<Template> list = new List<Template>();
            for (int i = from; i < too; i++)
            {
                var row = values[i].Row.ToString();
                var column = values[i].Column.ToString();
                var value = "";
                if (values[i].Value == 1) { value = "fiche-wit"; } else if (values[i].Value == 2) { value = "fiche-zwart"; }
                list.Add(new Template { row = row, column = column, value = value });
            }
            return list;
        }
    }
}