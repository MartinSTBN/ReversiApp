using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ReversiApp.Models
{
    public class BordArrayValues
    {
        public int ID { get; set; }
        public int ArrayIndexX { get; set; }
        public int ArrayIndexY { get; set; }
        public int Value { get; set; }
        public int GameID { get; set; }
        public virtual Game Game { get; set; }
    }
}
