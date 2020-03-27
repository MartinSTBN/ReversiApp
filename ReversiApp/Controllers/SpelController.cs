using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
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

        [HttpGet]
        [Route("zetmogelijk/{id}")]
        public async Task<string> Beurt(int id)
        {
            var game = _context.Game.Find(id);
            game.AandeBeurt = Kleur.Zwart;
            _context.Update(game);
            await _context.SaveChangesAsync();

            JArray array = new JArray();
            array.Add("Manual text");
            array.Add(new DateTime(2000, 5, 23));

            JArray anotherArray = new JArray();
            anotherArray.Add("Manual text");
            anotherArray.Add(new DateTime(2000, 5, 23));

            JObject o = new JObject();
            o["MyArray"] = array;
            o["anotherArray"] = array;

            string json = o.ToString();

            return json;
        }


        [HttpGet]
        [Route("state/{id}")]
        public async Task<string> State(int id)
        {
            var game = _context.Game.Find(id);
            var values = _context.BordArrayValues.Where(item => item.GameID == game.GameID).ToList();

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