using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ReversiApp.Models
{
    public class AppUser
    {
        public string Id { get; set; }
        [Required]
        [Display(Name = "Username")]
        public string UserName { get; set; }
        [Required]
        [EmailAddress]
        public string Email { get; set; }
       
        [StringLength(100, ErrorMessage = "The {0} must be at least {2} and at max {1} characters long.", MinimumLength = 6)]
        [RegularExpression(@"^((?=.*[a-z])(?=.*[A-Z])(?=.*\d)).+$", ErrorMessage = "Must contain alteast 1 uppercase, lowercase letter and one numeric and non numeric digit.")]
        [DataType(DataType.Password)]
        public string Password { get; set; }
        public string Role { get; set; }
        public bool TwoFactorEnabled { get; set; }
    }
}
