using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace ReversiApp.Models
{
    public class BordArrayValues
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ID { get; set; }
        public int ArrayIndexX { get; set; }
        public int ArrayIndexY { get; set; }
        public int Value { get; set; }
        [DatabaseGenerated(DatabaseGeneratedOption.Computed)]
        public int GameID { get; set; }
        
    }
}
