using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace ReversiApp.Models
{
    public class Highscore
    {
        public int ID { get; set; }
        public int Wins { get; set; }
        public int Losses { get; set; }
        public float WinLossRatio { get; set; }
        public string UserID { get; set; }
        [NotMapped]
        public string UserName { get; set; }

        public Highscore()
        {
            Wins = 0;
            Losses = 0;
            WinLossRatio = 0;
        }
    }
}
