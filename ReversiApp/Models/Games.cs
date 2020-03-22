using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ReversiApp.Models
{
    public class Games
    {
        public int ID { get; set; }
        public int GameID { get; set; }
        public virtual Game Game { get; set; }
    }
}
