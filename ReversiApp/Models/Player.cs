using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ReversiApp.Models
{
    public enum Kleur
    {
        Geen,
        Wit,
        Zwart
    }
    public class Player 
    {
        public string Id { get; set; }
        [DataType(DataType.EmailAddress)]
        public string Email { get; set; }
        [DataType(DataType.Password)]
        public string Password { get; set; }
        public string Token { get; set; }
        public Kleur Kleur { get; set; }
    }
}
