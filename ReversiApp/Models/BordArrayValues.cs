using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace ReversiApp.Models
{
    public class BordArrayValues
    {
        public int ID { get; set; }
        public int Row { get; set; }
        public int Column { get; set; }
        public int Value { get; set; }
        public int GameID { get; set; }
        
    }
}
